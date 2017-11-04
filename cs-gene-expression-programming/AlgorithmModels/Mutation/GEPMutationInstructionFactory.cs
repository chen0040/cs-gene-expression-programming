using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GEP.ComponentModels;
using TreeGP.ComponentModels;

namespace GEP.AlgorithmModels.Mutation
{
    using System.Xml;
    using TreeGP.ComponentModels;
    using TreeGP.AlgorithmModels.Mutation;
    using TreeGP.Core.ComponentModels;
    using TreeGP.Core.AlgorithmModels.Mutation;

    public class GEPMutationInstructionFactory<P, S> : TGPMutationInstructionFactory<P, S>
        where S : GEPSolution
        where P : IGPPop
    {
        public GEPMutationInstructionFactory(string filename)
            : base(filename)
        {
           
        }

        protected override MutationInstruction<P, S> LoadDefaultInstruction()
        {
            return new GEPMutationInstruction_UniformRandom<P, S>();
        }

        protected override MutationInstruction<P, S> LoadInstructionFromXml(string selected_strategy, XmlElement xml)
        {
            if (selected_strategy == "uniform_random")
            {
                return new GEPMutationInstruction_UniformRandom<P, S>(xml);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override MutationInstructionFactory<P, S> Clone()
        {
            GEPMutationInstructionFactory<P, S> clone = new GEPMutationInstructionFactory<P, S>(mFilename);
            return clone;
        }


    }
}
