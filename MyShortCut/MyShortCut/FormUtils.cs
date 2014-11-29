using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using Microsoft.Win32;
using System.Diagnostics;

namespace MyShortCut
{
    public class FormUtils
    {
        private Form1 form1;
        public FormUtils(Form1 ff)
        {
            this.form1 = ff;
        }

        //获取当前运行实例
        public Process getRunningInstance()
        {
            Process currentProcess = Process.GetCurrentProcess();//获取当前进程
            //获取当前运行程序完全限定名
            string currentFileName = currentProcess.MainModule.FileName;
            //获取进程名为ProcessName的Process数组。
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
            //遍历有相同进程名称正在运行的进程
            foreach (Process process in processes)
            {
                if (process.MainModule.FileName == currentFileName)
                {
                    if (process.Id != currentProcess.Id)//根据进程ID排除当前进程
                        return process;//返回已运行的进程实例
                }
            }
            return null;
        }
        
        //初始化按钮
        public void initButtons()
        {
            //清空页面上显示的按钮
            for (int i = 0, j = form1.buttons.Count; i < j; i++)
            {
                form1.Controls.Remove((Button)form1.buttons[i]);
            }

            form1.buttons.Clear();
            form1.buttons = new ArrayList();

            for (int i = 0, j = form1.config.table.Count; i < j; i++)
            {
                int index = i;
                ApplicationGroup group = (ApplicationGroup)form1.config.table[index];
                System.Windows.Forms.Button bb = new System.Windows.Forms.Button();

                bb.Name = group.text;
                bb.Size = new System.Drawing.Size(292, 23);
                bb.TabIndex = 0;
                bb.Text = group.text;
                bb.UseVisualStyleBackColor = true;
                bb.Click += new System.EventHandler(form1.button1_Click);
                if (index > 0)
                {
                    bb.Dock = System.Windows.Forms.DockStyle.Bottom;
                }
                else
                {
                    bb.Dock = System.Windows.Forms.DockStyle.Top;
                    form1.currentGroupKey = bb.Text;
                    loadListViewGroup(form1.currentGroupKey);
                }

                form1.Controls.Add(bb);
                form1.buttons.Add(bb);
            }
        }

        //初始化数据
        public void loadConfig()
        {
            form1.config.clear();
            XmlDocument xml = new XmlDocument();
            xml.Load(System.AppDomain.CurrentDomain.BaseDirectory + @"\config.xml");
            foreach (XmlNode node in xml.ChildNodes)
            {
                if (node.Name == "config")
                {
                    foreach (XmlNode node1 in node.ChildNodes)
                    {
                        if (node1.Name == "list")
                        {
                            foreach (XmlNode node2 in node1.ChildNodes)
                            {
                                if (node2.Name == "row")
                                {
                                    ApplicationGroup group = new ApplicationGroup();
                                    group.text = node2.Attributes["text"].Value;
                                    foreach (XmlNode node3 in node2.ChildNodes)
                                    {
                                        if (node3.Name == "detail")
                                        {
                                            Console.WriteLine(node3.Attributes["value"].Value);
                                            ApplicationCell cell = new ApplicationCell();
                                            cell.text = node3.Attributes["text"].Value;
                                            cell.value = node3.Attributes["value"].Value;
                                            group.cells.Add(cell);
                                        }
                                    }
                                    form1.config.table.Add(group);
                                }
                            }
                        }
                        else if (node1.Name == "windows")
                        {
                            form1.config.width =  int.Parse(node1.Attributes["width"].Value);
                            form1.config.height = int.Parse(node1.Attributes["height"].Value);
                        }
                    }
                }
            }
        }

        //读取listview页中的快捷方式
        public void loadListViewGroup(String groupKey)
        {

            ApplicationGroup group = (ApplicationGroup)form1.config.getGroup(groupKey);
            form1.listView1.Clear();
            for (int ii = 0, jj = group.cells.Count; ii < jj; ii++)
            {
                ApplicationCell cell = (ApplicationCell)group.cells[ii];

                // listView1.Items.Add(new ListViewItem(cell.text, cell.value));
                form1.listView1.Items.Add(cell.text, cell.value);
            }
        }

        //显示窗口或者隐藏窗口
        public void displayOrHiddenWindow()
        {
            if (form1.WindowState == FormWindowState.Minimized)
            {
                form1.Show();
                form1.WindowState = FormWindowState.Normal;
            }
            else
            {
                form1.Hide();
                form1.WindowState = FormWindowState.Minimized;

            }
        }


        public void setAutoRun()
        {
            RunWhenStart(true, "MyShortCut", System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        }
        //设置自己启动
        public void RunWhenStart(bool Started, string name, string path)
        {
            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey Run = HKLM.CreateSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\");
            if (Started == true)
            {
                try
                {
                    Run.SetValue(name, path);
                    HKLM.Close();
                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.Message.ToString(), "MUS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    Run.DeleteValue(name);
                    HKLM.Close();
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
