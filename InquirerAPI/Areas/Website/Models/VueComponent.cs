using InquirerAPI.Website.ViewModels;
using System;

namespace InquirerAPI.Website.Models
{
    public class VueComponent
    {
        public string Name { get; set; }
        public Type Type { get; set; }

        public object Template { get; set; }
        public object Body { get; set; }

        public VueComponent(string name, Type type)
        {
            Name = name;
            Type = type;

            Template = Activator.CreateInstance(type, "template");
            Body = Activator.CreateInstance(type);
        }

        public static VueComponent Create<T>(string name) where T : new()
        {
            return new VueComponent(name, typeof(GenericViewModel<T>));
        }

        public static VueComponent Create(string name)
        {
            return new VueComponent(name, typeof(GenericViewModel));
        }
    }
}
