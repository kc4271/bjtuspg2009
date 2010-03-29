using System;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace Demo
{
    public partial class Chaining : Page
    {
        private int iScore = 10;
        private int iPass = 5;
        private string status = "请说话:";
        private string info = "您需要完成成语接龙游戏\n来打开宝箱得到金币";
        private IdiomsDB DB;

        private bool Running = true;
        public delegate void TextBoxUpdaterDelegate(TextBox tb, string s);
        public delegate void RectangleUpdateDelegate(int mode);
        Thread oThread;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           
            DB = new IdiomsDB();
            txtLog.Text += "宝箱:\t" + DB.GetIdiom();
            txtStatus.Text = status;
            refresh(true);
            Running = true;
            App.SA.SetDic(DB.GetLastpy());
            App.SA.StartRec();
            App.SA.SwitchOn();
            oThread = new Thread(new ThreadStart(RUN));
            oThread.Start();
            runMode.Fill = Brushes.Red;
            
        }

        private void RUN()
        {
            while (Running)
            {
                this.runMode.Dispatcher.BeginInvoke
                    (new RectangleUpdateDelegate(RectangleUpdater), App.SA.GetStatus());
                if (App.SA.HasNewResult())
                {
                    if (this.txtAnswer != null)
                    {
                        this.txtAnswer.Dispatcher.BeginInvoke
                            (new TextBoxUpdaterDelegate(TextBoxUpdater), 
                            this.txtAnswer, App.SA.GetResult());
                    }
                    App.SA.Reply();
                }
                Thread.Sleep(30);
            }
        }

        public void TextBoxUpdater(TextBox tb, string s)
        {
            s = Regex.Replace(s, @"[0-9,\.,\:, ,D,\$,X]", "");
            if(s.Length != 0)
                txtAnswer.Text = s;
        }

        public void RectangleUpdater(int mode)
        {
            switch (mode)
            {
                case 0: runMode.Fill = Brushes.Red; break;
                case 1: runMode.Fill = Brushes.Orange; break;
                case 2: runMode.Fill = Brushes.Yellow; break;
                case 3: runMode.Fill = Brushes.Green; break;
                case 4: runMode.Fill = Brushes.Magenta; break;
            }
        }

        private void refresh(bool update)
        {
            if (update)
            {
                String A = App.SA.GetCurrDic();
                String B = DB.GetLastpy();
                if (A != B)
                {
                    App.SA.EndRec();
                    App.SA.SetDic(DB.GetLastpy());
                }
            }
            txtLog.ScrollToEnd();
            if (iScore == 0)
            {
                MessageBox.Show("成功打开宝箱,恭喜您!");
                App.CurrentUser.gold += 500;
                this.NavigationService.Navigate(new Uri("Adventure.xaml", UriKind.Relative));
            }
            info = "还需要答对" + iScore + "个成语完成任务\n还有" + iPass + "次放弃机会\n剩余" + App.CurrentUser.item1.ToString() + "个提示道具.";
            txtScore.Text = info;
            txtAnswer.Clear();
            txtAnswer.Focus();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (DB.CheckAnswer(txtAnswer.Text))
            {
                iScore--;
                txtStatus.Text = "回答正确!";
                txtLog.Text += "\n" + App.CurrentUser.pet + ":\t" + txtAnswer.Text;
                txtLog.Text += "\n宝箱:\t" + DB.GetIdiom(DB.GetLastpy());
                refresh(true);
            }
            else
            {
                txtStatus.Text = "错误!";
            }
        }

        private void btnPass_Click(object sender, RoutedEventArgs e)
        {
            if (iPass == 0)
            {
                MessageBox.Show("没有放弃机会了~");
            }
            else
            {
                iPass--;
                txtLog.Text += "\n宝箱:\t" + DB.GetIdiom();
                txtLog.ScrollToEnd();
                refresh(true);
            }
        }

        private void btnShowAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser.item1 > 0)
            {
                App.CurrentUser.item1--;
                MessageBox.Show(DB.ShowAnswer(DB.GetLastpy()));
            }
            else
            {
                MessageBox.Show("对不起,您已经没有提示道具了,请到商店里购买.");
            }
            refresh(false);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            DB.CloseDB();

            Running = false;
            App.SA.SwitchOff();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("您确定要放弃这个宝箱吗?\n现在放弃可能得不到奖励喔",
                "确认放弃", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.NavigationService.Navigate(new Uri("Adventure.xaml", UriKind.Relative));
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            txtAnswer.Text = "";
        }
    }
}
