using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;

namespace MyShortCut
{
    public partial class Form1 : Form
    {
        public ArrayList buttons = new ArrayList();
        public Config config = new Config();
        public String currentGroupKey;
        public int Hotkey1;
        public FormUtils formUtils;
 
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            //读取配置
            formUtils = new FormUtils(this);
            if (formUtils.getRunningInstance() != null)
            {
                MessageBox.Show("已经启动一个实例了");
                
            }

            //设置自启动
            formUtils.setAutoRun();
            //读取配置
            formUtils.loadConfig();
            //初始化按钮
            formUtils.initButtons();

            //快捷键
            Hotkey hotkey = new Hotkey(this.Handle);
            Hotkey1 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.Q, Hotkey.KeyFlags.MOD_ALT);
            hotkey.OnHotkey += new HotkeyEventHandler(OnHotkey);

            //窗口大小
            this.Width = config.width;
            this.Height = config.height;

            //改变位置
            int x = System.Windows.Forms.SystemInformation.VirtualScreen.Width - this.Width - this.Width/2;
            int y = (System.Windows.Forms.SystemInformation.VirtualScreen.Height - this.Height)/3;
            this.Location = new Point(x, y);
 
        }

        public void OnHotkey(int HotkeyID)
        {
            if (HotkeyID == Hotkey1)
            {
                formUtils.displayOrHiddenWindow();
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button bb = (System.Windows.Forms.Button)sender;
            //notifyIcon1.BalloonTipText = "哈佛有一个著名的理论：人的差别在于业余时间，而一个人的命运决定于晚上8点到10点之间。每晚抽出2个小时的时间用来阅读、进修、思考或参加有意的演讲、讨论，你会发现，你的人生正在发生改变，坚持数年之后，成功会向你招手。 ";
            //notifyIcon1.ShowBalloonTip(5000);
            for (int i = 0, j = buttons.Count; (i < j); i++)
            {
                int index = j - i - 1;
                System.Windows.Forms.Button row = (System.Windows.Forms.Button)buttons[index];
                //查找按钮
                if (row.Text.Equals(bb.Text))
                {
                    row.Dock = System.Windows.Forms.DockStyle.Top;
                    currentGroupKey = bb.Text;
                    formUtils.loadListViewGroup(currentGroupKey);
                }
                else
                {
                    row.Dock = System.Windows.Forms.DockStyle.Bottom;
                }
            }
 
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            System.Windows.Forms.Button bb = (System.Windows.Forms.Button)buttons[0];
            listView1.Height = this.Height - buttons.Count * 23 - 32;
        }


        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            System.Windows.Forms.ListView lv = (System.Windows.Forms.ListView)sender;
            ApplicationCell cell = config.getGroup(currentGroupKey).getCell(lv.SelectedItems[0].Text);
            Console.WriteLine(cell.text + ":" + cell.value);
            //System.Diagnostics.Process.Start("explorer", cell.value);
            System.Diagnostics.Process LandFileDivisison = new System.Diagnostics.Process();
            LandFileDivisison.StartInfo.FileName = cell.value;
           try
            {
                LandFileDivisison.Start();
            }
           catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message+",fileName:"+cell.value);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            formUtils.displayOrHiddenWindow();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void reloadConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formUtils.loadConfig();
            formUtils.initButtons();
            currentGroupKey = "";

            MessageBox.Show("配置文件重新载入成功");
        }

        private void exitMSCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();  //释放内存，比第一个好。
            Application.Exit();
        }

    }
}
