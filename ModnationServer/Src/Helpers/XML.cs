using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModnationServer.Src.Helpers
{
    class XML
    {
        const string XML_VERSION = "1.0";

        public XmlDocument Document { get; }
        public Encoding Encoding { get; }
        //public bool IsFromData { get; }   //TODO: Is this needed?

        public XML()
        {
            Document = new XmlDocument();
        }
        public XML(byte[] data, Encoding enc)
        {
            Document = new XmlDocument();
            Encoding = enc;
            Document.LoadXml(Encoding.GetString(data));
        }

        public byte[] Serialize()
        {
            Document.CreateXmlDeclaration(XML_VERSION, Encoding.BodyName, null);
            return Encoding.GetBytes(Document.OuterXml);
        }
    }
}
