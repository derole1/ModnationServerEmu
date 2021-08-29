using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace ModnationServer.Src.Services.Models
{
    class preferences_update_req
    {
        public string preference_language_code { get; }
        public string preference_timezone { get; }
        public string preference_region_code { get; }
        public string preference_domain { get; }
    }


    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class preferences_update_res
    {

        private string language_codeField;

        private short timezoneField;

        private string region_codeField;

        private string domainField;

        private string ip_addressField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string language_code
        {
            get
            {
                return this.language_codeField;
            }
            set
            {
                this.language_codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public short timezone
        {
            get
            {
                return this.timezoneField;
            }
            set
            {
                this.timezoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string region_code
        {
            get
            {
                return this.region_codeField;
            }
            set
            {
                this.region_codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string domain
        {
            get
            {
                return this.domainField;
            }
            set
            {
                this.domainField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ip_address
        {
            get
            {
                return this.ip_addressField;
            }
            set
            {
                this.ip_addressField = value;
            }
        }
    }
}
