using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using BombServerEmu_MNR.Src.Log;

namespace BombServerEmu_MNR.Src.DataTypes
{
    class BombXml
    {
        public const string TRANSACTION_TYPE_REQUEST = "TRANSACTION_TYPE_REQUEST";
        public const string TRANSACTION_TYPE_REPLY = "TRANSACTION_TYPE_REPLY";
        public const string TRANSACTION_TYPE_NONE = "TRANSACTION_TYPE_NONE";

        XmlDocument reqDoc;
        XmlDocument resDoc;

        BombService service;

        public BombXml(BombService service, string reqDoc)
        {
            this.service = service;
            this.reqDoc = new XmlDocument();
            this.reqDoc.LoadXml(reqDoc);
            foreach (var element in this.reqDoc.GetElementsByTagName("param"))
            {
                Logging.Log(typeof(BombXml), "Name:{0} Value:{1}", LogType.Debug
                    , ((XmlElement)element).GetElementsByTagName("name")[0].InnerText
                    , ((XmlElement)element).GetElementsByTagName("value")[0].InnerText);
            }
            InitRes(service);
        }

        void InitRes(BombService service)
        {
            resDoc = new XmlDocument();
            var root = CreateXmlElement(ref resDoc, "service"); {
                root.SetAttribute("name", service.name);
                var trans = CreateXmlElement(ref resDoc, root, "transaction"); {
                    trans.SetAttribute("id", ((XmlElement)reqDoc.SelectSingleNode("service/transaction")).GetAttribute("id"));
                    trans.SetAttribute("type", TRANSACTION_TYPE_REPLY);
                    var method = CreateXmlElement(ref resDoc, trans, "method");
                }
            }
        }

        public void SetName(string name)
        {
            ((XmlElement)resDoc.SelectSingleNode("service")).SetAttribute("name", name);
        }

        public void SetTransactionType(string type)
        {
            ((XmlElement)resDoc.SelectSingleNode("service/transaction")).SetAttribute("type", type);
        }

        public string GetMethod()
        {
            return ((XmlElement)reqDoc.SelectSingleNode("service/transaction/method")).InnerText.Split(' ')[1];
        }

        public void SetMethod(string method)
        {
            ((XmlElement)resDoc.SelectSingleNode("service/transaction/method")).InnerText = string.Format(" {0} ", method);
        }

        public void SetError(string error)
        {
            CreateXmlElement(ref resDoc, ((XmlElement)resDoc.SelectSingleNode("service/transaction/method")), "error", string.Format(" {0} ", error));
        }

        public string GetParam(string name)
        {
            var paramNode = ((XmlElement)reqDoc.SelectSingleNode(string.Format("service/transaction[name='{0}'][1]", name)));
            if (paramNode != null) {
                return paramNode.Value;
            }
            else {
                Logging.Log(typeof(BombXml), "Param {0} does not exist in request!", LogType.Error, name);
                return null;
            }
        }

        public void AddParam(string name, object value)
        {
            var param = CreateXmlElement(ref resDoc, ((XmlElement)resDoc.SelectSingleNode("service/transaction/method")), "param"); {
                CreateXmlElement(ref resDoc, param, "name", string.Format(" {0} ", name));
                CreateXmlElement(ref resDoc, param, "value", string.Format(" {0} ", value));
            }
        }

        public string GetResDoc()
        {
            string xml = resDoc.OuterXml;
            Logging.Log(typeof(BombXml), xml, LogType.Debug);
            InitRes(service);
            return xml;
        }

        XmlElement CreateXmlElement(ref XmlDocument doc, string name, object text = null)
        {
            XmlElement element = doc.CreateElement(name);
            if (text != null) {
                element.InnerText = text.ToString();
            }
            doc.AppendChild(element);
            return element;
        }

        XmlElement CreateXmlElement(ref XmlDocument doc, XmlElement parent, string name, object text = null)
        {
            XmlElement element = doc.CreateElement(name);
            if (text != null) {
                element.InnerText = text.ToString();
            }
            parent.AppendChild(element);
            return element;
        }
    }
}
