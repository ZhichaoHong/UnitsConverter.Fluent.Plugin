﻿using UnitsNet;

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
    }
}