/* =====================================================================================================
 * Code to parse Date string and time strings into DateTime class
 * =====================================================================================================
 * By: Zakie Mashiah
 * Copyright: You can use the code below, portions or all of it for any use. Credit the author will be
 * appreciated
 * =====================================================================================================
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherDataAnalyzer
{
    static class DateParser
    {
        public static DateTime DateStringToDateTime(string s)
        {
            int y, m, d;
            string ptr;
            int ly = s.IndexOf("/");

            ptr = s.Substring(0, ly);
            y = Convert.ToInt32(ptr);
            ptr = s.Substring(ly + 1); // Now start with the month
            int lm = ptr.IndexOf("/"); // and find the delimiter to day
            ptr = s.Substring(ly + 1, lm);
            m = Convert.ToInt32(ptr);
            ptr = s.Substring(ly + 1 + lm + 1);
            d = Convert.ToInt32(ptr);

            return new DateTime(y, m, d);
        }

        public static DateTime DateAndTimeStringToDateTime(string sDate, string sTime)
        {
            int y, m, d;
            int h, mm;
            string ptr;
            int ly = sDate.IndexOf("/");
            int lmm = sTime.IndexOf(":");

            // The date part
            ptr = sDate.Substring(0, ly);
            y = Convert.ToInt32(ptr);
            ptr = sDate.Substring(ly + 1); // Now start with the month
            int lm = ptr.IndexOf("/"); // and find the delimiter to day
            ptr = sDate.Substring(ly + 1, lm);
            m = Convert.ToInt32(ptr);
            ptr = sDate.Substring(ly + 1 + lm + 1);
            d = Convert.ToInt32(ptr);

            // The time part
            ptr = sTime.Substring(0, lmm);
            h = Convert.ToInt32(ptr);
            ptr = sTime.Substring(lmm + 1);
            mm = Convert.ToInt32(ptr);

            return new DateTime(y, m, d, h, mm, 0);

        }
    }
}
