using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Demo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            FileInfo fi = new FileInfo("userinfo.xml");
            if (!fi.Exists)
                ConfigClass.UserInfoGenerater();
        }

        public static class ConfigClass
        {
            public static string strFileName;
            public static string configName;
            public static string configValue;

            public static void ReadConfig(out string username, out string money)
            {
                username = "";
                money = "";
                XmlTextReader xmlTextReader = new XmlTextReader(@"userinfo.xml");
                xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
                while (xmlTextReader.Read())
                {
                    if (xmlTextReader.Name == "CUserInfo")
                    {
                        xmlTextReader.Read();
                        username = xmlTextReader.ReadElementContentAsString();
                        //xmlTextReader.Read();
                        money = xmlTextReader.ReadElementContentAsString();
                        break;
                    }
                }
                xmlTextReader.Close();
            }

            public static void SaveConfig(string configKey, string configValue)
            {
                XmlDocument doc = new XmlDocument();
                string strFileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "userinfo.xml";
                doc.Load(strFileName);
                XmlNode rootnode = doc.SelectSingleNode("CUserInfo");
                if (configValue.Length == 0)
                    configValue = "username";       
                rootnode.FirstChild.FirstChild.Value = configValue;
                doc.Save(strFileName);
            }

            public static void UserInfoGenerater()
            {
                CUserInfo userInfo = new CUserInfo();
                userInfo.sUserName = "Username";
                userInfo.nMoney = 888;
                XmlSerializer serializer = new XmlSerializer(typeof(CUserInfo));
                using (StreamWriter streamWriter = File.CreateText("userinfo.xml"))
                {
                    serializer.Serialize(streamWriter, userInfo);
                }
            }
        }

        [Serializable()]
        public class CUserInfo
        {
            // String field and property
            private string username;
            public string sUserName
            {
                get { return username; }
                set { username = value; }
            }

            // Bool field and property
            private int Money;
            public int nMoney
            {
                get { return Money; }
                set { Money = value; }
            }
        }
    }
}
