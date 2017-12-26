using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HistoricalWeatherData
{
    class Program
    {
        static List<String> headers = new List<string>()
            {
                    "time",
                    "DayOfYear",
                    "summary",
                    "icon",
                    "sunriseTime",
                    "sunsetTime",
                    "moonPhase",
                    "precipType",
                    "temperatureMin",
                    "temperatureMinTime",
                    "temperatureMax",
                    "temperatureMaxTime",
                    "apparentTemperatureMin",
                    "apparentTemperatureMinTime",
                    "apparentTemperatureMax",
                    "apparentTemperatureMaxTime",
                    "dewPoint",
                    "humidity",
                    "windSpeed",
                    "windBearing",
                    "visibility",
                    "cloudCover",
                    "pressure",
            };

        static void Main(string[] args)
        {
            double Lat = 39.758929;
            double Lon = 22.2231366;
            Console.WriteLine(String.Join(";", headers.ToArray()));
            DateTime StartDate = new DateTime(2017, 8, 7);
            for (int i = 1; i < 180; i++)
            {
                Int32 UnixTimestamp = (Int32)(StartDate.AddDays(-i).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                System.Threading.Thread.Sleep(2000);
                GetData(UnixTimestamp, Lat, Lon);
            }

        }

        public static DateTime ConvertTime(double UnixTimeStamp)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(UnixTimeStamp));
        }
        public static async void GetData(int UnixTimeStamp, double Lat, double Lon)
        {
            HttpClient client = new HttpClient();
            var data = await client.GetStringAsync(String.Format(@"https://api.darksky.net/forecast/7f22bbae07cf1d105594f18c319f6b3a/{0},{1},{2}?exclude=currently,hourly,minutely,alerts,flags&units=si",
                Lat,
                Lon,
                UnixTimeStamp));
            var json = JsonConvert.DeserializeObject<dynamic>(data);
            DateTime time = ConvertTime((double)json.daily.data[0].time);
            DateTime sunriseTime = ConvertTime((double)json.daily.data[0].sunriseTime);
            DateTime sunsetTime = ConvertTime((double)json.daily.data[0].sunsetTime);
            Console.WriteLine(String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22}",
                            time.ToShortDateString(),
                            time.DayOfYear,
                            json.daily.data[0].summary,
                            json.daily.data[0].icon,
                            sunriseTime.ToShortTimeString(),
                            sunsetTime.ToShortTimeString(),
                            json.daily.data[0].moonPhase,
                            json.daily.data[0].precipType,
                            json.daily.data[0].temperatureMin,
                            json.daily.data[0].temperatureMinTime,
                            json.daily.data[0].temperatureMax,
                            json.daily.data[0].temperatureMaxTime,
                            json.daily.data[0].apparentTemperatureMin,
                            json.daily.data[0].apparentTemperatureMinTime,
                            json.daily.data[0].apparentTemperatureMax,
                            json.daily.data[0].apparentTemperatureMaxTime,
                            json.daily.data[0].dewPoint,
                            json.daily.data[0].humidity,
                            json.daily.data[0].windSpeed,
                            json.daily.data[0].windBearing,
                            json.daily.data[0].visibility,
                            json.daily.data[0].cloudCover,
                            json.daily.data[0].pressure));


        }
    }

}
