using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private void button2_Click(object sender, EventArgs e)
        {
            var path = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("请先选择文件保存路径","提示");
                return;
            }
            var index = comboBox1.SelectedIndex;
            if (index==0)
            {
                MessageBox.Show("请选择分类","提示");
            }
            Handler handle = new Handler();
            handle.OnStart += (s, ev) =>
            {
                this.Invoke(new MethodInvoker(() => {
                    richTextBox1.Text += "开始下载文件，文件地址：" + ev.Uri + "\r\n";
                }), s);
            };
            handle.OnError += (s, ev) => {
                this.Invoke(new MethodInvoker(() => {
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
            var downLoadInfo=  handle.Execute(content,path).Result;
            var result=  handle.Start(downLoadInfo);
            MessageBox.Show($"{result}","温馨提示");
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
                    if (apiResult.msg=="成功")
                    {
                        int totalCount = 0;
                        foreach (var item in apiResult.content.specialList)
                        {
                            totalCount += item.musicCount;
                        }
                        Parm = Parm == "" ? "全部" : Parm;
                        MessageBox.Show($"{Parm}分类下共有{totalCount}个资源","温馨提示");
                        content = apiResult?.content;
                    }
                    else
                    {
                        MessageBox.Show("未能获取需下载的资源，请联系开发人员","提示");
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
    }
}
