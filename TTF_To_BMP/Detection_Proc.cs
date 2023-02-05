using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TTF_To_BMP
{

    struct Detection_Mgr
    {
        public byte[,] DetectionArray;
        public byte[,] DetectionArrayWithAlpha;

        public int width;
        public int height;

        public Boolean is_detect_new_segment;
        public Boolean is_detect_left_side;
        public Boolean is_detect_right_side;

        public int start_x;
        public int start_y;
    }

    struct Original_Font_Info
    {
        public byte[] OriginalArray1D;
        public byte[,] OriginalArray2D;

        public byte[] OriginalArray1D_With_Alpha;
        public byte[,] OriginalArray2D_With_Alpha;

        public int width;
        public int height;
        public string filepath;
        public string character;
    }
        
    internal class Detection_Proc
    {
        public const int DETECTION_TRY_1 = 0;
        public const int DETECTION_TRY_2 = 1;
        public const int DETECTION_TRY_3 = 2;
        public const int DETECTION_TRY_4 = 3;
        public const int DETECTION_TRY_5 = 4;
        public const int DETECTION_TRY_6 = 5;
        public const int DETECTION_TRY_7 = 6;
        public const int DETECTION_TRY_8 = 7;
        public const int DETECTION_TRY_TOTAL_NUM = 7;

        public Original_Font_Info orignal_font_info = new Original_Font_Info();
        public Detection_Mgr[] detection_mgr = new Detection_Mgr[DETECTION_TRY_TOTAL_NUM];

        public Detection_Proc(string filepath, string character)
        {
            /* Init Original Source */
            orignal_font_info.filepath = filepath;
            orignal_font_info.character = character;

            //Bitmap bitmap = new Bitmap(orignal_font_info.filepath);


            Bitmap bitmap_orign = new Bitmap(orignal_font_info.filepath);
            Bitmap bitmap = new Bitmap(bitmap_orign);
            orignal_font_info.width = bitmap.Width;
            orignal_font_info.height = bitmap.Height;
            

            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    Color pixelColor = bitmap.GetPixel(i, j);
                    if((pixelColor.A > 0)&&(pixelColor.A < 255))
                    {
                        Color color = Color.FromArgb(0, 0, 0, 0);
                        bitmap.SetPixel(i, j, color);
                    }
                }
            }

            orignal_font_info.OriginalArray1D_With_Alpha = new byte[orignal_font_info.width * orignal_font_info.height];
            orignal_font_info.OriginalArray2D_With_Alpha = new byte[orignal_font_info.width, orignal_font_info.height];

            StreamWriter writer_alpha;

            //moon
            string directory = Path.GetDirectoryName(filepath)+"\\";
            //string directory = Path.GetDirectoryName(filepath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(orignal_font_info.filepath);
            string outputFileName = directory + fileNameWithoutExtension + "__Full.txt";
            writer_alpha = File.CreateText(outputFileName);

            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    Color pixelColor = bitmap_orign.GetPixel(j, i);

                    if (pixelColor.A != 0)
                    {
                        Console.WriteLine("{0},{1}", j, i);
                        orignal_font_info.OriginalArray1D_With_Alpha[(i * orignal_font_info.width) + j] = pixelColor.A;
                        orignal_font_info.OriginalArray2D_With_Alpha[i, j] = pixelColor.A;

                        int input = pixelColor.A;
                        string result = input.ToString().PadLeft(3, '0') + ",";
                        writer_alpha.Write(result);
                    }
                    else
                    {
                        orignal_font_info.OriginalArray1D_With_Alpha[(i * orignal_font_info.width) + j] = 0;
                        orignal_font_info.OriginalArray2D_With_Alpha[i, j] = 0;

                        int input = pixelColor.A;
                        string result = input.ToString().PadLeft(3, '0') + ",";
                        writer_alpha.Write(result);
                    }
                }
                writer_alpha.Write("\r\n");
            }

            writer_alpha.Close();


            orignal_font_info.OriginalArray1D = new byte[orignal_font_info.width * orignal_font_info.height];
            orignal_font_info.OriginalArray2D = new byte[orignal_font_info.width, orignal_font_info.height];

            StreamWriter writer;

            //moon
            directory = Path.GetDirectoryName(filepath)+"\\";
            //directory = Path.GetDirectoryName(filepath) ;
            fileNameWithoutExtension = Path.GetFileNameWithoutExtension(orignal_font_info.filepath);
            outputFileName = directory + fileNameWithoutExtension + "__Full_Without_Alpha.txt";
            writer = File.CreateText(outputFileName);

            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    Color pixelColor = bitmap.GetPixel(j, i);

                    if (pixelColor.A != 0)
                    {
                        Console.WriteLine("{0},{1}", j, i);
                        orignal_font_info.OriginalArray1D[(i * orignal_font_info.width) + j] = pixelColor.A;
                        orignal_font_info.OriginalArray2D[i, j] = pixelColor.A;

                        int input = pixelColor.A;
                        string result = input.ToString().PadLeft(3, '0')+",";
                        writer.Write(result);
                    }
                    else
                    {
                        orignal_font_info.OriginalArray1D[(i * orignal_font_info.width) + j] = 0;
                        orignal_font_info.OriginalArray2D[i, j] = 0;

                        int input = pixelColor.A;
                        string result = input.ToString().PadLeft(3, '0') + ",";
                        writer.Write(result);
                    }
                }
                writer.Write("\r\n");
            }

            writer.Close();


            /* Prepare to Detection Mgr */
            for (int i = 0; i < DETECTION_TRY_TOTAL_NUM; i++)
            {
                detection_mgr[i].DetectionArray = new byte[orignal_font_info.width, orignal_font_info.height];
                Array.Clear(detection_mgr[i].DetectionArray, 0, detection_mgr[i].DetectionArray.Length);

                detection_mgr[i].DetectionArrayWithAlpha = new byte[orignal_font_info.width, orignal_font_info.height];
                Array.Clear(detection_mgr[i].DetectionArrayWithAlpha, 0, detection_mgr[i].DetectionArrayWithAlpha.Length);

                detection_mgr[i].is_detect_new_segment = false;
                detection_mgr[i].is_detect_left_side = false;
                detection_mgr[i].is_detect_right_side = false;

                detection_mgr[i].start_x = 0;
                detection_mgr[i].start_y = 0;
            }


            /* 1st try BM1 */
            Boolean is_detect_start_point = false;
            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    if (orignal_font_info.OriginalArray2D[i, j] != 0)
                    {
                        detection_mgr[DETECTION_TRY_1].start_x = j;
                        detection_mgr[DETECTION_TRY_1].start_y = i;
                        detection_mgr[DETECTION_TRY_1].is_detect_new_segment = true;

                        detection_mgr[DETECTION_TRY_1].DetectionArray[i, j] = orignal_font_info.OriginalArray2D[i, j];
                        is_detect_start_point = true;
                        break;
                    }
                }

                if (is_detect_start_point == true)
                {
                    break;
                }
            }

            if (detection_mgr[DETECTION_TRY_1].is_detect_new_segment == true)
            {
                SearchLetterConsonant(orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_1].DetectionArray, orignal_font_info.width, orignal_font_info.height);
                WriteToTextFileWithArray(filepath, "_BM1_Split_1", detection_mgr[DETECTION_TRY_1].DetectionArray, orignal_font_info.width, orignal_font_info.height);

                string split_file_1_path = directory + fileNameWithoutExtension + "_BM1_Split_1.png";
                //WriteToSplitImage(bitmap, split_file_1_path, orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_1].DetectionArray, orignal_font_info.width, orignal_font_info.height);

                string split_file_1_alpha_path = directory + fileNameWithoutExtension + "_BM1_Split_1.png";
                WriteToSplitImageWithAlpha(bitmap, bitmap_orign, split_file_1_alpha_path, orignal_font_info.OriginalArray2D_With_Alpha, orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_1].DetectionArray, orignal_font_info.width, orignal_font_info.height);
            }

            /* 1st try BM2 */
            is_detect_start_point = false;
            detection_mgr[DETECTION_TRY_1].is_detect_new_segment = false;
            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != 0)
                    {
                        detection_mgr[DETECTION_TRY_1].start_x = j;
                        detection_mgr[DETECTION_TRY_1].start_y = i;
                        detection_mgr[DETECTION_TRY_1].is_detect_new_segment = true;

                        detection_mgr[DETECTION_TRY_1].DetectionArrayWithAlpha[i, j] = orignal_font_info.OriginalArray2D_With_Alpha[i, j];
                        is_detect_start_point = true;
                        break;
                    }
                }

                if (is_detect_start_point == true)
                {
                    break;
                }
            }

            if (detection_mgr[DETECTION_TRY_1].is_detect_new_segment == true)
            {
                SearchLetterConsonant(orignal_font_info.OriginalArray2D_With_Alpha, detection_mgr[DETECTION_TRY_1].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);
                WriteToTextFileWithArray(filepath, "_BM2_Split_1", detection_mgr[DETECTION_TRY_1].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);

                string split_file_1_path = directory + fileNameWithoutExtension + "_BM2_Split_1.png";
                WriteToSplitImage(bitmap_orign, split_file_1_path, orignal_font_info.OriginalArray2D_With_Alpha, detection_mgr[DETECTION_TRY_1].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);               
            }

            /* 2nd try */
            is_detect_start_point = false;
            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_1].DetectionArray[i, j])
                    {
                        detection_mgr[DETECTION_TRY_2].start_x = j;
                        detection_mgr[DETECTION_TRY_2].start_y = i;
                        detection_mgr[DETECTION_TRY_2].is_detect_new_segment = true;

                        detection_mgr[DETECTION_TRY_2].DetectionArray[i, j] = orignal_font_info.OriginalArray2D[i, j];
                        is_detect_start_point = true;
                        break;
                    }
                }

                if (is_detect_start_point == true)
                {
                    break;
                }
            }

            if (detection_mgr[DETECTION_TRY_2].is_detect_new_segment == true)
            {
                SearchLetterConsonant(orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_2].DetectionArray, orignal_font_info.width, orignal_font_info.height);
                WriteToTextFileWithArray(filepath, "_BM1_Split_2", detection_mgr[DETECTION_TRY_2].DetectionArray, orignal_font_info.width, orignal_font_info.height);

                string split_file_2_path = directory + fileNameWithoutExtension + "_BM1_Split_2.png";
                //WriteToSplitImage(bitmap, split_file_2_path, orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_2].DetectionArray, orignal_font_info.width, orignal_font_info.height);

                string split_file_2_alpha_path = directory + fileNameWithoutExtension + "_BM1_Split_2.png";
                WriteToSplitImageWithAlpha(bitmap, bitmap_orign, split_file_2_alpha_path, orignal_font_info.OriginalArray2D_With_Alpha, orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_2].DetectionArray, orignal_font_info.width, orignal_font_info.height);
            }

            /* 2nd try BM2 */
            is_detect_start_point = false;
            detection_mgr[DETECTION_TRY_2].is_detect_new_segment = false;
            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_1].DetectionArrayWithAlpha[i, j])
                    {
                        detection_mgr[DETECTION_TRY_2].start_x = j;
                        detection_mgr[DETECTION_TRY_2].start_y = i;
                        detection_mgr[DETECTION_TRY_2].is_detect_new_segment = true;

                        detection_mgr[DETECTION_TRY_2].DetectionArrayWithAlpha[i, j] = orignal_font_info.OriginalArray2D_With_Alpha[i, j];
                        is_detect_start_point = true;
                        break;
                    }
                }

                if (is_detect_start_point == true)
                {
                    break;
                }
            }

            if (detection_mgr[DETECTION_TRY_2].is_detect_new_segment == true)
            {
                SearchLetterConsonant(orignal_font_info.OriginalArray2D_With_Alpha, detection_mgr[DETECTION_TRY_2].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);
                WriteToTextFileWithArray(filepath, "_BM2_Split_2", detection_mgr[DETECTION_TRY_2].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);

                string split_file_1_path = directory + fileNameWithoutExtension + "_BM2_Split_2.png";
                WriteToSplitImage(bitmap_orign, split_file_1_path, orignal_font_info.OriginalArray2D_With_Alpha, detection_mgr[DETECTION_TRY_2].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);
            }

            /* 3rd try */
            is_detect_start_point = false;
            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_2].DetectionArray[i, j])
                    {
                        if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_1].DetectionArray[i, j])
                        {
                            detection_mgr[DETECTION_TRY_3].start_x = j;
                            detection_mgr[DETECTION_TRY_3].start_y = i;
                            detection_mgr[DETECTION_TRY_3].is_detect_new_segment = true;

                            detection_mgr[DETECTION_TRY_3].DetectionArray[i, j] = orignal_font_info.OriginalArray2D[i, j];
                            is_detect_start_point = true;
                            break;
                        }
                    }
                }

                if (is_detect_start_point == true)
                {
                    break;
                }
            }

            if (detection_mgr[DETECTION_TRY_3].is_detect_new_segment == true)
            {
                SearchLetterConsonant(orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_3].DetectionArray, orignal_font_info.width, orignal_font_info.height);
                WriteToTextFileWithArray(filepath, "_BM1_Split_3", detection_mgr[DETECTION_TRY_3].DetectionArray, orignal_font_info.width, orignal_font_info.height);

                string split_file_3_path = directory + fileNameWithoutExtension + "_BM1_Split_3.png";
                //WriteToSplitImage(bitmap, split_file_3_path, orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_3].DetectionArray, orignal_font_info.width, orignal_font_info.height);

                string split_file_3_alpha_path = directory + fileNameWithoutExtension + "_BM1_Split_3.png";
                WriteToSplitImageWithAlpha(bitmap, bitmap_orign, split_file_3_alpha_path, orignal_font_info.OriginalArray2D_With_Alpha, orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_3].DetectionArray, orignal_font_info.width, orignal_font_info.height);
            }

            /* 3rd try BM2 */
            is_detect_start_point = false;
            detection_mgr[DETECTION_TRY_3].is_detect_new_segment = false;
            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_2].DetectionArrayWithAlpha[i, j])
                    {
                        if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_1].DetectionArrayWithAlpha[i, j])
                        {
                            detection_mgr[DETECTION_TRY_3].start_x = j;
                            detection_mgr[DETECTION_TRY_3].start_y = i;
                            detection_mgr[DETECTION_TRY_3].is_detect_new_segment = true;

                            detection_mgr[DETECTION_TRY_3].DetectionArrayWithAlpha[i, j] = orignal_font_info.OriginalArray2D_With_Alpha[i, j];
                            is_detect_start_point = true;
                            break;
                        }
                    }
                }

                if (is_detect_start_point == true)
                {
                    break;
                }
            }

            if (detection_mgr[DETECTION_TRY_3].is_detect_new_segment == true)
            {
                SearchLetterConsonant(orignal_font_info.OriginalArray2D_With_Alpha, detection_mgr[DETECTION_TRY_3].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);
                WriteToTextFileWithArray(filepath, "_BM2_Split_3", detection_mgr[DETECTION_TRY_3].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);

                string split_file_1_path = directory + fileNameWithoutExtension + "_BM2_Split_3.png";
                WriteToSplitImage(bitmap_orign, split_file_1_path, orignal_font_info.OriginalArray2D_With_Alpha, detection_mgr[DETECTION_TRY_3].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);
            }


            /* 4th try */
            is_detect_start_point = false;
            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_3].DetectionArray[i, j])
                    {
                        if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_2].DetectionArray[i, j])
                        {
                            if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_1].DetectionArray[i, j])
                            {
                                detection_mgr[DETECTION_TRY_4].start_x = j;
                                detection_mgr[DETECTION_TRY_4].start_y = i;
                                detection_mgr[DETECTION_TRY_4].is_detect_new_segment = true;

                                detection_mgr[DETECTION_TRY_4].DetectionArray[i, j] = orignal_font_info.OriginalArray2D[i, j];
                                is_detect_start_point = true;
                                break;
                            }
                        }
                    }
                }

                if (is_detect_start_point == true)
                {
                    break;
                }
            }

            if (detection_mgr[DETECTION_TRY_4].is_detect_new_segment == true)
            {
                SearchLetterConsonant(orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_4].DetectionArray, orignal_font_info.width, orignal_font_info.height);
                WriteToTextFileWithArray(filepath, "_BM1_Split_4", detection_mgr[DETECTION_TRY_4].DetectionArray, orignal_font_info.width, orignal_font_info.height);

                string split_file_4_path = directory + fileNameWithoutExtension + "_BM1_Split_4.png";
                //WriteToSplitImage(bitmap, split_file_4_path, orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_4].DetectionArray, orignal_font_info.width, orignal_font_info.height);

                string split_file_4_alpha_path = directory + fileNameWithoutExtension + "_BM1_Split_4.png";
                WriteToSplitImageWithAlpha(bitmap, bitmap_orign, split_file_4_alpha_path, orignal_font_info.OriginalArray2D_With_Alpha, orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_4].DetectionArray, orignal_font_info.width, orignal_font_info.height);
            }

            /* 4th try BM2 */
            is_detect_start_point = false;
            detection_mgr[DETECTION_TRY_4].is_detect_new_segment = false;
            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_3].DetectionArrayWithAlpha[i, j])
                    {
                        if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_2].DetectionArrayWithAlpha[i, j])
                        {
                            if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_1].DetectionArrayWithAlpha[i, j])
                            {
                                detection_mgr[DETECTION_TRY_4].start_x = j;
                                detection_mgr[DETECTION_TRY_4].start_y = i;
                                detection_mgr[DETECTION_TRY_4].is_detect_new_segment = true;

                                detection_mgr[DETECTION_TRY_4].DetectionArrayWithAlpha[i, j] = orignal_font_info.OriginalArray2D_With_Alpha[i, j];
                                is_detect_start_point = true;
                                break;
                            }
                        }
                    }
                }

                if (is_detect_start_point == true)
                {
                    break;
                }
            }

            if (detection_mgr[DETECTION_TRY_4].is_detect_new_segment == true)
            {
                SearchLetterConsonant(orignal_font_info.OriginalArray2D_With_Alpha, detection_mgr[DETECTION_TRY_4].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);
                WriteToTextFileWithArray(filepath, "_BM2_Split_4", detection_mgr[DETECTION_TRY_4].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);

                string split_file_1_path = directory + fileNameWithoutExtension + "_BM2_Split_4.png";
                WriteToSplitImage(bitmap_orign, split_file_1_path, orignal_font_info.OriginalArray2D_With_Alpha, detection_mgr[DETECTION_TRY_4].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);
            }


            /* 5th try */
            is_detect_start_point = false;
            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_4].DetectionArray[i, j])
                    {
                        if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_3].DetectionArray[i, j])
                        {
                            if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_2].DetectionArray[i, j])
                            {
                                if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_1].DetectionArray[i, j])
                                {
                                    detection_mgr[DETECTION_TRY_5].start_x = j;
                                    detection_mgr[DETECTION_TRY_5].start_y = i;
                                    detection_mgr[DETECTION_TRY_5].is_detect_new_segment = true;

                                    detection_mgr[DETECTION_TRY_5].DetectionArray[i, j] = orignal_font_info.OriginalArray2D[i, j];
                                    is_detect_start_point = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (is_detect_start_point == true)
                {
                    break;
                }
            }

            if (detection_mgr[DETECTION_TRY_5].is_detect_new_segment == true)
            {
                SearchLetterConsonant(orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_5].DetectionArray, orignal_font_info.width, orignal_font_info.height);
                WriteToTextFileWithArray(filepath, "_BM1_Split_5", detection_mgr[DETECTION_TRY_5].DetectionArray, orignal_font_info.width, orignal_font_info.height);

                string split_file_5_path = directory + fileNameWithoutExtension + "_BM1_Split_5.png";
                //WriteToSplitImage(bitmap, split_file_5_path, orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_5].DetectionArray, orignal_font_info.width, orignal_font_info.height);

                string split_file_5_alpha_path = directory + fileNameWithoutExtension + "_BM1_Split_5.png";
                WriteToSplitImageWithAlpha(bitmap,bitmap_orign, split_file_5_alpha_path, orignal_font_info.OriginalArray2D_With_Alpha, orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_5].DetectionArray, orignal_font_info.width, orignal_font_info.height);
            }

            /* 5th try BM2 */
            is_detect_start_point = false;
            detection_mgr[DETECTION_TRY_5].is_detect_new_segment = false;
            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_4].DetectionArrayWithAlpha[i, j])
                    {
                        if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_3].DetectionArrayWithAlpha[i, j])
                        {
                            if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_2].DetectionArrayWithAlpha[i, j])
                            {
                                if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_1].DetectionArrayWithAlpha[i, j])
                                {
                                    detection_mgr[DETECTION_TRY_5].start_x = j;
                                    detection_mgr[DETECTION_TRY_5].start_y = i;
                                    detection_mgr[DETECTION_TRY_5].is_detect_new_segment = true;

                                    detection_mgr[DETECTION_TRY_5].DetectionArrayWithAlpha[i, j] = orignal_font_info.OriginalArray2D_With_Alpha[i, j];
                                    is_detect_start_point = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (is_detect_start_point == true)
                {
                    break;
                }
            }

            if (detection_mgr[DETECTION_TRY_5].is_detect_new_segment == true)
            {
                SearchLetterConsonant(orignal_font_info.OriginalArray2D_With_Alpha, detection_mgr[DETECTION_TRY_5].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);
                WriteToTextFileWithArray(filepath, "_BM2_Split_5", detection_mgr[DETECTION_TRY_5].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);

                string split_file_1_path = directory + fileNameWithoutExtension + "_BM2_Split_5.png";
                WriteToSplitImage(bitmap_orign, split_file_1_path, orignal_font_info.OriginalArray2D_With_Alpha, detection_mgr[DETECTION_TRY_5].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);
            }


            /* 6th try */
            is_detect_start_point = false;
            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_5].DetectionArray[i, j])
                    {
                        if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_4].DetectionArray[i, j])
                        {
                            if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_3].DetectionArray[i, j])
                            {
                                if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_2].DetectionArray[i, j])
                                {
                                    if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_1].DetectionArray[i, j])
                                    {
                                        detection_mgr[DETECTION_TRY_6].start_x = j;
                                        detection_mgr[DETECTION_TRY_6].start_y = i;
                                        detection_mgr[DETECTION_TRY_6].is_detect_new_segment = true;

                                        detection_mgr[DETECTION_TRY_6].DetectionArray[i, j] = orignal_font_info.OriginalArray2D[i, j];
                                        is_detect_start_point = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (is_detect_start_point == true)
                {
                    break;
                }
            }

            if (detection_mgr[DETECTION_TRY_6].is_detect_new_segment == true)
            {
                SearchLetterConsonant(orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_6].DetectionArray, orignal_font_info.width, orignal_font_info.height);
                WriteToTextFileWithArray(filepath, "_BM1_Split_6", detection_mgr[DETECTION_TRY_6].DetectionArray, orignal_font_info.width, orignal_font_info.height);

                string split_file_5_path = directory + fileNameWithoutExtension + "_BM1_Split_6.png";
                //WriteToSplitImage(bitmap, split_file_5_path, orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_6].DetectionArray, orignal_font_info.width, orignal_font_info.height);

                string split_file_5_alpha_path = directory + fileNameWithoutExtension + "_BM1_Split_6.png";
                WriteToSplitImageWithAlpha(bitmap, bitmap_orign, split_file_5_alpha_path, orignal_font_info.OriginalArray2D_With_Alpha, orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_6].DetectionArray, orignal_font_info.width, orignal_font_info.height);
            }

            /* 6th try BM2 */
            is_detect_start_point = false;
            detection_mgr[DETECTION_TRY_6].is_detect_new_segment = false;
            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_5].DetectionArrayWithAlpha[i, j])
                    {
                        if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_4].DetectionArrayWithAlpha[i, j])
                        {
                            if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_3].DetectionArrayWithAlpha[i, j])
                            {
                                if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_2].DetectionArrayWithAlpha[i, j])
                                {
                                    if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_1].DetectionArrayWithAlpha[i, j])
                                    {
                                        detection_mgr[DETECTION_TRY_6].start_x = j;
                                        detection_mgr[DETECTION_TRY_6].start_y = i;
                                        detection_mgr[DETECTION_TRY_6].is_detect_new_segment = true;

                                        detection_mgr[DETECTION_TRY_6].DetectionArrayWithAlpha[i, j] = orignal_font_info.OriginalArray2D_With_Alpha[i, j];
                                        is_detect_start_point = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (is_detect_start_point == true)
                {
                    break;
                }
            }

            if (detection_mgr[DETECTION_TRY_6].is_detect_new_segment == true)
            {
                SearchLetterConsonant(orignal_font_info.OriginalArray2D_With_Alpha, detection_mgr[DETECTION_TRY_6].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);
                WriteToTextFileWithArray(filepath, "_BM2_Split_6", detection_mgr[DETECTION_TRY_6].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);

                string split_file_1_path = directory + fileNameWithoutExtension + "_BM2_Split_6.png";
                WriteToSplitImage(bitmap_orign, split_file_1_path, orignal_font_info.OriginalArray2D_With_Alpha, detection_mgr[DETECTION_TRY_6].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);
            }

            /* 7th try */
            is_detect_start_point = false;
            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_6].DetectionArray[i, j])
                    {
                        if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_5].DetectionArray[i, j])
                        {
                            if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_4].DetectionArray[i, j])
                            {
                                if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_3].DetectionArray[i, j])
                                {
                                    if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_2].DetectionArray[i, j])
                                    {
                                        if (orignal_font_info.OriginalArray2D[i, j] != detection_mgr[DETECTION_TRY_1].DetectionArray[i, j])
                                        {
                                            detection_mgr[DETECTION_TRY_7].start_x = j;
                                            detection_mgr[DETECTION_TRY_7].start_y = i;
                                            detection_mgr[DETECTION_TRY_7].is_detect_new_segment = true;

                                            detection_mgr[DETECTION_TRY_7].DetectionArray[i, j] = orignal_font_info.OriginalArray2D[i, j];
                                            is_detect_start_point = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (is_detect_start_point == true)
                {
                    break;
                }
            }

            if (detection_mgr[DETECTION_TRY_7].is_detect_new_segment == true)
            {
                SearchLetterConsonant(orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_7].DetectionArray, orignal_font_info.width, orignal_font_info.height);
                WriteToTextFileWithArray(filepath, "_BM1_Split_7", detection_mgr[DETECTION_TRY_7].DetectionArray, orignal_font_info.width, orignal_font_info.height);

                string split_file_5_path = directory + fileNameWithoutExtension + "_BM1_Split_7.png";
                //WriteToSplitImage(bitmap, split_file_5_path, orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_7].DetectionArray, orignal_font_info.width, orignal_font_info.height);

                string split_file_5_alpha_path = directory + fileNameWithoutExtension + "_BM1_Split_7.png";
                WriteToSplitImageWithAlpha(bitmap, bitmap_orign, split_file_5_alpha_path, orignal_font_info.OriginalArray2D_With_Alpha, orignal_font_info.OriginalArray2D, detection_mgr[DETECTION_TRY_7].DetectionArray, orignal_font_info.width, orignal_font_info.height);
            }

            /* 7th try BM2 */
            is_detect_start_point = false;
            detection_mgr[DETECTION_TRY_7].is_detect_new_segment = false;
            for (int i = 0; i < orignal_font_info.height; i++)
            {
                for (int j = 0; j < orignal_font_info.width; j++)
                {
                    if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_6].DetectionArrayWithAlpha[i, j])
                    {
                        if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_5].DetectionArrayWithAlpha[i, j])
                        {
                            if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_4].DetectionArrayWithAlpha[i, j])
                            {
                                if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_3].DetectionArrayWithAlpha[i, j])
                                {
                                    if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_2].DetectionArrayWithAlpha[i, j])
                                    {
                                        if (orignal_font_info.OriginalArray2D_With_Alpha[i, j] != detection_mgr[DETECTION_TRY_1].DetectionArrayWithAlpha[i, j])
                                        {
                                            detection_mgr[DETECTION_TRY_7].start_x = j;
                                            detection_mgr[DETECTION_TRY_7].start_y = i;
                                            detection_mgr[DETECTION_TRY_7].is_detect_new_segment = true;

                                            detection_mgr[DETECTION_TRY_7].DetectionArrayWithAlpha[i, j] = orignal_font_info.OriginalArray2D_With_Alpha[i, j];
                                            is_detect_start_point = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (is_detect_start_point == true)
                {
                    break;
                }
            }

            if (detection_mgr[DETECTION_TRY_7].is_detect_new_segment == true)
            {
                SearchLetterConsonant(orignal_font_info.OriginalArray2D_With_Alpha, detection_mgr[DETECTION_TRY_7].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);
                WriteToTextFileWithArray(filepath, "_BM2_Split_7", detection_mgr[DETECTION_TRY_7].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);

                string split_file_1_path = directory + fileNameWithoutExtension + "_BM2_Split_7.png";
                WriteToSplitImage(bitmap_orign, split_file_1_path, orignal_font_info.OriginalArray2D_With_Alpha, detection_mgr[DETECTION_TRY_7].DetectionArrayWithAlpha, orignal_font_info.width, orignal_font_info.height);
            }
        }

        public void WriteToSplitImage(Bitmap bmp, string file_path, byte[,] src, byte[,] dst, int width, int height)
        {
            /**/
            Bitmap bitmap_copy = new Bitmap(bmp);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (src[i, j] != dst[i, j])
                    {
                        Color color = Color.FromArgb(0, 0, 0, 0);
                        bitmap_copy.SetPixel(j, i, color);
                    }
                }
            }
            Graphics graphics = Graphics.FromImage(bitmap_copy);
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            //graphics.DrawString(unicode.ToString(), font, Brushes.Black, new PointF(0, 0));

            string split_file_1_path = file_path;

            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(split_file_1_path, FileMode.Create, FileAccess.ReadWrite))
                {
                    bitmap_copy.Save(memory, ImageFormat.Png);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            /**/
        }

        public void WriteToSplitImageWithAlpha(Bitmap bmp,Bitmap origin_bmp, string file_path, byte[,] src_alpha, byte[,] src, byte[,] dst, int width, int height)
        {
            /**/
            Bitmap bitmap_copy = new Bitmap(bmp);
            Bitmap origin_bmps = new Bitmap(origin_bmp);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (src[i, j] != dst[i, j])
                    {
                        Color color = Color.FromArgb(0, 0, 0, 0);
                        bitmap_copy.SetPixel(j, i, color);
                    }
                }
            }

            /*
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color pixelColor = origin_bmps.GetPixel(j,i);
                    Console.Write("@ x={0}, y{1}, v={2} \r\n",j,i,pixelColor.A );
                }
            }*/

            //Merge Alpha
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (dst[i, j] == 255)
                    {
                        /* up */
                        if ((i - 1) >= 0) {
                            if (src_alpha[i - 1, j] != 0)
                            {
                                //Color pixelColor = origin_bmp.GetPixel(i-1, j);
                                //bitmap_copy.SetPixel(i-1, j, pixelColor);
                                Color pixelColor = origin_bmp.GetPixel(j, i-1);
                                bitmap_copy.SetPixel(j,i - 1, pixelColor);
                            }
                        }
                        /* down */
                        if ((i + 1) < height)
                        {
                            if (src_alpha[i + 1, j] != 0)
                            {
                                //Color pixelColor = origin_bmp.GetPixel(i+1, j);
                                //bitmap_copy.SetPixel(i+1, j, pixelColor);

                                Color pixelColor = origin_bmp.GetPixel(j,i+1);
                                bitmap_copy.SetPixel(j,i+1, pixelColor);
                            }
                        }

                        /* Left */
                        if ((j - 1) >= 0)
                        {
                            if (src_alpha[i, j -1 ] != 0)
                            {
                                //Color pixelColor = origin_bmp.GetPixel(i, j -1 );
                                //bitmap_copy.SetPixel(i, j-1, pixelColor);

                                Color pixelColor = origin_bmp.GetPixel( j - 1,i);
                                bitmap_copy.SetPixel( j - 1,i, pixelColor);
                            }
                        }

                        /* Right */
                        if ((j + 1) < width)
                        {
                            if (src_alpha[i, j + 1] != 0)
                            {
                                //Color pixelColor = origin_bmp.GetPixel(i, j + 1);
                                //bitmap_copy.SetPixel(i, j + 1, pixelColor);

                                Color pixelColor = origin_bmp.GetPixel( j + 1,i);
                                bitmap_copy.SetPixel( j + 1,i, pixelColor);
                            }
                        }
                    }
                }
            }

            Graphics graphics = Graphics.FromImage(bitmap_copy);
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            //graphics.DrawString(unicode.ToString(), font, Brushes.Black, new PointF(0, 0));

            string split_file_1_path = file_path;

            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(split_file_1_path, FileMode.Create, FileAccess.ReadWrite))
                {
                    bitmap_copy.Save(memory, ImageFormat.Png);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            /**/
        }

        public void WriteToTextFileWithArray(string filepath, string filename, byte[,] array, int width, int height)
        {
            StreamWriter writer;
            //moon
            string directory = Path.GetDirectoryName(filepath) + "\\";
            //string directory = Path.GetDirectoryName(filepath) ;
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(orignal_font_info.filepath);
            string outputFileName = directory + fileNameWithoutExtension + filename + ".txt";
            writer = File.CreateText(outputFileName);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (array[i, j] != 0)
                    {
                        int input = array[i, j];
                        string result = input.ToString().PadLeft(3, '0') + ",";
                        writer.Write(result);
                    }
                    else
                    {
                        int input = array[i, j];
                        string result = input.ToString().PadLeft(3, '0') + ",";
                        writer.Write(result);
                    }
                }
                writer.Write("\r\n");
            }
            writer.Close();
        }

        public Boolean SearchLetterConsonant(byte[,] src_array,byte[,] dst_array, int width, int height)
        {
            Boolean is_find_letter = false;

            Boolean is_find_skip_point = false;
            int skip_x  = 0, skip_y = 0;

            while (true)
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (is_find_skip_point == true)
                        {
                            if((i==skip_x)&&(j==skip_y))
                            {
                                continue;
                            }
                        }

                        if (dst_array[i, j] != 0)
                        {
                            /* Up */
                            if (i > 0)
                            {
                                if ((src_array[i - 1, j]) != 0 && (dst_array[i - 1, j] == 0))
                                {
                                    is_find_letter = true;
                                    dst_array[i - 1, j] = src_array[i - 1, j];
                                }
                            }

                            /* Down */
                            if ((i + 1 < orignal_font_info.height))
                            {
                                if ((src_array[i + 1, j] != 0) && (dst_array[i + 1, j] == 0))
                                {
                                    is_find_letter = true;
                                    dst_array[i + 1, j] = src_array[i + 1, j];
                                }
                            }

                            /* Left */
                            if (j > 0)
                            {
                                if ((src_array[i, j - 1] != 0) && (dst_array[i, j - 1] == 0))
                                {
                                    is_find_letter = true;
                                    dst_array[i, j - 1] = src_array[i, j - 1];

                                    /*
                                    if (dst_array[i, j] < src_array[i, j - 1 ])
                                    {
                                        if (dst_array[i, j] < src_array[i, j + 1])
                                        {
                                            is_find_skip_point = true;
                                            skip_x = i;
                                            skip_y = j;
                                            continue;
                                        }
                                        else
                                        {
                                            is_find_letter = true;
                                            dst_array[i, j - 1] = src_array[i, j - 1];
                                        }
                                    }
                                    */
                                }
                            }

                            /* Right */
                            if (j + 1 < orignal_font_info.width)
                            {
                                if ((src_array[i, j + 1] != 0) && (dst_array[i, j + 1] == 0))
                                {
                                    is_find_letter = true;
                                    dst_array[i, j + 1] = src_array[i, j + 1];
                                    
                                    /*
                                    if(dst_array[i,j] < src_array[i, j + 1])
                                    {
                                        if (dst_array[i, j] < src_array[i, j - 1])
                                        {
                                            is_find_skip_point = true;
                                            skip_x = i;
                                            skip_y = j;
                                            continue;
                                        }
                                        else
                                        {
                                            is_find_letter = true;
                                            dst_array[i, j + 1] = src_array[i, j + 1];
                                        }
                                    }
                                    */
                                }
                            }

                            /* Left Up */
                            if ((i > 0) && (j > 0))
                            {
                                if ((src_array[i - 1, j - 1] != 0) && (dst_array[i - 1, j - 1] == 0))
                                {
                                    //if ((src_array[i , j - 1] != 0) || (src_array[i - 1, j] != 0))
                                    //if ((src_array[i, j - 1] > 20) || (src_array[i - 1, j] > 20))
                                    {
                                        is_find_letter = true;
                                        dst_array[i - 1, j - 1] = src_array[i - 1, j - 1];
                                    }
                                }
                            }
                            /* Left Down */
                            if ((i + 1 < orignal_font_info.height) && (j > 0))
                            {
                                if ((src_array[i + 1, j - 1] != 0) && (dst_array[i + 1, j - 1] == 0))
                                {
                                    //if ((src_array[i , j - 1] != 0) || (src_array[i + 1, j] != 0))
                                    //if ((src_array[i, j - 1] > 20) || (src_array[i + 1, j] > 20))
                                    {
                                        is_find_letter = true;
                                        dst_array[i + 1, j - 1] = src_array[i + 1, j - 1];
                                    }
                                }
                            }
                            /* Right Up */
                            if ((i > 0) && (j + 1 < orignal_font_info.width))
                            {
                                if ((src_array[i - 1, j + 1] != 0) && (dst_array[i - 1, j + 1] == 0))
                                {
                                    //if ((src_array[i, j + 1] != 0) || (src_array[i - 1, j] != 0))
                                    //if ((src_array[i, j + 1] > 20) || (src_array[i - 1, j] > 20))
                                    {
                                        is_find_letter = true;
                                        dst_array[i - 1, j + 1] = src_array[i - 1, j + 1];
                                    }
                                }
                            }
                            /* Right Down */
                            if ((i + 1 < orignal_font_info.height) && (j + 1 < orignal_font_info.width))
                            {
                                if ((src_array[i + 1, j + 1] != 0) && (dst_array[i + 1, j + 1] == 0))
                                {
                                    //if ((src_array[i, j + 1] != 0) || (src_array[i + 1, j] != 0))
                                    //if ((src_array[i, j + 1] > 20) || (src_array[i + 1, j] > 20))
                                    {
                                        is_find_letter = true;
                                        dst_array[i + 1, j + 1] = src_array[i + 1, j + 1];
                                    }
                                }
                            }
                        }
                    }
                }
                if (is_find_letter == true)
                {
                    /* retry for search .. */
                    is_find_letter = false;
                }
                else
                {
                    return true;
                }
            }
        }

    }


}
