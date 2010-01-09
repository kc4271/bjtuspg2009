using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.OleDb;

using Demo.Global;

namespace Demo
{
    /// <summary>
    /// Chaining.xaml 的交互逻辑
    /// </summary>
    public partial class Chaining : Page
    {
        private Random ro = new Random();
        private int iScore;
        private int iPass;
        private int iWinScore;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadIdioms();
            iScore = 0;     // 当前得分
            iPass = 10;     // Pass次数
            iWinScore = 10;  // 赢得本局需要的得分
            txtScore.Text = "分数:" + iScore.ToString();
            txtPassLeft.Text = "剩余 " + iPass.ToString() + " 次机会";
        }

        private void LoadIdioms()
        {
            int iRandom;
            while (true)
            {
                iRandom = ro.Next(13000);
                if (iRandom > 0)
                    break;
            }
            DBControl db = new DBControl();
            try
            {
                db.CommandText = "SELECT ChengYu FROM ChengYu WHERE id = @id";
                db.AddParameter("id", iRandom);
                db.ExecuteReader();
                while (db.Reader.Read())
                {
                    txtQuestion.Text = db.Reader[0].ToString();
                    break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadIdioms Function Exception!");
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                db.CloseConntion();
            }
        }

        private void LoadIdioms(String sLastCharacter)
        {
            DBControl db = new DBControl();
            try
            {
                DataSet ds = new DataSet();
                OleDbDataAdapter a = new OleDbDataAdapter("SELECT ChengYu FROM ChengYu WHERE ChengYu LIKE '" + sLastCharacter + "%'", db.Connection);
                a.Fill(ds);
                int rs = ds.Tables[0].Rows.Count;

                if (rs > 0)
                {
                    txtQuestion.Text = ds.Tables[0].Rows[ro.Next(rs)].ItemArray[0].ToString();
                }
                else
                {
                    txtStatus.Text = "没有以\"" + sLastCharacter + "\"开头的成语,加一次Pass机会";
                    iPass++;
                    txtPassLeft.Text = "剩余 " + iPass.ToString() + " 次机会";
                    db.CloseConntion();
                    LoadIdioms();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadIdioms(string) Function Exception!");
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                db.CloseConntion();
            }
        }


        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            txtDebug.Text = "";

            if (txtAnswer.Text == "" || txtAnswer.Text.Substring(0, 1) != txtQuestion.Text.Substring(txtQuestion.Text.Length - 1, 1))
            {
                txtStatus.Text = "错误!";
                return;
            }

            DBControl db = new DBControl();
            try
            {
                db.CommandText = "SELECT ChengYu FROM ChengYu WHERE ChengYu = @ChengYu";
                db.AddParameter("id", txtAnswer.Text);
                db.ExecuteReader();
                if (db.Reader.Read())
                {
                    txtStatus.Text = "回答正确,加1分!";
                    // 分数操作
                    iScore++;
                    txtScore.Text = "分数:" + iScore.ToString();
                    if (iScore >= iWinScore)
                    {
                        MessageBox.Show("恭喜你顺利打开宝箱!");
                        this.NavigationService.Navigate(new Uri("Adventure.xaml", UriKind.Relative));
                    }
                    else
                    {
                        db.CloseConntion();
                        LoadIdioms(txtAnswer.Text.Substring(txtAnswer.Text.Length - 1, 1));
                    }
                }
                else
                {
                    txtStatus.Text = "这个词不是成语!";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Submit Function Exception!");
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                db.CloseConntion();
            }
        }

        private void btnPass_Click(object sender, RoutedEventArgs e)
        {
            if (iPass > 0)
            {
                iPass--;
                txtPassLeft.Text = "剩余 " + iPass.ToString() + " 次机会";
                txtStatus.Text = "PASS";
                LoadIdioms(txtQuestion.Text.Substring(txtQuestion.Text.Length - 1, 1));
            }
            else
            {
                MessageBox.Show("对不起,已经没有Pass机会了.");
                this.NavigationService.Navigate(new Uri("Adventure.xaml", UriKind.Relative));
            }
        }

        private void btnItem1_Click(object sender, RoutedEventArgs e)
        {
            iPass++;
            txtPassLeft.Text = "剩余 " + iPass.ToString() + " 次机会";
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Adventure.xaml", UriKind.Relative));
        }

        private void btnItem2_Click(object sender, RoutedEventArgs e)
        {
            iScore--;
            txtScore.Text = "分数:" + iScore.ToString();
            DBControl db = new DBControl();
            try
            {
                DataSet ds = new DataSet();
                OleDbDataAdapter a = new OleDbDataAdapter("SELECT ChengYu FROM ChengYu WHERE ChengYu LIKE '" +
                    txtQuestion.Text.Substring(txtQuestion.Text.Length - 1, 1) + "%'", db.Connection);
                a.Fill(ds);
                int rs = ds.Tables[0].Rows.Count;

                if (rs > 0)
                {
                    txtDebug.Text = ds.Tables[0].Rows[ro.Next(rs)].ItemArray[0].ToString();
                }
                else
                {
                    txtDebug.Text = "这个我也不会...";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Item2 Function Exception!");
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                db.CloseConntion();
            }
        }
    }
}
