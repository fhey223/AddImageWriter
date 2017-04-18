using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{

    /// <summary>
    /// TODOCustomConfigurationFirst
    /// by gouyf
    /// 2016/12/20 9:25:03
    /// </summary>
    public class CustomConfigurationFirst : ConfigurationSection
    {
        private static CustomConfigurationFirst setting;
        public static CustomConfigurationFirst Setting
        {
            get
            {
                if (setting == null)
                    setting = (CustomConfigurationFirst)ConfigurationManager.GetSection("firstCustomConfiguration");
                return setting;
            }
        }

        [ConfigurationProperty("id", DefaultValue = "1", IsRequired = true)]
        public long Id
        {
            get { return (long)this["id"]; }
            set { this["id"] = value; }
        }

        [ConfigurationProperty("name", DefaultValue = "Lily", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("firstProperty", DefaultValue = "Property1", IsRequired = true)]
        public string FirstProperty
        {
            get { return (string)this["firstProperty"]; }
            set { this["firstProperty"] = value; }
        }
    }
}
