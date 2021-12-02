using UnitsNet;

namespace UnitsConverter.Fluent.Plugin
{
    public class UnitConversionResult
    {
        public double ConvertedValue { get; }

        public string UnitName { get; }

        public QuantityType QuantityType { get; }

        public UnitConversionResult(double convertedValue, string unitName, QuantityType quantityType)
        {
            ConvertedValue = convertedValue;
            UnitName = unitName;
            QuantityType = quantityType;
        }

        public UnitConversionResult(IQuantity quantity)
        {
            String[] parts = quantity.ToString().Split(' ');

            ConvertedValue = quantity.Value;
            UnitName = parts[1];
            QuantityType = quantity.QuantityInfo.QuantityType;
        }
    }
}