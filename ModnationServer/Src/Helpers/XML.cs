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
        public XmlElement Root { get; }
        //public bool IsFromData { get; }   //TODO: Is this needed?

        public XML(Encoding enc)
        {
            Document = new XmlDocument();
            Encoding = enc;
            Root = CreateRoot("result");    //Create the root node
        }
        public XML(byte[] data, Encoding enc)
        {
            Document = new XmlDocument();
            Encoding = enc;
            Document.LoadXml(Encoding.GetString(data));
        }

        public void SetResult(int id)
        {
            var status = CreateElement("status");
            CreateElement(status, "id", id.ToString());
            CreateElement(status, "message", "Successful completion"); //TODO
        }

        public XmlElement CreateRoot(string name, string text = null, params KeyValuePair<string, string>[] attributes)
        {
            var element = Document.CreateElement(name);
            if (text != null)
                element.InnerText = text;
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                    element.SetAttribute(attribute.Key, attribute.Value);
            }
            Document.AppendChild(element);
            return element;
        }

        public XmlElement CreateElement(XmlElement parent, string name, string text = null, params KeyValuePair<string, string>[] attributes)
        {
            var element =  Document.CreateElement(name);
            if (text != null)
                element.InnerText = text;
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                    element.SetAttribute(attribute.Key, attribute.Value);
            }
            if (parent != null)
                parent.AppendChild(element);
            else
                Root.AppendChild(element);
            return element;
        }

        public XmlElement CreateElement(string name, string text = null, params KeyValuePair<string, string>[] attributes) => CreateElement(null, name, text, attributes);

        public byte[] Serialize()
        {
            Document.CreateXmlDeclaration(XML_VERSION, Encoding.BodyName, null);
            return Encoding.GetBytes(Document.OuterXml);
        }
    }
}
