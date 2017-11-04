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
    using TreeGP.Core.ComponentModels;
    using TreeGP.Core.AlgorithmModels.PopInit;
    public class GEPPopInitInstructionFactory<P, S> : TGPPopInitInstructionFactory<P, S>
        where S : GEPSolution
        where P : IGPPop
    {
        public delegate int ChromosomeLengthRequestedHandle();
        public event ChromosomeLengthRequestedHandle ChromosomeLengthRequested;

        public GEPPopInitInstructionFactory()
        {

        }

        public GEPPopInitInstructionFactory(string filename)
            : base(filename)
        {
            
        }

        protected override PopInitInstruction<P, S> LoadDefaultInstruction()
        {
            GEPPopInitInstruction_Random<P, S> instruction = new GEPPopInitInstruction_Random<P, S>();
            instruction.ChromosomeLengthRequested += () =>
                {
                    return ChromosomeLengthRequested();
                };
            return instruction;
        }

        protected override PopInitInstruction<P, S> LoadInstructionFromXml(string selected_strategy, XmlElement xml_level1)
        {
            if (selected_strategy == "random")
            {
                GEPPopInitInstruction_Random<P, S> instruction = new GEPPopInitInstruction_Random<P, S>(xml_level1);
                instruction.ChromosomeLengthRequested += () =>
                {
                    return ChromosomeLengthRequested();
                };
                return instruction;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override PopInitInstructionFactory<P, S> Clone()
        {
            GEPPopInitInstructionFactory<P, S> clone = new GEPPopInitInstructionFactory<P, S>(mFilename);
            return clone;
        }
    }
}
