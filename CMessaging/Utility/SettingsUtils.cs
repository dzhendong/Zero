using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;

namespace ILvYou.Zero.Utility
{
    public class SettingsUtils
    {
        #region Field
        private static IDictionary<string, string> settingsDic;
        public static string LocalIP;
        public static string ServerName;
        #endregion

        #region APIs
        static SettingsUtils()
        {
            SettingsUtils.settingsDic = new Dictionary<string, string>();
            SettingsUtils.LocalIP = "";
            SettingsUtils.ServerName = "";
            SettingsUtils.InitAppSettings();
            SettingsUtils.ServerName = SettingsUtils.GetLocalHostName();
            IPAddress[] hostAddresses = Dns.GetHostAddresses(SettingsUtils.ServerName);
            IPAddress[] array = hostAddresses;
            for (int i = 0; i < array.Length; i++)
            {
                IPAddress iPAddress = array[i];
                string text = iPAddress.ToString();
                if (text.Contains('.'))
                {
                    SettingsUtils.LocalIP = text;
                    break;
                }
            }
            if (SettingsUtils.LocalIP == "")
            {
                SettingsUtils.LocalIP = "127.0.0.0";
            }
        }

        private static void InitAppSettings()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            string[] allKeys = ConfigurationManager.AppSettings.AllKeys;
            string[] array = allKeys;
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                SettingsUtils.settingsDic[text] = ConfigurationManager.AppSettings[text];
            }
        }

        public static string GetCollectorName()
        {
            return (SettingsUtils.ServerName + "-Collector").ToLower();
        }

        public static string GetDispatcherName()
        {
            return (SettingsUtils.ServerName + "-Dispatcher").ToLower();
        }

        public static string GetLocalHostName()
        {
            return Dns.GetHostName();
        }

        public static string GetLocalIp()
        {
            return SettingsUtils.LocalIP;
        }

        public static IDictionary<string, string> GetAppSettings()
        {
            return SettingsUtils.settingsDic;
        }

        public static DateTime GetClearDate()
        {
            DateTime now = DateTime.Now;
            TimeSpan value = new TimeSpan(48, 0, 0);
            return now.Subtract(value);
        }

        public static int GetCurrentPartitionKey()
        {
            return (int)(DateTime.Now.DayOfWeek + 1);
        }
        #endregion
    }
}
