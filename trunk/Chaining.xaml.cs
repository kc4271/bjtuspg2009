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
using System.Configuration;

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

        OleDbConnection conn = new OleDbConnection();   // 创建数据库连接对象

        /// <summary>
        /// 页面载入初始化, 初始化当前得分,pass次数,胜利分数,提示文字,数据库连接,随机产生一个成语
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            iScore = 0;     // 当前得分
            iPass = 100;     // Pass次数
            iWinScore = 10;  // 赢得本局需要的得分

            txtScore.Text = "分数:" + iScore.ToString();
            txtPassLeft.Text = "剩余 " + iPass.ToString() + " 次机会";

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
            conn.Open();

            LoadIdioms();
        }

        /// <summary>
        /// 随机产生一个成语
        /// </summary>
        private void LoadIdioms()
        {
            int iRandom;
            while (true)
            {
                iRandom = ro.Next(13000);   // 数据库记录数
                if (iRandom > 0)
                    break;
            }
            try
            {
                OleDbCommand cmd = new OleDbCommand("SELECT ChengYu FROM ChengYu WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("id", iRandom);
               
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtQuestion.Text = reader[0].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadIdioms Function Exception!");
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 随机产生以sLastCharacter开头的成语
        /// </summary>
        /// <param name="sLastCharacter">成语首字</param>
        private void LoadIdioms(String sLastCharacter)
        {
            try
            {
                DataSet ds = new DataSet();
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT ChengYu FROM ChengYu WHERE ChengYu LIKE '" + sLastCharacter + "%'", conn);
                adapter.Fill(ds);

                int rs = ds.Tables[0].Rows.Count;   // 以sLastCharacter开头的成语的个数
                if (rs > 0)
                {
                    txtQuestion.Text = ds.Tables[0].Rows[ro.Next(rs)].ItemArray[0].ToString();
                }
                else
                {
                    txtStatus.Text = "没有以\"" + sLastCharacter + "\"开头的成语,加一次Pass机会";
                    iPass++;
                    txtPassLeft.Text = "剩余 " + iPass.ToString() + " 次机会";
                    LoadIdioms();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadIdioms(string) Function Exception!");
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 确认后判定所输入单词是否合法,是否为所要求的成语
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            txtDebug.Text = ""; // 清空提示框

            if (txtAnswer.Text == "" || txtAnswer.Text.Substring(0, 1) != txtQuestion.Text.Substring(txtQuestion.Text.Length - 1, 1))
            {
                txtStatus.Text = "错误!";
                return;
            }
            try
            {
                OleDbCommand cmd = new OleDbCommand("SELECT ChengYu FROM ChengYu WHERE ChengYu = @ChengYu", conn);
                cmd.Parameters.AddWithValue("id", txtAnswer.Text);
                
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtStatus.Text = "回答正确,加1分!";
                    iScore++;
                    txtScore.Text = "分数:" + iScore.ToString();
                    if (iScore >= iWinScore)
                    {
                        MessageBox.Show("恭喜你顺利打开宝箱!");
                        this.NavigationService.Navigate(new Uri("Adventure.xaml", UriKind.Relative));
                    }
                    else
                    {
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
        }

        /// <summary>
        /// Pass,给出答案并继续游戏,若无答案,重新生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 道具1,增加一次pass机会
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem1_Click(object sender, RoutedEventArgs e)
        {
            iPass++;
            txtPassLeft.Text = "剩余 " + iPass.ToString() + " 次机会";
            txtStatus.Text = "增加一次Pass机会";
        }

        /// <summary>
        /// 道具2,提示答案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem2_Click(object sender, RoutedEventArgs e)
        {
            iScore--;
            txtScore.Text = "分数:" + iScore.ToString();

            try
            {
                DataSet ds = new DataSet();
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT ChengYu FROM ChengYu WHERE ChengYu LIKE '" +
                    txtQuestion.Text.Substring(txtQuestion.Text.Length - 1, 1) + "%'", conn);
                adapter.Fill(ds);
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
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Adventure.xaml", UriKind.Relative));
        }

        /// <summary>
        /// 卸载页面,关闭数据库连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            conn.Close();   // 关闭数据库连接
        }
    }
}
