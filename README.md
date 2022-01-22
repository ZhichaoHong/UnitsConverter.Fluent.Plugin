# UnitsConverter.Fluent.Plugin
Fluent Search Plugin for units conversion.

To search without using tag, enter a phrase such as *5 cm in "* in fluent search field. This will convert 5 cm (source unit) to inches (target unit). Instead of using "in", you can use "to" instead. 
To search with tag (tag name is unitsconverter), you can simply enter the quantity with unit, it will list all converted units. For example: 5 meter to list all the converted units in length.

Currently supports the following units (UnitsNet)
   - QuantityType.Acceleration,

      Example to convert 5 meters/square seconds to knot/hour: 5 m/s^2 in knotperhour

   - QuantityType.Angle,

      Example to convert 180 degree to rad: 180 degree to rad

   - QuantityType.Area,

      Example to covert 5 sqft to acre: 5 squarefoot in acre

   - QuantityType.Duration,

      Examples to convert 10 hours to day: 10 hr in day

   - QuantityType.Energy,

      Example to convert 10 joules to calories: 10 joule in cal

   - QuantityType.Information,

      Example to conver 1 killo bytes to bits: 1 KB in b

   - QuantityType.Length,

      Example to convert 6 meters in inches: 6 m in " 

   - QuantityType.Mass,

      Example to convert 5 killograms in pounds: 5 kg to p

   - QuantityType.Power,

      Example to conver 7 watt to joule/hour: 7 watt in jouleperhour
      
   - QuantityType.Pressure,

      Example to convert 8 pascal to foot of elevation: 8 pascal in footofelevation

   - QuantityType.Speed,

      Example to convert 3 miles per hour to 5 km/h to ft/h

   - QuantityType.Temperature,

      Example to convert 30 Celsius to Fahrenheit : 5 c to f 

   - QuantityType.Volume


For more details about the supported quantities and their units definitions, please see https://github.com/angularsen/UnitsNet/tree/master/Common/UnitDefinitions.
