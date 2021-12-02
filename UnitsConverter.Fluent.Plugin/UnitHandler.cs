// The implementation is largely borrowed from Powertoys Run units converter plugin and adapted to FS plugins architecture.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using UnitsNet.Units;

namespace UnitsConverter.Fluent.Plugin
{
    public static class UnitHandler
    {

        private static readonly QuantityType[] _included = new QuantityType[]
        {
            QuantityType.Acceleration,
            QuantityType.Angle,
            QuantityType.Area,
            QuantityType.Duration,
            QuantityType.Energy,
            QuantityType.Information,
            QuantityType.Length,
            QuantityType.Mass,
            QuantityType.Power,
            QuantityType.Pressure,
            QuantityType.Speed,
            QuantityType.Temperature,
            QuantityType.Volume,
        };

        /// <summary>
        /// Given string representation of unit, converts it to the enum.
        /// </summary>
        /// <returns>Corresponding enum or null.</returns>
        private static Enum GetUnitEnum(string unit, QuantityInfo unitInfo)
        {
            if (unit == null)
            {
                return null;
            }

            UnitInfo first = Array.Find(unitInfo.UnitInfos, info => info.Name.ToLower() == unit.ToLower());
            if (first != null)
            {
                return first.Value;
            }

            if (UnitParser.Default.TryParse(unit, unitInfo.UnitType, out Enum enum_unit))
            {
                return enum_unit;
            }

            return null;
        }

        /// <summary>
        /// Given parsed ConvertModel, computes result. (E.g "1 foot in cm").
        /// </summary>
        /// <returns>The converted value as a double.</returns>
        public static double ConvertInput(ConvertModel convertModel, QuantityType quantityType)
        {
            QuantityInfo unitInfo = Quantity.GetInfo(quantityType);

            var fromUnit = GetUnitEnum(convertModel.FromUnit, unitInfo);
            var toUnit = GetUnitEnum(convertModel.ToUnit, unitInfo);

            if (fromUnit != null && toUnit != null)
            {
                return UnitsNet.UnitConverter.Convert(convertModel.Value, fromUnit, toUnit);
            }

            return double.NaN;
        }

        private static IEnumerable<IQuantity> ConvertFromUnit(ConvertModel convertModel, QuantityType quantityType)
        {
            var results = new List<IQuantity>();
            QuantityInfo unitInfo = Quantity.GetInfo(quantityType);

            var fromUnit = GetUnitEnum(convertModel.FromUnit, unitInfo);
            if (fromUnit != null)
            {
                var quantity = Quantity.From(convertModel.Value, fromUnit);
                string fromUnitStr = fromUnit.ToString().ToLower();
                foreach (var toUnit in quantity.QuantityInfo.UnitInfos)
                {
                    string toUnitName = toUnit.Name.ToLower();
                    if (fromUnit.Equals(toUnit)  || toUnitName == "undefined") continue;
                    var convertedValue = UnitConverter.Convert(convertModel.Value, fromUnit, toUnit.Value);
                    results.Add(Quantity.From(convertedValue, toUnit.Value));
                }
            }
            return results;
        }

        /// <summary>
        /// Given ConvertModel returns collection of possible results.
        /// </summary>
        /// <returns>The converted value as a double.</returns>
        public static IEnumerable<IQuantity> Convert(ConvertModel convertModel)
        {
            var results = new List<IQuantity>();
            if (convertModel.ToUnit != null)
            {
                foreach (QuantityType quantityType in _included)
                {
                    double convertedValue = UnitHandler.ConvertInput(convertModel, quantityType);
                    QuantityInfo unitInfo = Quantity.GetInfo(quantityType);
                    var toUnit = GetUnitEnum(convertModel.ToUnit, unitInfo);

                    if (!double.IsNaN(convertedValue))
                    {
                        results.Add(Quantity.From(convertedValue, toUnit));
                    }
                }
            }
            else
            {
                foreach (QuantityType quantityType in _included)
                {
                    results.AddRange(ConvertFromUnit(convertModel, quantityType).ToList());
                }
            }

            return results;
        }
    }
}
