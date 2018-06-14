using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InquirerAPI.Website.ViewModels
{
    public class GenericViewModel
    {
        public string Section { get; set; } = "";

        public GenericViewModel()
        {
        }

        public GenericViewModel(string section)
        {
            Section = section;
        }
    }

    public class GenericViewModel<T> : GenericViewModel where T : new()
    {
        public T Data { get; set; }

        public GenericViewModel() : base()
        {
            Data = new T();
        }

        public GenericViewModel(T data)
        {
            Data = data;
        }

        public GenericViewModel(string section, T data) : base(section)
        {
            Data = data;
        }

        public GenericViewModel(string section) : base(section)
        {
            Data = new T();
        }
    }
}
