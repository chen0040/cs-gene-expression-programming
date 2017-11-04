using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreeGP.ComponentModels;
using System.Xml;

namespace GEP.ComponentModels
{
    public class GEPConfig : TGPConfig
    {
        private int mChromosomeLength = 10;

        public int ChromosomeLength
        {
            get { return mChromosomeLength; }
        }


        public GEPConfig(string filename)
            : base(filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlElement doc_root = doc.DocumentElement;

            foreach (XmlElement xml_level1 in doc_root.ChildNodes)
            {
                if (xml_level1.Name == "parameters")
                {
                    foreach (XmlElement xml_level2 in xml_level1.ChildNodes)
                    {
                        if (xml_level2.Name == "param")
                        {
                            string attrname = xml_level2.Attributes["name"].Value;
                            string attrvalue = xml_level2.Attributes["value"].Value;
                            if (attrname == "ChromosomeLength")
                            {
                                int value = 0;
                                int.TryParse(attrvalue, out value);
                                mChromosomeLength = value;
                            }
                        }
                    }
                }
            }
        }
    }
}
