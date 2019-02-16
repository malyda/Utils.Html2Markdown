using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Html2Markdown
{
    class Helpers
    {
        public static string CurrentDateFileName
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        public static string CurrentDateURL
        {
            get
            {
                return DateTime.Now.ToString("yyyy'/'MM'/'dd");
            }
        }
    }
}
