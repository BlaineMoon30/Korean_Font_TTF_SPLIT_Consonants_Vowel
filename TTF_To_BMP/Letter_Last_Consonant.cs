using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTF_To_BMP
{
    struct Letter_Last
    {
        public string[] NameOfKorean;
        public string[] Unicode;

        public string[] imagePath;
        public string ExpectedStrOfSplit;
        public int[] ExpectedNumOfSplit;
        public Bitmap[] bitmap;
    }
    internal class Letter_Last_Consonant
    {
        public const int LAST_LETTER_NUM = 27;
        public const int LAST_LETTER_VERSE_NUM = 4;
        public Letter_Last[] letter_last_db = new Letter_Last[LAST_LETTER_NUM];

        public Letter_Last_Consonant()
        {
            Console.WriteLine("Letter_Last_Consonant class Create");

            /* ㄱ */
            letter_last_db[0].NameOfKorean = new string[] { "각", "걱", "객", "곡" };
            letter_last_db[0].Unicode = new string[] { "\uac01", "\uac71", "\uac1d", "\uace1" };

            /* ㄲ */
            letter_last_db[1].NameOfKorean = new string[] { "갂", "걲", "갞", "곢" };
            letter_last_db[1].Unicode = new string[] { "\uac02", "\uac72", "\uac1e", "\uace2" };

            /* ㄳ */
            letter_last_db[2].NameOfKorean = new string[] { "갃", "걳", "갟", "곣" };
            letter_last_db[2].Unicode = new string[] { "\uac03", "\uac73", "\uac1f", "\uace3" };

            /* ㄴ */
            letter_last_db[3].NameOfKorean = new string[] { "간", "건", "갠", "곤" };
            letter_last_db[3].Unicode = new string[] { "\uac04", "\uac74", "\uac20", "\uace4" };

            /* ㄵ */
            letter_last_db[4].NameOfKorean = new string[] { "갅", "걵", "갡", "곥" };
            letter_last_db[4].Unicode = new string[] { "\uac05", "\uac75", "\uac21", "\uace5" };

            /* ㄶ */
            letter_last_db[5].NameOfKorean = new string[] { "갆", "걶", "갢", "곦" };
            letter_last_db[5].Unicode = new string[] { "\uac06", "\uac76", "\uac22", "\uace6" };

            /* ㄷ */
            letter_last_db[6].NameOfKorean = new string[] { "갇", "걷", "갣", "곧" };
            letter_last_db[6].Unicode = new string[] { "\uac07", "\uac77", "\uac23", "\uace7" };

            /* ㄹ */
            letter_last_db[7].NameOfKorean = new string[] { "갈", "걸", "갤", "골" };
            letter_last_db[7].Unicode = new string[] { "\uac08", "\uac78", "\uac24", "\uace8" };

            /* ㄺ */
            letter_last_db[8].NameOfKorean = new string[] { "갉", "걹", "갥", "곩" };
            letter_last_db[8].Unicode = new string[] { "\uac09", "\uac79", "\uac25", "\uace9" };

            /* ㄻ */
            letter_last_db[9].NameOfKorean = new string[] { "갊", "걺", "갦", "곪" };
            letter_last_db[9].Unicode = new string[] { "\uac0a", "\uac7a", "\uac26", "\uacea" };

            /* ㄼ */
            letter_last_db[10].NameOfKorean = new string[] { "갋", "걻", "갧", "곫" };
            letter_last_db[10].Unicode = new string[] { "\uac0b", "\uac7b", "\uac27", "\uaceb" };

            /* ㄽ */
            letter_last_db[11].NameOfKorean = new string[] { "갌", "걼", "갨", "곬" };
            letter_last_db[11].Unicode = new string[] { "\uac0c", "\uac7c", "\uac28", "\uacec" };

            /* ㄾ */
            letter_last_db[12].NameOfKorean = new string[] { "갍", "걽", "갩", "곭" };
            letter_last_db[12].Unicode = new string[] { "\uac0d", "\uac7d", "\uac29", "\uaced" };

            /* ㄿ */
            letter_last_db[13].NameOfKorean = new string[] { "갎", "걾", "갪", "곮" };
            letter_last_db[13].Unicode = new string[] { "\uac0e", "\uac7e", "\uac2a", "\uacee" };

            /* ㅀ */
            letter_last_db[14].NameOfKorean = new string[] { "갏", "걿", "갫", "곯" };
            letter_last_db[14].Unicode = new string[] { "\uac0f", "\uac7f", "\uac2b", "\uacef" };

            /* ㅁ */
            letter_last_db[15].NameOfKorean = new string[] { "감", "검", "갬", "곰" };
            letter_last_db[15].Unicode = new string[] { "\uac10", "\uac80", "\uac2c", "\uacf0" };

            /* ㅂ */
            letter_last_db[16].NameOfKorean = new string[] { "갑", "겁", "갭", "곱" };
            letter_last_db[16].Unicode = new string[] { "\uac11", "\uac81", "\uac2d", "\uacf1" };

            /* ㅄ */
            letter_last_db[17].NameOfKorean = new string[] { "값", "겂", "갮", "곲" };
            letter_last_db[17].Unicode = new string[] { "\uac12", "\uac82", "\uac2e", "\uacf2" };

            /* ㅅ */
            letter_last_db[18].NameOfKorean = new string[] { "갓", "것", "갯", "곳" };
            letter_last_db[18].Unicode = new string[] { "\uac13", "\uac83", "\uac2f", "\uacf3" };

            /* ㅆ */
            letter_last_db[19].NameOfKorean = new string[] { "갔", "겄", "갰", "곴" };
            letter_last_db[19].Unicode = new string[] { "\uac14", "\uac84", "\uac30", "\uacf4" };

            /* ㅇ */
            letter_last_db[20].NameOfKorean = new string[] { "강", "겅", "갱", "공" };
            letter_last_db[20].Unicode = new string[] { "\uac15", "\uac85", "\uac31", "\uacf5" };

            /* ㅈ */
            letter_last_db[21].NameOfKorean = new string[] { "갖", "겆", "갲", "곶" };
            letter_last_db[21].Unicode = new string[] { "\uac16", "\uac86", "\uac32", "\uacf6" };

            /* ㅊ */
            letter_last_db[22].NameOfKorean = new string[] { "갗", "겇", "갳", "곷" };
            letter_last_db[22].Unicode = new string[] { "\uac17", "\uac87", "\uac33", "\uacf7" };

            /* ㅋ */
            letter_last_db[23].NameOfKorean = new string[] { "갘", "겈", "갴", "곸" };
            letter_last_db[23].Unicode = new string[] { "\uac18", "\uac88", "\uac34", "\uacf8" };

            /* ㅌ */
            letter_last_db[24].NameOfKorean = new string[] { "같", "겉", "갵", "곹" };
            letter_last_db[24].Unicode = new string[] { "\uac19", "\uac89", "\uac35", "\uacf9" };

            /* ㅍ */
            letter_last_db[25].NameOfKorean = new string[] { "갚", "겊", "갶", "곺" };
            letter_last_db[25].Unicode = new string[] { "\uac1a", "\uac8a", "\uac36", "\uacfa" };

            /* ㅎ */
            letter_last_db[26].NameOfKorean = new string[] { "갛", "겋", "갷", "곻" };
            letter_last_db[26].Unicode = new string[] { "\uac1b", "\uac8b", "\uac37", "\uacfb" };

            for (int i = 0; i < LAST_LETTER_NUM; i++)
            {
                letter_last_db[i].imagePath = new string[LAST_LETTER_VERSE_NUM];
            }
        }
    }
}
