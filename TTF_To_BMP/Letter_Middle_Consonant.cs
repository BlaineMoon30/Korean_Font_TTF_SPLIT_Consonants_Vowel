using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTF_To_BMP
{
    struct Letter_Middle
    {
        public string[] NameOfKorean;
        public string[] Unicode;

        public string[] imagePath;
        public string ExpectedStrOfSplit;
        public int[] ExpectedNumOfSplit;
        public Bitmap[] bitmap;
    }
    internal class Letter_Middle_Consonant
    {
        public const int MIDDLE_LETTER_NUM = 21;
        public const int MIDDLE_LETTER_VERSE_NUM = 4;
        public Letter_Middle[] letter_middle_db = new Letter_Middle[MIDDLE_LETTER_NUM];

        public Letter_Middle_Consonant()
        {
            Console.WriteLine("Letter_Middle_Consonant class Create");

            /* ㅏ */
            letter_middle_db[0].NameOfKorean = new string[] { "가", "마", "감", "맘" };
            letter_middle_db[0].Unicode = new string[] { "\uac00", "\ub9c8", "\uac10", "\ub9d8" };

            /* ㅐ */
            letter_middle_db[1].NameOfKorean = new string[] { "개", "매", "갬", "맴" };
            letter_middle_db[1].Unicode = new string[] { "\uac1c", "\ub9e4", "\uac2c", "\ub9f4" };

            /* ㅑ */
            letter_middle_db[2].NameOfKorean = new string[] { "갸", "먀", "걈", "먐" };
            letter_middle_db[2].Unicode = new string[] { "\uac38", "\uba00", "\uac48", "\uba10" };

            /* ㅒ */
            letter_middle_db[3].NameOfKorean = new string[] { "걔", "먜", "걤", "먬" };
            letter_middle_db[3].Unicode = new string[] { "\uac54", "\uba1c", "\uac64", "\uba2c" };

            /* ㅓ */
            letter_middle_db[4].NameOfKorean = new string[] { "거", "머", "검", "멈" };
            letter_middle_db[4].Unicode = new string[] { "\uac70", "\uba38", "\uac80", "\uba48" };

            /* ㅔ */
            letter_middle_db[5].NameOfKorean = new string[] { "게", "메", "겜", "멤" };
            letter_middle_db[5].Unicode = new string[] { "\uac8c", "\uba54", "\uac9c", "\uba64" };

            /* ㅕ */
            letter_middle_db[6].NameOfKorean = new string[] { "겨", "며", "겸", "몀" };
            letter_middle_db[6].Unicode = new string[] { "\uaca8", "\uba70", "\uacb8", "\uba80" };

            /* ㅖ */
            letter_middle_db[7].NameOfKorean = new string[] { "계", "몌", "곔", "몜" };
            letter_middle_db[7].Unicode = new string[] { "\uacc4", "\uba8c", "\uacd4", "\uba9c" };

            /* ㅗ */
            letter_middle_db[8].NameOfKorean = new string[] { "고", "모", "곰", "몸" };
            letter_middle_db[8].Unicode = new string[] { "\uace0", "\ubaa8", "\uacf0", "\ubab8" };

            /* ㅘ */
            letter_middle_db[9].NameOfKorean = new string[] { "과", "뫄", "괌", "뫔" };
            letter_middle_db[9].Unicode = new string[] { "\uacfc", "\ubac4", "\uad0c", "\ubad4" };

            /* ㅙ */
            letter_middle_db[10].NameOfKorean = new string[] { "괘", "뫠", "괨", "뫰" };
            letter_middle_db[10].Unicode = new string[] { "\uad18", "\ubae0", "\uad28", "\ubaf0" };

            /* ㅚ */
            letter_middle_db[11].NameOfKorean = new string[] { "괴", "뫼", "굄", "묌" };
            letter_middle_db[11].Unicode = new string[] { "\uad34", "\ubafc", "\uad44", "\ubb0c" };

            /* ㅛ */
            letter_middle_db[12].NameOfKorean = new string[] { "교", "묘", "굠", "묨" };
            letter_middle_db[12].Unicode = new string[] { "\uad50", "\ubb18", "\uad60", "\ubb28" };

            /* ㅜ */
            letter_middle_db[13].NameOfKorean = new string[] { "구", "무", "굼", "뭄" };
            letter_middle_db[13].Unicode = new string[] { "\uad6c", "\ubb34", "\uad7c", "\ubb44" };

            /* ㅝ */
            letter_middle_db[14].NameOfKorean = new string[] { "궈", "뭐", "궘", "뭠" };
            letter_middle_db[14].Unicode = new string[] { "\uad88", "\ubb50", "\uad98", "\ubb60" };

            /* ㅞ */
            letter_middle_db[15].NameOfKorean = new string[] { "궤", "뭬", "궴", "뭼" };
            letter_middle_db[15].Unicode = new string[] { "\uada4", "\ubb6c", "\uadb4", "\ubb7c" };

            /* ㅟ */
            letter_middle_db[16].NameOfKorean = new string[] { "귀", "뮈", "귐", "뮘" };
            letter_middle_db[16].Unicode = new string[] { "\uadc0", "\ubb88", "\uadd0", "\ubb98" };

            /* ㅠ */
            letter_middle_db[17].NameOfKorean = new string[] { "규", "뮤", "귬", "뮴" };
            letter_middle_db[17].Unicode = new string[] { "\uaddc", "\ubba4", "\uadec", "\ubbb4" };

            /* ㅡ */
            letter_middle_db[18].NameOfKorean = new string[] { "그", "므", "금", "믐" };
            letter_middle_db[18].Unicode = new string[] { "\uadf8", "\ubbc0", "\uae08", "\ubbd0" };

            /* ㅢ */
            letter_middle_db[19].NameOfKorean = new string[] { "긔", "믜", "긤", "믬" };
            letter_middle_db[19].Unicode = new string[] { "\uae14", "\ubbdc", "\uae24", "\ubbec" };

            /* ㅣ */
            letter_middle_db[20].NameOfKorean = new string[] { "기", "미", "김", "밈" };
            letter_middle_db[20].Unicode = new string[] { "\uae30", "\ubbf8", "\uae40", "\ubc08" };

            for (int i = 0; i < MIDDLE_LETTER_NUM; i++)
            {
                letter_middle_db[i].imagePath = new string[MIDDLE_LETTER_VERSE_NUM];
            }

        }

    }


}
