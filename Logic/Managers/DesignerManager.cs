using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace GodotUtils
{
    public class DesignAttribute : Attribute
    {
        public string Name;
        public string Parent;

        public DesignAttribute(string name = "", string parent ="")
        {
            Name = String.IsNullOrEmpty(name) ? null : name;
            Parent = String.IsNullOrEmpty(parent) ? null : parent;
        }
    }

    public static class DesignerManager
    {
        public static void InitializeComponents(Node input)
        {
            var dtype = typeof(DesignAttribute);
            var fields = input.GetType().
                GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).
                Where(f => f.IsDefined(dtype, false));

            foreach (var field in fields)
            {
                var ftype = field.FieldType;
                var dattrib = (DesignAttribute)field.GetCustomAttribute(dtype);
                var name = dattrib.Name ?? field.Name;

                var instance = dattrib.Parent == null ? 
                    input.GetNode(name) : input.GetNode(dattrib.Parent).GetNode(name);

                field.SetValue(input, instance);
            }
        }
    }
}
