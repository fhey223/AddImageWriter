using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ImageWriter
{
    //// <summary>
    /// 处理图片的类（包括加水印，生成缩略图）
    /// </summary>
    public class ImageWaterMark
    {
        // 给图片加水印#region 给图片加水印
        /// <summary>
        ///     添加水印(分图片水印与文字水印两种)
        /// </summary>
        /// <param name="oldpath">原图片绝对地址</param>
        /// <param name="newpath">新图片放置的绝对地址(不可与原图片名相同)</param>
        /// <param name="wmtType">要添加的水印的类型</param>
        /// <param name="sWaterMarkContent">
        ///     水印内容，若添加文字水印，此即为要添加的文字；
        ///     若要添加图片水印，此为图片的路径(图片水印暂不可用)
        /// </param>
        public void addWaterMark(string oldpath, string newpath,
            WaterMarkType wmtType, string sWaterMarkContent,
            string _watermarkPosition, float transparence)
        {
            try
            {
                var image = Image.FromFile(oldpath);
                var b = new Bitmap(image.Width, image.Height,
                    PixelFormat.Format24bppRgb);
                var g = Graphics.FromImage(b);
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.High;
                g.DrawImage(image, 0, 0, image.Width, image.Height);
                switch (wmtType)
                {
                    case WaterMarkType.ImageMark:
                        //图片水印
                        addWatermarkImage(g,
                            new Bitmap(sWaterMarkContent),
                            _watermarkPosition, transparence, image.Width, image.Height);
                        break;
                    case WaterMarkType.TextMark:
                        //文字水印
                        addWatermarkText(g, sWaterMarkContent, _watermarkPosition, transparence,
                            image.Width, image.Height);
                        break;
                }
                b.Save(newpath);
                b.Dispose();
                image.Dispose();
            }
            catch (Exception ex)
            {
                if (File.Exists(oldpath))
                {
                    //File.Delete(oldpath);
                }
            }
            finally
            {
                if (File.Exists(oldpath))
                {
                    // File.Delete(oldpath);
                }
            }
        }

        /// <summary>
        ///     在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径 </param>
        /// <param name="Path_syp">生成的带图片水印的图片路径 </param>
        /// <param name="Path_sypf">水印图片路径 </param>
        public void AddWaterPic(string Path, string Path_syp, string Path_sypf)
        {
            var image = Image.FromFile(Path);
            var copyImage = Image.FromFile(Path_sypf);
            var g = Graphics.FromImage(image);
            var rect = new Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width,
                copyImage.Height);
            g.DrawImage(copyImage, rect, 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
            g.Dispose();
            image.Save(Path_syp);
            image.Dispose();
        }

        /**/

        /// <summary>
        ///     加水印文字
        /// </summary>
        /// <param name="picture">imge 对象</param>
        /// <param name="_watermarkText">水印文字内容</param>
        /// <param name="_watermarkPosition">水印位置</param>
        /// <param name="_width">被加水印图片的宽</param>
        /// <param name="_height">被加水印图片的高</param>
        private void addWatermarkText(Graphics picture, string _watermarkText,
            string _watermarkPosition, float transparence, int _width, int _height)
        {
            // 确定水印文字的字体大小
            int[] sizes = {32, 30, 28, 26, 24, 22, 20, 18, 16, 14, 12, 10, 8, 6, 4};
            Font crFont = null;
            var crSize = new SizeF();
            for (var i = 0; i < sizes.Length; i++)
            {
                crFont = new Font("Arial Black", sizes[i], FontStyle.Bold);
                crSize = picture.MeasureString(_watermarkText, crFont);
                if ((ushort) crSize.Width < (ushort) _width)
                {
                    break;
                }
            }
            // 生成水印图片（将文字写到图片中）
            var floatBmp = new Bitmap((int) crSize.Width + 3,
                (int) crSize.Height + 3, PixelFormat.Format32bppArgb);
            var fg = Graphics.FromImage(floatBmp);
            var pt = new PointF(0, 0);
            // 画阴影文字
            Brush TransparentBrush0 = new SolidBrush(Color.FromArgb(255, Color.Black));
            Brush TransparentBrush1 = new SolidBrush(Color.FromArgb(255, Color.Black));
            fg.DrawString(_watermarkText, crFont, TransparentBrush0, pt.X, pt.Y + 1);
            fg.DrawString(_watermarkText, crFont, TransparentBrush0, pt.X + 1, pt.Y);
            fg.DrawString(_watermarkText, crFont, TransparentBrush1, pt.X + 1, pt.Y + 1);
            fg.DrawString(_watermarkText, crFont, TransparentBrush1, pt.X, pt.Y + 2);
            fg.DrawString(_watermarkText, crFont, TransparentBrush1, pt.X + 2, pt.Y);
            TransparentBrush0.Dispose();
            TransparentBrush1.Dispose();
            // 画文字
            fg.SmoothingMode = SmoothingMode.HighQuality;
            fg.DrawString(_watermarkText,
                crFont, new SolidBrush(Color.White),
                pt.X, pt.Y, StringFormat.GenericDefault);
            // 保存刚才的操作
            fg.Save();
            fg.Dispose();
            // floatBmp.Save("d://WebSite//DIGITALKM//ttt.jpg");
            // 将水印图片加到原图中
            addWatermarkImage(
                picture,
                new Bitmap(floatBmp),
                _watermarkPosition, transparence,
                _width,
                _height);
        }

        /**/

        /// <summary>
        ///     加水印图片
        /// </summary>
        /// <param name="picture">imge 对象</param>
        /// <param name="iTheImage">Image对象（以此图片为水印）</param>
        /// <param name="_watermarkPosition">水印位置</param>
        /// <param name="transparence">水印透明度</param>
        /// <param name="_width">被加水印图片的宽</param>
        /// <param name="_height">被加水印图片的高</param>
        private void addWatermarkImage(Graphics picture, Image iTheImage,
            string _watermarkPosition, float transparence, int _width, int _height)
        {
            if (transparence == 0.0f || transparence == 1.0f)
                throw new ArgumentException("透明度值只能在0.0f~1.0f之间");
            Image watermark = new Bitmap(iTheImage);
            var imageAttributes = new ImageAttributes();
            var colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = {colorMap};
            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
            //设置透明度
            float[][] colorMatrixElements =
            {
                new[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                new[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                new[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                new[] {0.0f, 0.0f, 0.0f, transparence, 0.0f},
                new[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
            };

            var colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            var xpos = 0;
            var ypos = 0;
            var WatermarkWidth = 0;
            var WatermarkHeight = 0;
            var bl = 1d;
            //计算水印图片的比率
            //取背景的1/4宽度来比较
            if ((_width > watermark.Width*4) && (_height > watermark.Height*4))
            {
                bl = 1;
            }
            else if ((_width > watermark.Width*4) && (_height < watermark.Height*4))
            {
                bl = Convert.ToDouble(_height/4)/Convert.ToDouble(watermark.Height);
            }
            else if ((_width < watermark.Width*4) && (_height > watermark.Height*4))
            {
                bl = Convert.ToDouble(_width/4)/Convert.ToDouble(watermark.Width);
            }
            else
            {
                if (_width*watermark.Height > _height*watermark.Width)
                {
                    bl = Convert.ToDouble(_height/4)/Convert.ToDouble(watermark.Height);
                }
                else
                {
                    bl = Convert.ToDouble(_width/4)/Convert.ToDouble(watermark.Width);
                }
            }
            WatermarkWidth = Convert.ToInt32(watermark.Width*bl);
            WatermarkHeight = Convert.ToInt32(watermark.Height*bl);
            switch (_watermarkPosition)
            {
                case "LeftTop":
                    xpos = 10;
                    ypos = 10;
                    break;
                case "RightTop":
                    xpos = _width - WatermarkWidth - 10;
                    ypos = 10;
                    break;
                case "RightBottom":
                    xpos = _width - WatermarkWidth - 10;
                    ypos = _height - WatermarkHeight - 10;
                    break;
                case "LeftBottom":
                    xpos = 10;
                    ypos = _height - WatermarkHeight - 10;
                    break;
            }
            picture.DrawImage(
                watermark,
                new Rectangle(xpos, ypos, WatermarkWidth, WatermarkHeight),
                0,
                0,
                watermark.Width,
                watermark.Height,
                GraphicsUnit.Pixel,
                imageAttributes);
            watermark.Dispose();
            imageAttributes.Dispose();
        }

        /**/

        /// <summary>
        ///     加水印图片
        /// </summary>
        /// <param name="picture">imge 对象</param>
        /// <param name="WaterMarkPicPath">水印图片的地址</param>
        /// <param name="_watermarkPosition">水印位置</param>
        /// <param name="_watermarkPosition">水印透明度</param>
        /// <param name="_width">被加水印图片的宽</param>
        /// <param name="_height">被加水印图片的高</param>
        private void addWatermarkImage(Graphics picture, string WaterMarkPicPath,
            string _watermarkPosition, float transparence, int _width, int _height)
        {
            Image watermark = new Bitmap(WaterMarkPicPath);

            addWatermarkImage(picture, watermark, _watermarkPosition, transparence, _width,
                _height);
        }

        //生成缩略图#region 生成缩略图
        /**/

        /// <summary>
        ///     保存图片
        /// </summary>
        /// <param name="image">Image 对象</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="ici">指定格式的编解码参数</param>
        private void SaveImage(Image image, string savePath, ImageCodecInfo ici)
        {
            //设置 原图片 对象的 EncoderParameters 对象
            var parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(
                Encoder.Quality, (long) 90);
            image.Save(savePath, ici, parameters);
            parameters.Dispose();
        }

        /**/

        /// <summary>
        ///     获取图像编码解码器的所有相关信息
        /// </summary>
        /// <param name="mimeType">包含编码解码器的多用途网际邮件扩充协议 (MIME) 类型的字符串</param>
        /// <returns>返回图像编码解码器的所有相关信息</returns>
        private ImageCodecInfo GetCodecInfo(string mimeType)
        {
            var CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (var ici in CodecInfo)
            {
                if (ici.MimeType == mimeType)
                    return ici;
            }
            return null;
        }

        /**/

        /// <summary>
        ///     生成缩略图
        /// </summary>
        /// <param name="sourceImagePath">原图片路径(相对路径)</param>
        /// <param name="thumbnailImagePath">生成的缩略图路径,如果为空则保存为原图片路径(相对路径)</param>
        /// <param name="thumbnailImageWidth">缩略图的宽度（高度与按源图片比例自动生成）</param>
        public void ToThumbnailImages(
            string SourceImagePath,
            string ThumbnailImagePath,
            int ThumbnailImageWidth)
        {
            var htmimes = new Hashtable();
            htmimes[".jpeg"] = "image/jpeg";
            htmimes[".jpg"] = "image/jpeg";
            htmimes[".png"] = "image/png";
            htmimes[".tif"] = "image/tiff";
            htmimes[".tiff"] = "image/tiff";
            htmimes[".bmp"] = "image/bmp";
            htmimes[".gif"] = "image/gif";
            // 取得原图片的后缀
            var sExt = GetExtension(SourceImagePath);
            //从 原图片 创建 Image 对象
            var image = Image.FromFile(SourceImagePath);
            var num = ThumbnailImageWidth/4*3;
            var width = image.Width;
            var height = image.Height;
            //计算图片的比例
            if (width/(double) height >= 1.3333333333333333f)
            {
                num = height*ThumbnailImageWidth/width;
            }
            else
            {
                ThumbnailImageWidth = width*num/height;
            }
            if ((ThumbnailImageWidth < 1) || (num < 1))
            {
                return;
            }
            //用指定的大小和格式初始化 Bitmap 类的新实例
            var bitmap = new Bitmap(ThumbnailImageWidth, num,
                PixelFormat.Format32bppArgb);
            //从指定的 Image 对象创建新 Graphics 对象
            var graphics = Graphics.FromImage(bitmap);
            //清除整个绘图面并以透明背景色填充
            graphics.Clear(Color.Transparent);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.High;
            //在指定位置并且按指定大小绘制 原图片 对象
            graphics.DrawImage(image, new Rectangle(0, 0, ThumbnailImageWidth, num));
            image.Dispose();

            try
            {
                //将此 原图片 以指定格式并用指定的编解码参数保存到指定文件 
                SaveImage(bitmap, ThumbnailImagePath,
                    GetCodecInfo((string) htmimes[sExt]));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        ///     获取: 图片以'.'开头的小写字符串扩展名
        /// </summary>
        /// <param name="path">图片路径(包含完整路径,文件名及其扩展名): string</param>
        /// <returns>返回: 图片以'.'开头的小写字符串扩展名: string</returns>
        public string GetExtension(string path)
        {
            return path.Remove(0, path.LastIndexOf('.')).ToLower();
            //return path.Substring(path.LastIndexOf(".")).ToLower();
        }
    }
}