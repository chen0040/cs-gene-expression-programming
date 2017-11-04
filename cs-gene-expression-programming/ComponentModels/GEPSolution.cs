using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreeGP.ComponentModels;
using TreeGP;
using TreeGP.Core.ComponentModels;
using TreeGP.Distribution;

namespace GEP.ComponentModels
{
    public class GEPSolution : TGPSolution
    {
        public GEPSolution()
        {

        }

        protected GEPSolution(IGPPop pop, int tree_count)
            : base(pop, tree_count)
        {

        }

        public override ISolution Create(IGPPop pop, int tree_count)
        {
            return new GEPSolution(pop, tree_count);
        }

        public override ISolution Clone()
        {
            GEPSolution clone = new GEPSolution(mPop, mTrees.Count);
            clone.Copy(this);
            return clone;
        }


        public GEPSolution CloneWithoutGPTree()
        {
            GEPSolution clone = new GEPSolution(mPop, mTrees.Count);

            clone.mTrees.Clear();
            for (int i = 0; i < mTrees.Count; ++i)
            {
                clone.mTrees.Add(((GEPProgram)mTrees[i]).CloneWithoutGPTree());
            }

            clone.mFitness = mFitness;
            clone.mIsFitnessValid = mIsFitnessValid;
            clone.mObjectiveValue = mObjectiveValue;

            foreach (string attrname in mAttributes.Keys)
            {
                clone.mAttributes[attrname] = mAttributes[attrname];
            }

            return clone;
        }

        public void UniformMutate()
        {
            for (int tindex = 0; tindex < mTrees.Count; ++tindex)
            {
                GEPProgram program = (GEPProgram)mTrees[tindex];

                double mutation_rate = mPop.MacroMutationRate;

                int upper_bound = program.CodonGeneUpperBound;

                List<int> codon = program.Chromosome;
                for (int i = 0; i < codon.Count; ++i)
                {
                    if (DistributionModel.GetUniform() < mutation_rate)
                    {
                        codon[i] = DistributionModel.NextInt(upper_bound);
                    }
                }
            }

            TrashFitness();
        }

        public void OnePointCrossover(GEPSolution rhs)
        {
            for (int tindex = 0; tindex < mTrees.Count; ++tindex)
            {
                GEPProgram gp1 = (GEPProgram)mTrees[tindex];
                GEPProgram gp2 = (GEPProgram)rhs.mTrees[tindex];

                List<int> codon1 = gp1.Chromosome;
                List<int> codon2 = gp2.Chromosome;

                int cut_point_index = 1 + DistributionModel.NextInt(codon1.Count - 2);

                for (int i = cut_point_index; i < codon1.Count; ++i)
                {
                    int temp = codon1[i];
                    codon1[i] = codon2[i];
                    codon2[i] = temp;
                }
            }


            TrashFitness();
            rhs.TrashFitness();
        }

        

        internal void CreateRandomly(int iMaximumDepthForCreation)
        {
            for (int i = 0; i < mTrees.Count; ++i)
            {
                ((GEPProgram)mTrees[i]).CreateRandomly(iMaximumDepthForCreation);
            }

            TrashFitness();
        }
    }
}
