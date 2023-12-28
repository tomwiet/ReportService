﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReportService.Extensions
{
    public static class StringExtensions
    {
        public static string StripHTML(this string input) 
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }

    }
}
