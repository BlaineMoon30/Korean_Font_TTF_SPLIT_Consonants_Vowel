using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTF_To_BMP
{
    internal class Util
    {
        public Util()
        {

        }

        public string Util_Get_DateTime()
        {
            DateTime now = DateTime.Now;

            int year = now.Year;
            int month = now.Month;
            int day = now.Day;
            int hour = now.Hour;
            int minute = now.Minute;
            int second = now.Second;

            Console.WriteLine("Year: " + year);
            Console.WriteLine("Month: " + month);
            Console.WriteLine("Day: " + day);
            Console.WriteLine("Hour: " + hour);
            Console.WriteLine("Minute: " + minute);
            Console.WriteLine("Second: " + second);

            string combine_date_time = "_" + year + "_" + month + "_" + day + "_" + hour + "_" + minute + "_" + second;
            return combine_date_time;
        }
    }
}
