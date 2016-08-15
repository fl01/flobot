using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flobot.InversionOfControl
{
    public class NameValueParameter
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public NameValueParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
