using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GEP.ComponentModels;
using TreeGP.ComponentModels;

namespace GEP.AlgorithmModels.Crossover
{
    using System.Xml;
    using TreeGP.AlgorithmModels.Crossover;
    using TreeGP.Core.AlgorithmModels.Crossover;
    using TreeGP.Core.ComponentModels;
    public class GEPCrossoverInstructionFactory<P, S> : TGPCrossoverInstructionFactory<P, S>
        where S : GEPSolution
        where P : IGPPop
    {

        public GEPCrossoverInstructionFactory(string filename)
            : base(filename)
        {
            
        }

        public GEPCrossoverInstructionFactory()
        {

        }

        protected override CrossoverInstruction<P, S> CreateDefaultInstruction()
        {
            return new GEPCrossoverInstruction_OnePoint<P, S>();
        }

        protected override CrossoverInstruction<P, S> CreateInstructionFromXml(string strategy_name, XmlElement xml)
        {
            if (strategy_name == "one_point")
            {
                return new GEPCrossoverInstruction_OnePoint<P, S>(xml);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override CrossoverInstructionFactory<P, S> Clone()
        {
            GEPCrossoverInstructionFactory<P, S> clone = new GEPCrossoverInstructionFactory<P, S>(mFilename);
            return clone;
        }
    }
}
