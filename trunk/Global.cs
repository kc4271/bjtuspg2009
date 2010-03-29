using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Data.OleDb;
using System.Data;
using System.Collections;

namespace Demo
{
    public class IdiomsDB
    {
        private OleDbConnection conn;
        private string chengyu;
        private string firstpy;
        private string lastpy;
        private Dictionary<string, string> dic = new Dictionary<string, string>();

        public string GetLastpy()
        {
            return this.lastpy;
        }

        public IdiomsDB()
        {
            conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\\Resources\\DB\\db.mdb;");
            conn.Open();
            this.GetIdiom();
        }

        public string GetIdiom()
        {
            int iRandom = new Random().Next(1, 3750);   // total

            OleDbCommand cmd = new OleDbCommand("SELECT * FROM ChengYu WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", iRandom);
            OleDbDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    this.chengyu = reader["ChengYu"].ToString();
                    this.firstpy = reader["FirstPy"].ToString();
                    this.lastpy = reader["Lastpy"].ToString();
                }
                reader.Close();
            }
            else
            {
                return "ERROR in GetIdiom";
            }
            return this.chengyu;
        }

        public string GetIdiom(string _lastpy)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM ChengYu WHERE FirstPy = '" + _lastpy + "'", conn);
            OleDbDataReader reader = cmd.ExecuteReader();

            List<string> cylist = new List<string>();
            List<string> lplist = new List<string>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    cylist.Add(reader["ChengYu"].ToString());
                    lplist.Add(reader["LastPy"].ToString());
                }
                reader.Close();
            }
            else
            {
                MessageBox.Show("没有以" + _lastpy + "开头的成语!");
                return GetIdiom();
            }
            int iRandom = new Random().Next(0, cylist.Count > 1 ? cylist.Count - 1 : 0);
            this.chengyu = cylist[iRandom];
            this.lastpy = lplist[iRandom];
            return this.chengyu;
        }

        public String ShowAnswer(string _lastpy)
        {
            string ret = "";
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM ChengYu WHERE FirstPy = '" + _lastpy + "'", conn);
            OleDbDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                ret =  reader["ChengYu"].ToString();
                reader.Close();
            }
            else
            {
                ret =  "No Answer";
            }
            return ret;
        }

        public bool CheckAnswer(string _str)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM ChengYu WHERE ChengYu = @ChengYu AND FirstPy = @lastpy", conn);
            cmd.Parameters.AddWithValue("ChengYu", _str);
            cmd.Parameters.AddWithValue("lastpy", this.lastpy);
            OleDbDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                this.lastpy = reader["LastPy"].ToString();  // 更新this.lastpy以便电脑据此给出下一条成语
                reader.Close();
                return true;
            }
            else
            {
                reader.Close();
                return false;
            }
        }

        public void CloseDB()
        {
            conn.Close();
        }
    }

    public class User
    {
        public string name;
        public string pet;
        public int gold;
        public int food;
        public int energy;
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
                new XElement("Pet", App.CurrentUser.pet.ToString()),
                new XElement("Gold", App.CurrentUser.gold.ToString()),
                new XElement("Food", App.CurrentUser.food.ToString()),
                new XElement("Energy", App.CurrentUser.energy.ToString()),
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
            App.CurrentUser.food = int.Parse(myDc.SelectSingleNode("User").SelectSingleNode("Food").InnerText);
            App.CurrentUser.energy = int.Parse(myDc.SelectSingleNode("User").SelectSingleNode("Energy").InnerText);
            App.CurrentUser.item1 = int.Parse(myDc.SelectSingleNode("User").SelectSingleNode("Item1").InnerText);
            App.CurrentUser.item2 = int.Parse(myDc.SelectSingleNode("User").SelectSingleNode("Item2").InnerText);
        }

        public void SetXmlFile()
        {
            XmlReload("Profile/" + App.CurrentUser.name + ".xml");
            myDc.SelectSingleNode("User").SelectSingleNode("Name").InnerText = App.CurrentUser.name;
            myDc.SelectSingleNode("User").SelectSingleNode("Pet").InnerText = App.CurrentUser.pet;
            myDc.SelectSingleNode("User").SelectSingleNode("Gold").InnerText = App.CurrentUser.gold.ToString();
            myDc.SelectSingleNode("User").SelectSingleNode("Food").InnerText = App.CurrentUser.food.ToString();
            myDc.SelectSingleNode("User").SelectSingleNode("Energy").InnerText = App.CurrentUser.energy.ToString();
            myDc.SelectSingleNode("User").SelectSingleNode("Item1").InnerText = App.CurrentUser.item1.ToString();
            myDc.SelectSingleNode("User").SelectSingleNode("Item2").InnerText = App.CurrentUser.item2.ToString();
            myDc.Save(xmlPath);
        }
    }
}
