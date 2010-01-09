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

        Random ro = new Random();
        int iScore;
        int iPass;
        int iWinScore;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadIdioms();
            iScore = 0;     // 当前得分
            iPass = 20;     // Pass次数
            iWinScore = 5;  // 赢得本局需要的得分
            txtScore.Text = "分数:" + iScore.ToString();
            txtPassLeft.Text = "剩余 " + iPass.ToString() + " 次机会";
        }

        private void LoadIdioms()
        {
            txtStatus.Text = "";
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
                db.CommandText = "SELECT ChengYu FROM ChengYu WHERE ChengYu LIKE '" + sLastCharacter + "%'";
                db.ExecuteReader();
                if (db.Reader.Read())
                {
                    txtQuestion.Text = db.Reader[0].ToString();
                }
                else
                {
                    MessageBox.Show("找不到以 " + sLastCharacter + " 开头的成语,你得了1分");
                    // 分数操作
                    iScore++;
                    txtScore.Text = "SCORE:" + iScore.ToString();
                    if (iScore >= iWinScore)
                    {
                        MessageBox.Show("Cool! You're good at it!");
                        this.NavigationService.Navigate(new Uri("Adventure.xaml", UriKind.Relative));
                    }
                    else
                    {
                        LoadIdioms();
                    }
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
            if (txtAnswer.Text == "" || txtAnswer.Text.Substring(0, 1) != txtQuestion.Text.Substring(txtQuestion.Text.Length - 1, 1))
            {
                txtStatus.Text = "Wrong!";
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
                    txtStatus.Text = "Yes";
                    // 分数操作
                    iScore++;
                    txtScore.Text = "SCORE:" + iScore.ToString();
                    if (iScore >= iWinScore)
                    {
                        MessageBox.Show("Cool! You're good at it!");
                        this.NavigationService.Navigate(new Uri("Adventure.xaml", UriKind.Relative));
                    }
                    else
                    {
                        LoadIdioms(txtAnswer.Text.Substring(txtAnswer.Text.Length - 1, 1));
                    }
                }
                else
                {
                    txtStatus.Text = "Not Found!";
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
                txtPassLeft.Text = iPass.ToString() + " PASS LEFT";
                txtStatus.Text = "";
                LoadIdioms(txtQuestion.Text.Substring(txtQuestion.Text.Length - 1, 1));
            }
            else
            {
                MessageBox.Show("Sorry, you LOSE!");
                this.NavigationService.Navigate(new Uri("Adventure.xaml", UriKind.Relative));
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Adventure.xaml", UriKind.Relative));
        }

        // Debug Fuction
        private void txtDebug_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DBControl db = new DBControl();
            try
            {
                db.CommandText = "SELECT ChengYu FROM ChengYu WHERE ChengYu LIKE '" +
                    txtQuestion.Text.Substring(txtQuestion.Text.Length - 1, 1) + "%'";
                db.ExecuteReader();
                if (db.Reader.Read())
                {
                    txtDebug.Text = db.Reader[0].ToString();
                }
                else
                {
                    txtDebug.Text = "找不到了";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Debug Function Exception!");
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                db.CloseConntion();
            }
        }
    }
}
