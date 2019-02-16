using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Html2Markdown
{
    class Page : HeadItem
    {
        public string NameWithoutExtension
        {
            get
            {
                if (Name.Contains("."))
                {
                    var name = Name.Split('.');
                    return name[0];
                }
                return Name;
            }
        }
    }
}
