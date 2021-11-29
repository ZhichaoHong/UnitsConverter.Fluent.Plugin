using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitsConverter.Fluent.Plugin
{
    public class ConvertModel
    {
        public double Value { get; set; }

        public string FromUnit { get; set; }

        public string ToUnit { get; set; }

        public ConvertModel()
        {
        }

        public ConvertModel(double value, string fromUnit, string toUnit)
        {
            Value = value;
            FromUnit = fromUnit;
            ToUnit = toUnit;
        }
    }
}
