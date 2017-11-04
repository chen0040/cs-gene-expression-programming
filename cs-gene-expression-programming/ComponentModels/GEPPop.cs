using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEP.ComponentModels
{
    using TreeGP.ComponentModels;
    using TreeGP.AlgorithmModels.PopInit;
    using TreeGP.AlgorithmModels.Mutation;
    using TreeGP.AlgorithmModels.Crossover;

    using TreeGP;

    using GEP.AlgorithmModels.PopInit;
    using GEP.AlgorithmModels.Mutation;
    using GEP.AlgorithmModels.Crossover;

    using TreeGP.Core.AlgorithmModels.PopInit;
    using TreeGP.Core.ComponentModels;
    using TreeGP.Core.AlgorithmModels.Mutation;
    using TreeGP.Core.AlgorithmModels.Crossover;
    public class GEPPop<S> : TGPPop<S>
        where S : GEPSolution, new()
    {
        public GEPPop(GEPConfig config)
            : base(config)
        {

        }

        private List<TGPPrimitive> mChromosomeBasis = new List<TGPPrimitive>();

        public void BuildChromosomeBasis()
        {
            mChromosomeBasis.Clear();

            int op_count = mOperatorSet.OperatorCount;
            for (int i = 0; i < op_count; ++i)
            {
                mChromosomeBasis.Add(mOperatorSet.FindOperatorByIndex(i));
            }
            List<string> variable_names = mVariableSet.TerminalNames;
            foreach(string variable_name in variable_names)
            {
                mChromosomeBasis.Add(mVariableSet.FindTerminalBySymbol(variable_name));
            }
            int constant_count = mConstantSet.TerminalCount;
            for (int i = 0; i < constant_count; ++i)
            {
                mChromosomeBasis.Add(mConstantSet.FindTerminalByIndex(i));
            }
        }

        private GEPConfig GEConfig
        {
            get { return (GEPConfig)mConfig; }
        }

        public int ChromosomeValueUpperBound
        {

            get { return mChromosomeBasis.Count; }
        }

        public int ChromosomeLength
        {
            get { return GEConfig.ChromosomeLength; }
        }

        public override object CreateProgram()
        {
            GEPProgram program = new GEPProgram(mOperatorSet, mVariableSet, mConstantSet, mPrimitiveSet, mChromosomeBasis, ChromosomeValueUpperBound);
            return program;
        }

        protected override PopInitInstructionFactory<IGPPop, S> CreatePopInitInstructionFactory(string filename)
        {
            GEPPopInitInstructionFactory<IGPPop, S> factory = new GEPPopInitInstructionFactory<IGPPop, S>(filename);
            factory.ChromosomeLengthRequested += () =>
                {
                    return GEConfig.ChromosomeLength;
                };
            return factory;
        }

        protected override MutationInstructionFactory<IGPPop, S> CreateMutationInstructionFactory(string filename)
        {
            return new GEPMutationInstructionFactory<IGPPop, S>(filename);
        }

        protected override CrossoverInstructionFactory<IGPPop, S> CreateCrossoverInstructionFactory(string filename)
        {
            return new GEPCrossoverInstructionFactory<IGPPop, S>(filename);
        }

       
        public override ISolution CreateSolution()
        {
            return mSolutionFactory.Create(this, mTreeCount);
        }
        
    }
}
