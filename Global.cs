using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace Demo
{
    public class User
    {
        public string name;
        public string pet;
        public int gold;
        public int item1;
        public int item2;
    }

    public class XmlOperator
    {
        public string xmlPath = "Profile/Guest.xml";
        XmlDocument myDc = new XmlDocument();

        public XmlOperator()
        {
            try
            {
                myDc.Load(xmlPath);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void XmlReload(string Path)
        {
            xmlPath = Path;
            myDc = new XmlDocument();
            try
            {
                myDc.Load(xmlPath);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void CreateNewXml()
        {
            XDocument inventoryDoc =
            new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("User",
                new XElement("Name", App.CurrentUser.name),
                new XElement("Gold", App.CurrentUser.gold.ToString()),
                new XElement("Pet", App.CurrentUser.pet.ToString()),
                new XElement("Item1", App.CurrentUser.item1.ToString()),
                new XElement("Item2", App.CurrentUser.item2.ToString())
                )
            );
            inventoryDoc.Save("Profile/" + App.CurrentUser.name + ".xml");
        }

        public void GetXmlReader()
        {
            App.CurrentUser.name = myDc.SelectSingleNode("User").SelectSingleNode("Name").InnerText;
            App.CurrentUser.pet = myDc.SelectSingleNode("User").SelectSingleNode("Pet").InnerText;
            App.CurrentUser.gold = int.Parse(myDc.SelectSingleNode("User").SelectSingleNode("Gold").InnerText);
            App.CurrentUser.item1 = int.Parse(myDc.SelectSingleNode("User").SelectSingleNode("Item1").InnerText);
            App.CurrentUser.item2 = int.Parse(myDc.SelectSingleNode("User").SelectSingleNode("Item2").InnerText);
        }

        public void SetXmlFile()
        {
            XmlReload("Profile/" + App.CurrentUser.name + ".xml");
            myDc.SelectSingleNode("User").SelectSingleNode("Name").InnerText = App.CurrentUser.name;
            myDc.SelectSingleNode("User").SelectSingleNode("Pet").InnerText = App.CurrentUser.pet;
            myDc.SelectSingleNode("User").SelectSingleNode("Gold").InnerText = App.CurrentUser.gold.ToString();
            myDc.SelectSingleNode("User").SelectSingleNode("Item1").InnerText = App.CurrentUser.item1.ToString();
            myDc.SelectSingleNode("User").SelectSingleNode("Item2").InnerText = App.CurrentUser.item2.ToString();
            myDc.Save(xmlPath);
        }
    }
}
