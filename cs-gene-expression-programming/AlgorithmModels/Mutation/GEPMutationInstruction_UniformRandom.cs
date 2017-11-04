using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEP.AlgorithmModels.Mutation
{
    using System.Xml;
    using GEP.ComponentModels;
    using TreeGP.AlgorithmModels.Mutation;
    using TreeGP;
    using TreeGP.ComponentModels;
    using TreeGP.Core.AlgorithmModels.Mutation;
    using TreeGP.Core.ComponentModels;

    public class GEPMutationInstruction_UniformRandom<P, S> : MutationInstruction<P, S>
        where S : GEPSolution
        where P : IGPPop
    {
        public GEPMutationInstruction_UniformRandom()
        {
            
        }

        public GEPMutationInstruction_UniformRandom(XmlElement xml_level1)
            : base(xml_level1)
        {
            
        }

        public override void Mutate(P pop, S child)
        {
            GEPSolution program = (GEPSolution)child;
            program.UniformMutate();
        }

        public override MutationInstruction<P, S> Clone()
        {
            GEPMutationInstruction_UniformRandom<P, S> clone = new GEPMutationInstruction_UniformRandom<P, S>();
            return clone;
        }
    }
}
