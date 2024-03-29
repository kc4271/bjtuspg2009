﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;

namespace Demo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class App : Application
    {
        // 创建全局变量
        public static User CurrentUser = new User();
        public static XmlOperator Profile;
        public static SPEECH_API SA;
        public App()
        {
            // 检查Profile目录
            DirectoryInfo di = new DirectoryInfo("Profile");
            if (!di.Exists)
                Directory.CreateDirectory("Profile");

            // 检查Profile文件
            FileInfo fi = new FileInfo("Profile/Profile.xml");
            if (!fi.Exists)
            {
                ProfileXMLGenerater();
            }

            GuestXMLGenerater();

            Profile = new XmlOperator();

            SA = new SPEECH_API();
            SA.Init("TRec.cfg",@"Resources\Rec\diclist");
        }

        ~App()
        {
            SA.Clear();
           // Application.Current.Shutdown();
        }

        public void GuestXMLGenerater()
        {
            XDocument inventoryDoc =
            new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("User",
                new XElement("Name", "Guest"),
                new XElement("Pet", "DOG"),
                new XElement("Gold", 0),
                new XElement("Food", 0),
                new XElement("Energy", 0),
                new XElement("Item1", 0),
                new XElement("Item2", 0)
                )
            );
            inventoryDoc.Save("Profile/Guest.xml");
        }

        public void ProfileXMLGenerater()
        {
            XDocument inventoryDoc =
            new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("Profile",
                new XElement("Name", "Guest")
                )
            );
            inventoryDoc.Save("Profile/Profile.xml");
        }

    }
}
