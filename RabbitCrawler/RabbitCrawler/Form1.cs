using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RabbitCrawler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }
        private static Content content = new Content();

        private static List<SpecialList> list = new List<SpecialList>();
        private void button2_Click(object sender, EventArgs e)
        {
            var path = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("请先选择文件保存路径", "提示");
                return;
            }
            var index = comboBox1.SelectedIndex;
            if (index == 0)
            {
                MessageBox.Show("请选择分类", "提示");
            }
            Handler handle = new Handler();
            handle.OnStart += (s, ev) =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    richTextBox1.Text += "开始下载文件，文件地址：" + ev.Uri + "\r\n";
                }), s);
            };
            handle.OnError += (s, ev) =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    richTextBox1.Text += $"{ev.Exception.ToString()}【下载出错】，文件：{ev.Uri}未能下载成功，\r\n";
                }), s);

            };
            handle.OnCompleted += (s, ev) =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    richTextBox1.Text += $"{ev.Uri}【下载完成】\r\n";
                }), s);

            };
            var downLoadName = comboBox2.Text;
            if (string.IsNullOrEmpty(downLoadName))
            {
                MessageBox.Show("请选择要下载的故事","提示");
                return;
            }
            else
            {
                //var downLoadInfo = handle.Execute(list, path);
                //var result = handle.Start(downLoadInfo);
                //MessageBox.Show($"{result}", "温馨提示");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择文件存放路径";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }
                else
                {
                    textBox1.Text = dialog.SelectedPath;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = (SelectEnum)comboBox1.SelectedIndex;
            string Parm = index.EnumMetadataDisplay();
            string url = $"https://service.alilo.com.cn/gw/resource/special?classname={Parm}&classid=0";
            string result = string.Empty;
            switch (index)
            {
                case SelectEnum.All:
                case SelectEnum.Song:
                case SelectEnum.Story:
                case SelectEnum.Engish:
                case SelectEnum.Sinology:
                case SelectEnum.Poetry:
                case SelectEnum.Sleep:
                    result = HttpClinetHelper.DoPost(url, null);
                    ApiResult apiResult = new ApiResult();
                    apiResult = JsonConvert.DeserializeAnonymousType(result, apiResult);
                    if (apiResult.msg == "成功")
                    {
                        content = apiResult?.content;
                        comboBox2.Items.Clear();
                        foreach (var item in content.specialList)
                        {
                            comboBox2.Items.Add(item.name);
                        }
                        comboBox2.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show("未能获取需下载的资源，请联系开发人员", "提示");
                    }
                    break;
                default:
                    break;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length; //Set the current caret position at the end
            richTextBox1.ScrollToCaret(); //Now
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var downLoadName = comboBox2.Text;
            list = content.specialList.Where(s => s.name == downLoadName).ToList();
            var count = list.Select(x => x.musicCount).Sum();
            lab_message.Text = $"共{count}个资源";
        }
    }
}
