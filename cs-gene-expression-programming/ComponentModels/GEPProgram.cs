using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEP.ComponentModels
{
    using TreeGP.ComponentModels;
    using TreeGP;
    using TreeGP.Core.ProblemModels;
    using TreeGP.Distribution;
    public class GEPProgram : TGPProgram
    {
        public List<int> mChromosome = new List<int>();
        protected List<TGPPrimitive> mChromosomeBasis;
        protected int mChromosomeValueUpperBound;

        public GEPProgram(TGPOperatorSet os, TGPVariableSet vs, TGPConstantSet cs, List<KeyValuePair<TGPPrimitive, double>> ps, List<TGPPrimitive> basis, int ChromosomeValueUpperBound)
            : base(os, vs, cs, ps)
        {
            mChromosomeBasis = basis;
            mChromosomeValueUpperBound = ChromosomeValueUpperBound;
        }

        protected int TerminalCountWithinChromosomeBasis
        {
            get
            {
                return mVariableSet.TerminalCount + mConstantSet.TerminalCount;
            }
        }

        public List<int> Chromosome
        {
            get { return mChromosome; }
        }

        protected TGPPrimitive FindAtChromosomeBasisIndex(int p)
        {
            return mChromosomeBasis[p];
        }

        public override object ExecuteOnFitnessCase(IGPFitnessCase fitness_case, params object[] tags)
        {
            Express();
            return base.ExecuteOnFitnessCase(fitness_case, tags);
        }

        public virtual object Execute(Dictionary<string, object> variables, params object[] tags)
        {
            Express();
            List<string> variable_names = mVariableSet.TerminalNames;
            foreach(string variable_name in variable_names)
            {
                if (variables.ContainsKey(variable_name))
                {
                    mVariableSet.FindTerminalBySymbol(variable_name).Value = variables[variable_name];
                }
                else
                {
                    mVariableSet.FindTerminalBySymbol(variable_name).Value = 0;
                }
            }
            return mRootNode.Evaluate();
        }

        public override TGPProgram Clone()
        {
            GEPProgram program = new GEPProgram(mOperatorSet, mVariableSet, mConstantSet, mPrimitiveSet, mChromosomeBasis, mChromosomeValueUpperBound);
            program.Copy(this);
            return program;
        }

        public GEPProgram CloneWithoutGPTree()
        {
            GEPProgram clone = new GEPProgram(mOperatorSet, mVariableSet, mConstantSet, mPrimitiveSet, mChromosomeBasis, mChromosomeValueUpperBound);

            clone.mChromosome = new List<int>();
            int chromosome_length = mChromosome.Count;
            for (int i = 0; i < chromosome_length; ++i)
            {
                clone.mChromosome.Add(mChromosome[i]);
            }

            clone.mDepth = mDepth;
            clone.mLength = mLength;

            return clone;
        }

        public override void Copy(TGPProgram rhs)
        {
            base.Copy(rhs);

            GEPProgram rhs_=(GEPProgram)rhs;
            mChromosome = new List<int>();
            int chromosome_length=rhs_.mChromosome.Count;
            for (int i = 0; i < chromosome_length; ++i)
            {
                mChromosome.Add(rhs_.mChromosome[i]);
            }

            
        }


        protected bool IsOperatorAtChromosomeBasisIndex(int i)
        {
            return !mChromosomeBasis[i].IsTerminal;
        }

        protected TGPPrimitive FindTerminalWithinChromosomeBasis(int gene_index)
        {
            return mChromosomeBasis[mOperatorSet.OperatorCount + gene_index];
        }

        protected virtual void Express()
        {
            int codon_length=mChromosome.Count;

            bool is_head_found = false;
            int head_index = 0;
            //int tail_index = 0;
            for (int i = 0; i < mChromosome.Count; ++i)
            {
                int gene_value=mChromosome[i];
                if (IsOperatorAtChromosomeBasisIndex(gene_value))
                {
                    head_index = i;
                    is_head_found = true;
                    break;
                }
            }

            int terminal_count = TerminalCountWithinChromosomeBasis;
            if (is_head_found)
            {
                Dictionary<TGPNode, int> node_index_mapping = new Dictionary<TGPNode, int>();

                TGPOperator op=(TGPOperator)FindAtChromosomeBasisIndex(mChromosome[head_index]);
                TGPNode node = new TGPNode(op);
                mRootNode =node;
                int pointer_index = head_index;

                node_index_mapping[node] = pointer_index;

                Queue<TGPNode> node_stack = new Queue<TGPNode>();
                node_stack.Enqueue(node);
                bool can_grow = true;
                
                while (node_stack.Count > 0)
                {
                    node = node_stack.Dequeue();
                    if (can_grow)
                    {
                        for (int i = 0; i < node.Arity; ++i)
                        {
                            pointer_index++;
                            if (pointer_index >= mChromosome.Count)
                            {
                                can_grow = false;
                                node.RemoveAllChildren();
                                break;
                            }
                            TGPNode child_node = node.CreateChild(FindAtChromosomeBasisIndex(mChromosome[pointer_index]));
                            node_index_mapping[child_node] = pointer_index;
                            node_stack.Enqueue(child_node);
                        }
                        if (!can_grow)
                        {
                            int gene_index = mChromosome[node_index_mapping[node]];
                            gene_index = gene_index % terminal_count;
                            node.Primitive = FindTerminalWithinChromosomeBasis(gene_index);
                        }
                    }
                    else
                    {
                        int gene_index = mChromosome[node_index_mapping[node]];
                        gene_index = gene_index % terminal_count;
                        node.Primitive=FindTerminalWithinChromosomeBasis(gene_index);
                    }
                }
            }
            else
            {
                mRootNode = new TGPNode(FindAtChromosomeBasisIndex(mChromosome[0]));
            }
            CalcDepth();
            CalcLength();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int chromosome_length = mChromosome.Count;
            sb.Append("[");
            for (int i = 0; i < chromosome_length; ++i)
            {
                if (i == 0)
                {
                    sb.AppendFormat("{0}", mChromosome[i]);
                }
                else
                {
                    sb.AppendFormat(", {0}", mChromosome[i]);
                }
            }
            sb.Append("]");
            sb.AppendFormat("\n{0}", base.ToString());

            return sb.ToString();
        }

        internal void CreateRandomly(int iMaximumDepthForCreation)
        {
            int upper_bound = CodonGeneUpperBound;
            for (int i = 0; i < iMaximumDepthForCreation; ++i)
            {
                mChromosome.Add(DistributionModel.NextInt(upper_bound));
            }
        }

        public override void MicroMutate()
        {
            
        }

        public int CodonGeneUpperBound
        {
            get { return mChromosomeValueUpperBound; }
        }
    }
}
