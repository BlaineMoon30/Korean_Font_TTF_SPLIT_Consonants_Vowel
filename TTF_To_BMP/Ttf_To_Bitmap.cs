using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TTF_To_BMP
{
    struct Tft_To_Bitmpa_Mgr
    {
        public string ttf_file_path;
        public string ttf_file_name;
    }
    internal class Ttf_To_Bitmap
    {
        
        public const int FONT_SIZE = 20;
        public const int BITMAP_WIDTH = 32;
        public const int BITMAP_HEIGHT = 32;


        public Tft_To_Bitmpa_Mgr ttf_to_bitmap_mgr = new Tft_To_Bitmpa_Mgr();
        public Ttf_To_Bitmap()
        {

        }

        public void TTF_Set_FilePath(string Ttf_FilePath, string Ttf_Name)
        {
            ttf_to_bitmap_mgr.ttf_file_path = Ttf_FilePath;
            ttf_to_bitmap_mgr.ttf_file_name = Ttf_Name;
        }

        public string Ttf_To_Argb888Bmp(string ttfFilePath, string character, string unicode, string bmpFilePath)
        {
            PrivateFontCollection fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(ttfFilePath);

            Font font = new Font(fontCollection.Families[0], FONT_SIZE);
            Bitmap bitmap = new Bitmap(BITMAP_WIDTH, BITMAP_HEIGHT, PixelFormat.Format32bppArgb);

            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.DrawString(unicode.ToString(), font, Brushes.Black, new PointF(0, 0));
            //bitmap.Save(bmpFilePath, ImageFormat.Bmp);

            string outputFileName = bmpFilePath + character + ".png";
  
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    bitmap.Save(memory, ImageFormat.Png);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            return outputFileName;
        }

        public void Ttf_To_Bmp(string ttfFilePath, char character, string currentDirectory)
        {
            PrivateFontCollection fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(ttfFilePath);
            Font font = new Font(fontCollection.Families[0], 32);

            Bitmap bitmap = new Bitmap(32, 32);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            graphics.DrawString(character.ToString(), font, Brushes.White, new PointF(0, 0));

            string outputFileName = "test.bmp";
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    bitmap.Save(memory, ImageFormat.Bmp);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

    }
}
