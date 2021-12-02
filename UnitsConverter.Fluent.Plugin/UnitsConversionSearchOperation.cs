using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Blast.Core.Results;

namespace UnitsConverter.Fluent.Plugin
{
    public class UnitsConversionSearchOperation : SearchOperationBase
    {
        public UnitsConversionSearchOperation() : base("UnitsConverter", "Converts units in a given quantity type", "\uE8EF")
        {
            HideMainWindow = false;
        }

    }
}
