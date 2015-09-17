using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SedcServer
{
    public class QueryParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var p = obj as QueryParameter;
            if ((System.Object)p == null)
            {
                return false;
            }

            return (Name == p.Name) && (Value == p.Value);
        }
    }
}
