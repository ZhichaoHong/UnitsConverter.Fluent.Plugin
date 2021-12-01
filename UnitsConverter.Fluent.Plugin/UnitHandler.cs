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
        private static readonly int _roundingFractionalDigits = 4;

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
                foreach (var tounit in quantity.QuantityInfo.UnitInfos)
                {
                    if (fromUnit == tounit.Value || tounit.Name.ToLower() == "undefined") continue;
                    var convertedValue = UnitConverter.Convert(convertModel.Value, fromUnit, tounit.Value);
                    results.Add(Quantity.From(convertedValue, tounit.Value));
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
                        //UnitConversionResult result = new UnitConversionResult(Math.Round(convertedValue, _roundingFractionalDigits), convertModel.ToUnit, quantityType);
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

        public static List<IQuantity> ConvertAll(string quantity, QuantityType quantityType, string toUnit)
        {
            List<IQuantity> result = new List<IQuantity>();
            switch (quantityType)
            {
                case QuantityType.Acceleration:
                    Acceleration accel;
                    if (Acceleration.TryParse(quantity, out accel))
                    {
                        AccelerationUnit tu;
                        if (UnitParser.Default.TryParse<AccelerationUnit>(toUnit, out tu))
                        {
                            result.Add(accel.ToUnit(tu));
                            foreach (var unit in Acceleration.Units)
                            {
                                if (accel.Unit == unit || tu == unit) continue;
                                result.Add(accel.ToUnit(unit));
                            }
                        }
                    }
                    break;

                case QuantityType.Angle:
                    Angle angle;
                    if (Angle.TryParse(quantity, out angle))
                    {
                        AngleUnit tu;
                        if (UnitParser.Default.TryParse<AngleUnit>(toUnit, out tu))
                        {
                            result.Add(angle.ToUnit(tu));
                            foreach (var unit in Angle.Units)
                            {
                                if (angle.Unit == unit || tu == unit) continue;
                                result.Add(angle.ToUnit(unit));
                            }
                        }
                    }
                    break;

                case QuantityType.Area:
                    Area area;
                    if (Area.TryParse(quantity, out area))
                    {
                        AreaUnit tu;
                        if (UnitParser.Default.TryParse<AreaUnit>(toUnit, out tu))
                        {
                            result.Add(area.ToUnit(tu));
                            foreach (var unit in Area.Units)
                            {
                                if (area.Unit == unit || tu == unit) continue;
                                result.Add(area.ToUnit(unit));
                            }
                        }
                    }
                    break;

                case QuantityType.Duration:
                    Duration duration;
                    if (Duration.TryParse(quantity, out duration))
                    {
                        DurationUnit tu;
                        if (UnitParser.Default.TryParse<DurationUnit>(toUnit, out tu))
                        {
                            result.Add(duration.ToUnit(tu));
                            foreach (var unit in Duration.Units)
                            {
                                if (duration.Unit == unit || tu == unit) continue;
                                result.Add(duration.ToUnit(unit));
                            }
                        }
                    }
                    break;

                case QuantityType.Energy:
                    Energy energy;
                    if (Energy.TryParse(quantity, out energy))
                    {
                        EnergyUnit tu;
                        if (UnitParser.Default.TryParse<EnergyUnit>(toUnit, out tu))
                        {
                            result.Add(energy.ToUnit(tu));
                            foreach (var unit in Energy.Units)
                            {
                                if (energy.Unit == unit || tu == unit) continue;
                                result.Add(energy.ToUnit(unit));
                            }
                        }
                    }
                    break;

                case QuantityType.Information:
                    Information information;
                    if (Information.TryParse(quantity, out information))
                    {
                        InformationUnit tu;
                        if (UnitParser.Default.TryParse(toUnit, out tu))
                        {
                            result.Add(information.ToUnit(tu));
                            foreach (var unit in Information.Units)
                            {
                                if (information.Unit == unit || tu == unit) continue;
                                result.Add(information.ToUnit(unit));
                            }
                        }
                    }
                    break;

                case QuantityType.Length:
                    Length length;
                    if (Length.TryParse(quantity, out length))
                    {
                        LengthUnit tu;
                        if (UnitParser.Default.TryParse(toUnit, out tu))
                        {
                            result.Add(length.ToUnit(tu));
                            foreach (var unit in Length.Units)
                            {
                                if (length.Unit == unit || tu == unit) continue;
                                result.Add(length.ToUnit(unit));
                            }
                        }
                    }
                    break;

                case QuantityType.Mass:
                    Mass mass;
                    if (Mass.TryParse(quantity, out mass))
                    {
                        MassUnit tu;
                        if (UnitParser.Default.TryParse<MassUnit>(toUnit, out tu))
                        {
                            result.Add(mass.ToUnit(tu));
                            foreach (var unit in Mass.Units)
                            {
                                if (mass.Unit == unit || tu == unit) continue;
                                result.Add(mass.ToUnit(unit));
                            }
                        }
                    }
                    break;

                case QuantityType.Power:
                    Power power;
                    if (Power.TryParse(quantity, out power))
                    {
                        PowerUnit tu;
                        if (UnitParser.Default.TryParse(toUnit, out tu))
                        {
                            result.Add(power.ToUnit(tu));
                            foreach (var unit in Power.Units)
                            {
                                if (power.Unit == unit || tu == unit) continue;
                                result.Add(power.ToUnit(unit));
                            }
                        }
                    }
                    break;

                case QuantityType.Pressure:
                    Pressure pressure;
                    if (Pressure.TryParse(quantity, out pressure))
                    {
                        PressureUnit tu;
                        if (UnitParser.Default.TryParse(toUnit, out tu))
                        {
                            result.Add(pressure.ToUnit(tu));
                            foreach (var unit in Pressure.Units)
                            {
                                if (pressure.Unit == unit || tu == unit) continue;
                                result.Add(pressure.ToUnit(unit));
                            }
                        }
                    }
                    break;

                case QuantityType.Speed:
                    Speed speed;
                    if (Speed.TryParse(quantity, out speed))
                    {
                        SpeedUnit tu;
                        if (UnitParser.Default.TryParse<SpeedUnit>(toUnit, out tu))
                        {
                            result.Add(speed.ToUnit(tu));
                            foreach (var unit in Speed.Units)
                            {
                                if (speed.Unit == unit || tu == unit) continue;
                                result.Add(speed.ToUnit(unit));
                            }
                        }
                    }
                    break;


                case QuantityType.Temperature:
                    Temperature temperature;
                    if (Temperature.TryParse(quantity, out temperature))
                    {
                        TemperatureUnit tu;
                        if (UnitParser.Default.TryParse(toUnit, out tu))
                        {
                            result.Add(temperature.ToUnit(tu));
                            foreach (var unit in Temperature.Units)
                            {
                                if (temperature.Unit == unit || tu == unit) continue;
                                result.Add(temperature.ToUnit(unit));
                            }
                        }
                    }
                    break;

                case QuantityType.Volume:
                    Volume volume;
                    if (Volume.TryParse(quantity, out volume))
                    {
                        VolumeUnit tu;
                        if (UnitParser.Default.TryParse<VolumeUnit>(toUnit, out tu))
                        {
                            result.Add(volume.ToUnit(tu));
                            foreach (var unit in Volume.Units)
                            {
                                if (volume.Unit == unit || tu == unit) continue;
                                result.Add(volume.ToUnit(unit));
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
            return result;
        }
    }
}
