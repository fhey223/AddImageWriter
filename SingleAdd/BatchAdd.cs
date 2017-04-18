using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ImageWriter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImageTest
{
    public partial class BatchAdd : Form
    {
        public BatchAdd()
        {
            InitializeComponent();
        }

        private void btnAddMark_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text))
            {
                MessageBox.Show("请先选择文件夹路径");
                return;
            }
            ((Action)BatchAddWaterMark).BeginInvoke(null, null);
            
            MessageBox.Show("操作完成");
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var foldPath = dialog.SelectedPath;
                txtFilePath.Text = foldPath;
            }
        }

        private void BatchAddWaterMark()
        {
            //读取配置文件
            var settingPath = File.OpenText(Application.StartupPath + @"\seting.json");//读取记事本文件
            var settingInfo = JsonConvert.DeserializeObject<JObject>(settingPath.ReadToEnd());

            //读取水印配置信息
            var WaterMarkInfo = settingInfo["WaterMark"];
            var WaterMakerName = WaterMarkInfo["Name"].ToString();
            var WaterMakerPosition = WaterMarkInfo["Position"].ToString();
            var WaterMakerTransparence = Convert.ToSingle(WaterMarkInfo["Transparence"].ToString());

            var imagesfile = new DirectoryInfo(txtFilePath.Text);
            string[] extList = { ".jpg", ".png", ".jpeg", ".Icon", ".Bmp", ".Emf", ".Exif", ".Gif", ".Tiff", ".Wmf" };
            var fileList = imagesfile.GetFiles();
            foreach (var file in fileList)
            {
                var iwm = new ImageWaterMark();
                var ext = iwm.GetExtension(file.FullName);
                if (extList.Contains(ext))
                {
                    var newfileName = Application.StartupPath + @"\Images/" +
                                      DateTime.Now.Millisecond + "" + DateTime.Now.Second + "." + ext;
                    //原图片路径，新图片路径，要添加的水印类型，要添加的文字内容或是水印图片的路径(加图片水印时用到的)
                    iwm.addWaterMark(file.FullName, newfileName, WaterMarkType.ImageMark,
                        Application.StartupPath + "/Images/"+ WaterMakerName, WaterMakerPosition, WaterMakerTransparence);
                }
            }
        }
    }
}