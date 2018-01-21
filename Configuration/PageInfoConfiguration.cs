using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AdminPanel.Configuration
{
    public class PageInfoConfigurationElement : ConfigurationSection
    {
        [ConfigurationProperty("id", IsRequired = true)]
        public string Id
        {
            get { return this["id"] as string; }
        }

        [ConfigurationProperty("communityName", IsRequired = true)]
        public string Title
        {
            get { return this["communityName"] as string;  }
        }
    }

    public class PanelInfoConfigurationCollection : ConfigurationElementCollection
    {
        public PageInfoConfigurationElement this[int index]
        {
            get { return BaseGet(index) as PageInfoConfigurationElement; }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new PageInfoConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PageInfoConfigurationElement)element).Id;
        }
    }
}