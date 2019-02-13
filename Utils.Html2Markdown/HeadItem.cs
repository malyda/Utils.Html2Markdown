using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Html2Markdown
{
    class HeadItem
    {
        public string Title { get; set; }
        public string Name { get; set; }

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
