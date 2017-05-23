using System.Configuration;
using System.Globalization;
using TotalBase.Enums;

namespace TotalPortal.Configuration
{
    public static class SettingsManager
    {
        public static string BaseServiceUrl
        {
            get { return ConfigurationManager.AppSettings["BaseServiceUrl"]; }
        }

        public static int AutoCompleteMinLenght = 3;

        public static string DateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        public static string TimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
        public static string DateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;

        public static string NumberFormat = "{0:n0}";
        public static string YearMonthPattern = CultureInfo.CurrentCulture.DateTimeFormat.YearMonthPattern;
        public static int GridPopupHeight = 263;
        public static int GridPopupNoTabHeight = 330;

        public static int PopupHeight = 486;
        public static int PopupHeightSmall = 399;
        public static int PopupHeightWithTab = 518;
        public static int PopupHeightLarge = 518;
        public static int PopupHeightVoid = 269;

        public static int PopupWidth = 1068;
        public static int PopupWidthLarge = 1118;
        public static int PopupWidthSmall = 900;
        public static int PopupWidthMedium = 900;
        public static int PopupWidthVoid = 600;

        public static int PopupContentHeight = 360;
        public static int PopupContentHeightSmall = 281;
        public static int PopupContentHeightLarge = 397;

        public static string MonthDayPattern
        {
            get
            {
                string shortDatePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                while (shortDatePattern[0] != 'd' && shortDatePattern[0] != 'M')
                {
                    shortDatePattern = shortDatePattern.Substring(1);
                    if (shortDatePattern.Length == 0)
                        return CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                }
                while (shortDatePattern[shortDatePattern.Length - 1] != 'd' && shortDatePattern[shortDatePattern.Length - 1] != 'M')
                {
                    shortDatePattern = shortDatePattern.Substring(0, shortDatePattern.Length - 1);
                    if (shortDatePattern.Length == 0)
                        return CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                }
                return shortDatePattern;
            }
        }
    
    }

    public class MySettingsManager
    {
        public string BaseServiceUrl { get { return SettingsManager.BaseServiceUrl; } }

        public int AutoCompleteMinLenght { get { return SettingsManager.AutoCompleteMinLenght; } }
        public string DateFormat { get { return SettingsManager.DateFormat; } }
        public string TimeFormat { get { return SettingsManager.TimeFormat; } }
        public string DateTimeFormat { get { return SettingsManager.DateTimeFormat; } }
        public string NumberFormat { get { return SettingsManager.NumberFormat; } }
        public string YearMonthPattern { get { return SettingsManager.YearMonthPattern; } }
        public string MonthDayPattern { get { return SettingsManager.MonthDayPattern; } }
        public int GridPopupHeight { get { return SettingsManager.GridPopupHeight; } }
        public int GridPopupNoTabHeight { get { return SettingsManager.GridPopupNoTabHeight; } }
    }

}