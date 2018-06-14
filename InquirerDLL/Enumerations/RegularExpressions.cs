using System;
using System.Collections.Generic;
using System.Text;

namespace InquirerDLL.Enumerations
{
    public static class REGULAR_EXPRESSIONS
    {
        public static string ALPHA = @"^\w+([ ]{1}\w+)*$";
        public static string IP = @"^[0-9.]+&";
    }
}
