using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Text;
using System.Globalization;
using System.Collections;
using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace TTF_To_BMP
{
    public partial class Form1 : Form
    {
        Letter_First_Consonant letter_first = new Letter_First_Consonant();
        Letter_Middle_Consonant letter_middle = new Letter_Middle_Consonant();
        Letter_Last_Consonant letter_last = new Letter_Last_Consonant();
        Ttf_To_Bitmap ttp_to_bitmap = new Ttf_To_Bitmap();
        Util utils = new Util();

        public Form1()
        {
            InitializeComponent();
        }
       
        string currentDirectory = Directory.GetCurrentDirectory();
        private void GetPngPixelData(string pngFilePath, int x, int y)
        {
            Bitmap bitmap = new Bitmap(pngFilePath);
            Color pixelColor = bitmap.GetPixel(x, y);

            Console.WriteLine("Pixel data at ({0}, {1}):", x, y);
            Console.WriteLine("Red: {0}", pixelColor.R);
            Console.WriteLine("Green: {0}", pixelColor.G);
            Console.WriteLine("Blue: {0}", pixelColor.B);
            Console.WriteLine("Alpha: {0}", pixelColor.A);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string OutputDirectory = Path.Combine(currentDirectory, "output");
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string filePath = dlg.FileName;
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                    string fileDirectory = Path.GetDirectoryName(filePath);

                    Console.WriteLine("File Path: " + filePath);
                    Console.WriteLine("File Name Without Extension: " + fileNameWithoutExtension);
                    Console.WriteLine("File Directory: " + fileDirectory);

                    string newDirectory = fileDirectory + "\\" + fileNameWithoutExtension + utils.Util_Get_DateTime();
                    ttp_to_bitmap.TTF_Set_FilePath(filePath, fileNameWithoutExtension);
                    /* Create Directory */
                    try
                    {
                        if (!Directory.Exists(newDirectory))
                        {
                            Directory.CreateDirectory(newDirectory);
                            Console.WriteLine("Folder created successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Folder already exists.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }


                    /* create directory of First letter consonant */
                    string newDirectoryForFirstLetter = newDirectory + "\\" + "First";
                    try
                    {
                        if (!Directory.Exists(newDirectoryForFirstLetter))
                        {
                            Directory.CreateDirectory(newDirectoryForFirstLetter);
                            Console.WriteLine("Folder created successfully.");
                            newDirectoryForFirstLetter += "\\";
                        }
                        else
                        {
                            Console.WriteLine("Folder already exists.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }

                    for (int i = 0; i < letter_first.letter_first_db.Length; i++)
                    {
                        for (int j = 0; j < letter_first.letter_first_db[0].Unicode.Length; j++)
                        {
                            letter_first.letter_first_db[i].imagePath[j] = ttp_to_bitmap.Ttf_To_Argb888Bmp(filePath, letter_first.letter_first_db[i].NameOfKorean[j], letter_first.letter_first_db[i].Unicode[j], newDirectoryForFirstLetter);
                        }
                    }

                    /* create directory of Middle letter consonant */
                    string newDirectoryForMiddleLetter = newDirectory + "\\" + "Middle";
                    try
                    {
                        if (!Directory.Exists(newDirectoryForMiddleLetter))
                        {
                            Directory.CreateDirectory(newDirectoryForMiddleLetter);
                            Console.WriteLine("Folder created successfully.");
                            newDirectoryForMiddleLetter += "\\";
                        }
                        else
                        {
                            Console.WriteLine("Folder already exists.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }

                    for (int i = 0; i < letter_middle.letter_middle_db.Length; i++)
                    {
                        for (int j = 0; j < letter_middle.letter_middle_db[0].Unicode.Length; j++)
                        {
                            letter_middle.letter_middle_db[i].imagePath[j] = ttp_to_bitmap.Ttf_To_Argb888Bmp(filePath, letter_middle.letter_middle_db[i].NameOfKorean[j], letter_middle.letter_middle_db[i].Unicode[j], newDirectoryForMiddleLetter);
                        }
                    }

                    /* create directory of Last letter consonant */
                    string newDirectoryForLastLetter = newDirectory + "\\" + "Last";
                    try
                    {
                        if (!Directory.Exists(newDirectoryForLastLetter))
                        {
                            Directory.CreateDirectory(newDirectoryForLastLetter);
                            Console.WriteLine("Folder created successfully.");
                            newDirectoryForLastLetter += "\\";
                        }
                        else
                        {
                            Console.WriteLine("Folder already exists.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }

                    for (int i = 0; i < letter_last.letter_last_db.Length; i++)
                    {
                        for (int j = 0; j < letter_last.letter_last_db[0].Unicode.Length; j++)
                        {
                            letter_last.letter_last_db[i].imagePath[j] = ttp_to_bitmap.Ttf_To_Argb888Bmp(filePath, letter_last.letter_last_db[i].NameOfKorean[j], letter_last.letter_last_db[i].Unicode[j], newDirectoryForLastLetter);
                        }
                    }

                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            progressBar1.Value = 0;
            progressBar2.Value = 0;
            progressBar3.Value = 0;

            //Detection_Proc detectionMgr = new Detection_Proc("test.png","test");

            progressBar1.Maximum = letter_first.letter_first_db.Length;
            for (int i = 0; i < letter_first.letter_first_db.Length; i++)
            {
                progressBar1.Value = i+1;
                for (int j = 0; j < letter_first.letter_first_db[0].Unicode.Length; j++)
                {
                    Detection_Proc detectionMgr = new Detection_Proc(letter_first.letter_first_db[i].imagePath[j], "test");
                }
            }

            progressBar2.Maximum = letter_middle.letter_middle_db.Length;
            for (int i = 0; i < letter_middle.letter_middle_db.Length; i++)
            {
                progressBar2.Value = i+1;
                for (int j = 0; j < letter_middle.letter_middle_db[0].Unicode.Length; j++)
                {
                    Detection_Proc detectionMgr = new Detection_Proc(letter_middle.letter_middle_db[i].imagePath[j], "test");
                }
            }

            progressBar3.Maximum = letter_last.letter_last_db.Length;
            for (int i = 0; i < letter_last.letter_last_db.Length; i++)
            {
                progressBar3.Value = i + 1;
                for (int j = 0; j < letter_last.letter_last_db[0].Unicode.Length; j++)
                {
                    Detection_Proc detectionMgr = new Detection_Proc(letter_last.letter_last_db[i].imagePath[j], "test");
                }
            }

            MessageBox.Show("변환 완료!!");
        }

        private void button2_Click(object sender, EventArgs e)
        {



            string currentDirectory = Directory.GetCurrentDirectory();
            //Process.Start(currentDirectory);



            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = currentDirectory,
                UseShellExecute = true,
                Verb = "open"
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string filePath = dlg.FileName;
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                    string fileDirectory = Path.GetDirectoryName(filePath);

                    Console.WriteLine("File Path: " + filePath);
                    Console.WriteLine("File Name Without Extension: " + fileNameWithoutExtension);
                    Console.WriteLine("File Directory: " + fileDirectory);

                    string newDirectory = fileDirectory + "\\" + fileNameWithoutExtension + utils.Util_Get_DateTime();
                    ttp_to_bitmap.TTF_Set_FilePath(filePath, fileNameWithoutExtension);
                    /* Create Directory */
                    try
                    {
                        if (!Directory.Exists(newDirectory))
                        {
                            Directory.CreateDirectory(newDirectory);
                            Console.WriteLine("Folder created successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Folder already exists.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }

                    Detection_Proc detectionMgr = new Detection_Proc("test.png", "test");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            
        }
    }
}