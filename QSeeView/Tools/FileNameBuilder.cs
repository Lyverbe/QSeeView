using System;

namespace QSeeView.Tools
{
    public class FileNameBuilder
    {
        public static string Build(string pattern, DateTime timestamp, int channel)
        {
            var output = string.Empty;

            if (!string.IsNullOrEmpty(pattern))
            {
                var escapeChar = false;
                foreach (var patternChar in pattern)
                {
                    if (escapeChar)
                    {
                        switch (patternChar)
                        {
                            case 'd':
                                output += timestamp.Date.Day;
                                break;
                            case 'D':
                                output += timestamp.Date.Day.ToString("D2");
                                break;
                            case 'm':
                                output += timestamp.Date.Month;
                                break;
                            case 'M':
                                output += timestamp.Date.Month.ToString("D2");
                                break;
                            case 'y':
                                output += timestamp.Date.Year.ToString().Substring(2, 2);
                                break;
                            case 'Y':
                                output += timestamp.Date.Year;
                                break;
                            case 'h':
                                output += timestamp.TimeOfDay.Hours;
                                break;
                            case 'H':
                                output += timestamp.TimeOfDay.Hours.ToString("D2");
                                break;
                            case 'n':
                                output += timestamp.TimeOfDay.Minutes;
                                break;
                            case 'N':
                                output += timestamp.TimeOfDay.Minutes.ToString("D2");
                                break;
                            case 's':
                                output += timestamp.TimeOfDay.Seconds;
                                break;
                            case 'S':
                                output += timestamp.TimeOfDay.Seconds.ToString("D2");
                                break;
                            case 'c':
                                output += channel;
                                break;
                            case 'C':
                                output += channel.ToString("D2");
                                break;
                        }
                        escapeChar = false;
                    }
                    else
                    {
                        if (patternChar == '%')
                            escapeChar = true;
                        else
                            output += patternChar;
                    }
                }
            }

            return output;
        }
    }
}
