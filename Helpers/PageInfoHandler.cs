using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml;

namespace AdminPanel.Helpers
{
    public class PageInfoHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            throw new NotImplementedException();
        }
    }
}