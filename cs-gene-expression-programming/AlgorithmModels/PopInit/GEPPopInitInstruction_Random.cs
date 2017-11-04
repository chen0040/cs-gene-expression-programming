using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreeGP.ComponentModels;

namespace GEP.AlgorithmModels.PopInit
{
    using System.Xml;
    using TreeGP.AlgorithmModels.PopInit;
    using GEP.ComponentModels;
    using TreeGP;
    using TreeGP.Core.ComponentModels;
    using TreeGP.Core.AlgorithmModels.PopInit;
    public class GEPPopInitInstruction_Random<P, S> : PopInitInstruction<P, S>
        where S : GEPSolution
        where P : IGPPop
    {
        public delegate int ChromosomeLengthRequestedHandle();
        public event ChromosomeLengthRequestedHandle ChromosomeLengthRequested;

        public GEPPopInitInstruction_Random()
        {

        }

        public GEPPopInitInstruction_Random(XmlElement xml_level1)
            : base(xml_level1)
        {
            
        }

        public override void Initialize(P pop)
        {
            int iPopulationSize = pop.PopulationSize;

            int chromosome_length = ChromosomeLengthRequested();


            for (int i = 0; i < iPopulationSize; i++)
            {
                S program = pop.CreateSolution() as S;
                program.CreateRandomly(chromosome_length);
                pop.AddSolution(program);
            }
        }

        public override PopInitInstruction<P, S> Clone()
        {
            GEPPopInitInstruction_Random<P, S> clone = new GEPPopInitInstruction_Random<P, S>();
            return clone;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(">> Name: GEPopInstruction_MaximumInitialization\n");

            return sb.ToString();
        }
    }
}
