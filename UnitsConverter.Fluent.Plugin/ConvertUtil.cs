using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using UnitsNet.Units;

namespace UnitsConverter.Fluent.Plugin
{
    internal class ConvertUtil
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

        public static List<IQuantity> ConvertWithoutQuantity(string text)
        {
            // The text is in the form of "5 lb in kg" or "5 lbs to kg"
            string[] parts = text.Split(new string[] { "in", "to" }, StringSplitOptions.RemoveEmptyEntries);
            string from = parts[0].Trim();
            string to = parts.Length > 1 ? parts[1].Trim() : null;
            
            if (Acceleration.TryParse(from, out Acceleration accel))
            {
                return Convert(from, QuantityType.Acceleration, to);
            }

            if (Angle.TryParse(from, out Angle angle))
            {
                return Convert(from, QuantityType.Angle, to);
            }

            if (Area.TryParse(from, out Area area))
            {
                return Convert(from, QuantityType.Area, to);
            }

            if (Duration.TryParse(from, out Duration duration))
            {
                return Convert(from, QuantityType.Duration, to);
            }

            if (Energy.TryParse(from, out Energy energy))
            {
                return Convert(from, QuantityType.Energy, to);
            }

            if (Information.TryParse(from, out Information information))
            {
                return Convert(from, QuantityType.Information, to);
            }

            if (Length.TryParse(from, out Length length))
            {
                return Convert(from, QuantityType.Length, to);
            }

            if (Mass.TryParse(from, out Mass mass))
            {
                return Convert(from, QuantityType.Mass, to);
            }

            if (Power.TryParse(from, out Power power))
            {
                return Convert(from, QuantityType.Power, to); 
            }

            if (Pressure.TryParse(from, out Pressure pressure))
            {
                return Convert(from, QuantityType.Pressure, to);
            }

            if (Speed.TryParse(from, out Speed speed))
            {
                return Convert(from, QuantityType.Speed, to);
            }

            if (Temperature.TryParse(from, out Temperature temperature))
            {
                return Convert(from, QuantityType.Temperature, to);
            }

            if (Volume.TryParse(from.ToString(), out Volume volume))
            {
                return Convert(from, QuantityType.Volume, to);
            }

            return new List<IQuantity>();

        }

        public static List<IQuantity> Convert(string quantity, QuantityType quantityType, string toUnit = null)
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
                        }
                        else
                        {
                            foreach (var unit in Acceleration.Units)
                            {
                                if (accel.Unit == unit) continue;
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
                        }
                        else
                        {
                            foreach (var unit in Angle.Units)
                            {
                                if (angle.Unit == unit) continue;
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
                        }
                        else
                        {
                            foreach (var unit in Area.Units)
                            {
                                if (area.Unit == unit) continue;
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
                        }
                        else
                        {
                            foreach (var unit in Duration.Units)
                            {
                                if (duration.Unit == unit) continue;
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

                        }
                        else
                        {
                            foreach (var unit in Energy.Units)
                            {
                                if (energy.Unit == unit) continue;
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
                        }
                        else
                        {
                            foreach (var unit in Information.Units)
                            {
                                if (information.Unit == unit) continue;
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
                        }
                        else
                        {
                            foreach (var unit in Length.Units)
                            {
                                if (length.Unit == unit) continue;
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
                        }
                        else
                        {
                            foreach (var unit in Mass.Units)
                            {
                                if (mass.Unit == unit) continue;
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
                        }
                        else
                        {
                            foreach (var unit in Power.Units)
                            {
                                if (power.Unit == unit) continue;
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
                        }
                        else
                        {
                            foreach (var unit in Pressure.Units)
                            {
                                if (pressure.Unit == unit) continue;
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
                        }
                        else
                        {
                            foreach (var unit in Speed.Units)
                            {
                                if (speed.Unit == unit) continue;
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
                        }
                        else
                        {
                            foreach (var unit in Temperature.Units)
                            {
                                if (temperature.Unit == unit) continue;
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
                        }
                        else
                        {
                            foreach (var unit in Volume.Units)
                            {
                                if (volume.Unit == unit) continue;
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
