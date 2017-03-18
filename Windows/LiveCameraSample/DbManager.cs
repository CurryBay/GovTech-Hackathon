using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveCameraSample
{
    public class DbManager
    {
        private static string address = @"C:\Users\Caleb\Documents\School\GOVTECH\Act_Log.json";
        private static Dictionary<DateTime, string> act_log = JsonConvert.DeserializeObject<Dictionary<DateTime, string>>(File.ReadAllText(address));

        private static string slp_address = @"C:\Users\Caleb\Documents\School\GOVTECH\Slp_Schedule.json";
        private static Dictionary<DateTime, string> sleep_log = JsonConvert.DeserializeObject<Dictionary<DateTime, string>>(File.ReadAllText(slp_address));
        //private static Dictionary<DateTime, string> sleep_log = new Dictionary<DateTime, string> ();

        public static void updateDb(string category)
        {
            DateTime localDate = DateTime.Now;
            act_log.Add(localDate, category);
            string json = JsonConvert.SerializeObject(act_log, Formatting.Indented);
            if (category.Equals("Eating"))
            {
                addSleeping(category);
            }
            //inputting dictionary into file
            File.WriteAllText(address, JsonConvert.SerializeObject(act_log));
            //reading from file

        }
        public static void addSleeping(string category)
        {
            DateTime localDate = DateTime.Now;
            sleep_log.Add(localDate, category);
            string json = JsonConvert.SerializeObject(sleep_log, Formatting.Indented);
            File.WriteAllText(slp_address, JsonConvert.SerializeObject(sleep_log));
        }

        public static string Categorize(string message)
        {

            if (message.Contains("floor") || message.Contains("fire") || message.Contains("ground") || message.Contains("ground"))
            {

                return "Emergency";
            }

            if (message.Contains("man") || message.Contains("woman") || message.Contains("boy") || message.Contains("girl") || message.Contains("person"))
            {

                if (message.Contains("food") || message.Contains("eat") || message.Contains("plate") || message.Contains("drink") || message.Contains("kitchen"))
                {
                    return "Eating";
                }
                else if (message.Contains("sleep") || message.Contains("bed") || message.Contains("lay") || message.Contains("lie"))
                {
                    return "Sleeping";
                }
                else if (message.Contains("couch") || message.Contains("sofa") || message.Contains("sit") || message.Contains("television"))
                {
                    return "Leisure";
                }
                else
                {
                    return "Leisure";
                }
            }
            else
            {
                return "out";
            }
        }

    }
}
