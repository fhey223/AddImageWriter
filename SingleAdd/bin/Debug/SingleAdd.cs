using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ImageWriter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImageTest
{
    public partial class SingleAdd : Form
    {
        public SingleAdd()
        {
            InitializeComponent();
        }

        private void btnAddMark_Click(object sender, EventArgs e)
        {
            var filePath = txtUpdatePath.Text;
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("请先上传图片");
                return;
            }
            //新的文件名称
            var iwm = new ImageWaterMark();
            var ext = iwm.GetExtension(filePath);
            var newfileName = Application.StartupPath + @"\Images/" + DateTime.Now.Millisecond + "" + DateTime.Now.Second + "." + ext;
            //读取配置文件
            var settingPath = File.OpenText(Application.StartupPath + @"\seting.json");//读取记事本文件
            var settingInfo = JsonConvert.DeserializeObject<JObject>(settingPath.ReadToEnd());

            //读取水印配置信息
            var WaterMarkInfo = settingInfo["WaterMark"];
            var WaterMakerName = WaterMarkInfo["Name"].ToString();
            var WaterMakerPosition = WaterMarkInfo["Position"].ToString();
            var WaterMakerTransparence = Convert.ToSingle(WaterMarkInfo["Transparence"].ToString());

            var WaterMakerFullName = Application.StartupPath + "/Images/" + WaterMakerName;
            //原图片路径，新图片路径，要添加的水印类型，要添加的文字内容或是水印图片的路径(加图片水印时用到的)
            iwm.addWaterMark(filePath, newfileName, WaterMarkType.ImageMark,WaterMakerFullName, WaterMakerPosition, WaterMakerTransparence);
            txtUpdatePath.Text = "";
            MessageBox.Show("操作完成");
        }
        private void UploadButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "请选择上传的图片";
            ofd.Filter = "图片格式|*.jpg;*.png;*.gif;*.Bmp;*.Jpeg;*.Tiff";
            //设置是否允许多选
            ofd.Multiselect = false;
            //如果你点了“确定”按钮
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var filePath = ofd.FileName;
                txtUpdatePath.Text = filePath;
            }
        }
    }
}