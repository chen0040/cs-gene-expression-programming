using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEP.AlgorithmModels.Crossover
{
    using System.Xml;
    using GEP.ComponentModels;
    using TreeGP.AlgorithmModels.Crossover;
    using TreeGP;
    using TreeGP.ComponentModels;
    using TreeGP.Core.AlgorithmModels.Crossover;
    using TreeGP.Core.ComponentModels;
    using TreeGP.Distribution;
    public class GEPCrossoverInstruction_OnePoint<P, S> : CrossoverInstruction<P, S>
        where S : GEPSolution
        where P : IGPPop
    {
        public GEPCrossoverInstruction_OnePoint()
            : base()
        {
           
        }

        public GEPCrossoverInstruction_OnePoint(XmlElement xml_level1)
            : base()
        {
            
        }

        public override CrossoverInstruction<P, S> Clone()
        {
            GEPCrossoverInstruction_OnePoint<P, S> clone = new GEPCrossoverInstruction_OnePoint<P, S>();
            return clone;
        }

        public override List<S> Crossover(P pop, params S[] parents)
        {
            S gp1 = parents[0].CloneWithoutGPTree() as S;
            S gp2 = parents[1].CloneWithoutGPTree() as S;

            if (DistributionModel.GetUniform() < pop.CrossoverRate)
            {
                gp1.OnePointCrossover(gp2);
            }

            List<S> children = new List<S>();
            children.Add(gp1);
            children.Add(gp2);
            return children;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(">> Name: GECrossoverInstruction_OnePoint\n");
            
            return sb.ToString();
        }
    }
}
