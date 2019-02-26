using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Html2Markdown
{
    class DateHelpers
    {
        static DateTime hardDate = new DateTime(2019, 02, 16);
        public static string DateFileName
        {
            get
            {
                return hardDate.ToString("yyyy-MM-dd");
            }
        }

        public static string DateURL
        {
            get
            {
                return hardDate.ToString("yyyy'/'MM'/'dd");
            }
        }
    }
}
