using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inquirer.ViewModels
{
    public class ApiCallViewModel
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public object Data { get; set; }
    }
}
