/* =====================================================================================================
 * Code load data of Weather Station from flat CSV file.
 * =====================================================================================================
 * By: Zakie Mashiah
 * Copyright: You can use the code below, portions or all of it for any use. Credit the author will be
 * appreciated
 * =====================================================================================================
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherDataAnalyzer
{
    class WeatherStationReader : FlatFileDataReader
    {

        public WeatherStationReader(string FileName)
            : base(FileName)
        {

        }
        /*
         * This is how the file looks like:
         * ComputerDate,ComputerTime,Date,Time,year,month,day,hour,minute,second,voltage,charger,battery_status,temperature102,temperatureDHT22,temperatureBMP085,humidty,pressure
         * "2011/11/22","01:02","2011/08/31","19:04",2011,8,31,19,4,2,4.24,1,0,27.88,27.4,27.4,59.9,100224
         *
         */
        //string trailer = null;

        string[] currentLine = null;

        string[] fieldNames = new string[] 
            { 
                "ComputerDate", // 1
                "ComputerTime", // 2
#if false
                "DateString",   // 3
                "TimeString",   // 4
                "Year",     // 5
                "Month",    // 6
                "Day",  // 7
                "Hour", // 8
                "Minute",   // 9
                "Second",   // 10
#endif
                "Voltage",  // 11   3
                "Charger",  // 12   4
                "BatteryStatus",    // 13   5
                "Temperature102",   // 14   6
                "TemperatureDHT22", // 15   7
                "TemperatureBMP085",    // 16   8
                "Humidity", // 17   9
                "Pressure"  // 18   10
            };
        Type[] fieldTypes = new Type[] 
            { 
                typeof(string), // 1
                typeof(string), // 2
#if false
                typeof(string), // 3
                typeof(string), // 4
                typeof(int),    // 5
                typeof(int),    // 6
                typeof(int),    // 7
                typeof(int),    // 8
                typeof(int),    // 9
                typeof(int),    // 10
#endif
                typeof(float),  // 11   3
                typeof(int),    // 12   4
                typeof(int),    // 13   5
                typeof(float),  // 14   6
                typeof(float),  // 15   7
                typeof(float),  // 16   8
                typeof(float),  // 17   9
                typeof(long)    // 18   10
            };




        public override FlatFileDataReader.LineResult ReadLine(ref string line)
        {
            if (LineNumber == 1)
            {
                return LineResult.SKIP;
            }

            //simply split the line on ','
            currentLine = line.Split(new char[] { ',' });


            //strip enclosing double-quote characters
            //this is pretty crude as it doesn't account
            //for escaping of quotes, or wierd spacing
            for (int i = 0; i < currentLine.Length; i++)
            {
                string sv = currentLine[i];
                if (sv[0] == '"' && sv[sv.Length - 1] == '"')
                {
                    currentLine[i] = sv.Substring(1, sv.Length - 2);
                }
            }

            //force the row to update
            //currentLine[0] = "U";

            //check that the correct number of fields were found
            if (currentLine.Length != fieldNames.Length)
            {
                throw new ParseException(line, LineNumber, "Wrong Number of fields found");
            }


            return LineResult.READ;

        }

        public override object GetValue(int i)
        {
            return Convert.ChangeType(currentLine[i], fieldTypes[i]);
        }

        public override Type GetFieldType(int i)
        {
            return fieldTypes[i];
        }

        public override string GetName(int i)
        {
            return fieldNames[i];
        }

        public override int FieldCount
        {
            get
            {
                return fieldNames.Length;
            }
        }
    }
}
