using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTF_To_BMP
{
    struct Letter_First
    {
        public string[] NameOfKorean;
        public string[] Unicode;

        public string[] imagePath;
        public string ExpectedStrOfSplit;
        public int[] ExpectedNumOfSplit;
        public Bitmap[] bitmap;
    }
    internal class Letter_First_Consonant
    {
        public const int FIRST_LETTER_NUM = 19;
        public const int FIRST_LETTER_VERSE_NUM = 8;
        public Letter_First[] letter_first_db = new Letter_First[FIRST_LETTER_NUM];


        public Letter_First_Consonant()
        {
            Console.WriteLine("Letter_First_Consonant class Create");

            /* ㄱ */
            letter_first_db[0].NameOfKorean = new string[] { "개", "고", "구", "괘", "궤", "갱", "공", "괸" };
            letter_first_db[0].Unicode = new string[] { "\uac1c","\uace0","\uad6c","\uad18","\uada4","\uac31","\uacf5","\uad38" };
            letter_first_db[0].ExpectedStrOfSplit = "ㄱ";

            /* ㄲ */
            letter_first_db[1].NameOfKorean = new string[] { "깨", "꼬", "꾸", "꽤", "꿰", "깽", "꽁", "꾄" };
            letter_first_db[1].Unicode = new string[] { "\uae68", "\uaf2c", "\uafb8", "\uaf64", "\uaff0", "\uae7d", "\uaf41", "\uaf84" };
            letter_first_db[1].ExpectedStrOfSplit = "ㄲ";

            /* ㄴ */
            letter_first_db[2].NameOfKorean = new string[] { "내", "노", "누", "놰", "눼", "냉", "농", "뇐" };
            letter_first_db[2].Unicode = new string[] { "\ub0b4", "\ub178", "\ub204", "\ub1b0", "\ub23c", "\ub0c9", "\ub18d", "\ub1d0" };
            letter_first_db[2].ExpectedStrOfSplit = "ㄴ";

            /* ㄷ */
            letter_first_db[3].NameOfKorean = new string[] { "대", "도", "두", "돼", "뒈", "댕", "동", "된" };
            letter_first_db[3].Unicode = new string[] { "\ub300", "\ub3c4", "\ub450", "\ub3fc", "\ub488", "\ub315", "\ub3d9", "\ub41c" };
            letter_first_db[3].ExpectedStrOfSplit = "ㄷ";

            /* ㄸ */
            letter_first_db[4].NameOfKorean = new string[] { "때", "또", "뚜", "뙈", "뛔", "땡", "똥", "뙨" };
            letter_first_db[4].Unicode = new string[] { "\ub54c", "\ub610", "\ub69c", "\ub648", "\ub6d4", "\ub561", "\ub625", "\ub668" };
            letter_first_db[4].ExpectedStrOfSplit = "ㄸ";

            /* ㄹ */
            letter_first_db[5].NameOfKorean = new string[] { "래", "로", "루", "뢔", "뤠", "랭", "롱", "뢴" };
            letter_first_db[5].Unicode = new string[] { "\ub798", "\ub85c", "\ub8e8", "\ub894", "\ub920", "\ub7ad", "\ub871", "\ub8b4" };
            letter_first_db[5].ExpectedStrOfSplit = "ㄹ";

            /* ㅁ */
            letter_first_db[6].NameOfKorean = new string[] { "매", "모", "무", "뫠", "뭬", "맹", "몽", "묀" };
            letter_first_db[6].Unicode = new string[] { "\ub9e4", "\ubaa8", "\ubb34", "\ubae0", "\ubb6c", "\ub9f9", "\ubabd", "\ubb00" };
            letter_first_db[6].ExpectedStrOfSplit = "ㅁ";

            /* ㅂ */
            letter_first_db[7].NameOfKorean = new string[] { "배", "보", "부", "봬", "붸", "뱅", "봉", "뵌" };
            letter_first_db[7].Unicode = new string[] { "\ubc30", "\ubcf4", "\ubd80", "\ubd2c", "\ubdb8", "\ubc45", "\ubd09", "\ubd4c" };
            letter_first_db[7].ExpectedStrOfSplit = "ㅂ";

            /* ㅃ */
            letter_first_db[8].NameOfKorean = new string[] { "빼", "뽀", "뿌", "뽸", "쀄", "뺑", "뽕", "뾘" };
            letter_first_db[8].Unicode = new string[] { "\ube7c", "\ubf40", "\ubfcc", "\ubf78", "\uc004", "\ube91", "\ubf55", "\ubf98" };
            letter_first_db[8].ExpectedStrOfSplit = "ㅃ";

            /* ㅅ */
            letter_first_db[9].NameOfKorean = new string[] { "새", "소", "수", "쇄", "쉐", "생", "송", "쇤" };
            letter_first_db[9].Unicode = new string[] { "\uc0c8", "\uc18c", "\uc218", "\uc1c4", "\uc250", "\uc0dd", "\uc1a1", "\uc1e4" };
            letter_first_db[9].ExpectedStrOfSplit = "ㅅ";

            /* ㅆ */
            letter_first_db[10].NameOfKorean = new string[] { "쌔", "쏘", "쑤", "쐐", "쒜", "쌩", "쏭", "쐰" };
            letter_first_db[10].Unicode = new string[] { "\uc314", "\uc3d8", "\uc464", "\uc410", "\uc49c", "\uc329", "\uc3ed", "\uc430" };
            letter_first_db[10].ExpectedStrOfSplit = "ㅆ";

            /* ㅇ */
            letter_first_db[11].NameOfKorean = new string[] { "애", "오", "우", "왜", "웨", "앵", "옹", "왼" };
            letter_first_db[11].Unicode = new string[] { "\uc560", "\uc624", "\uc6b0", "\uc65c", "\uc6e8", "\uc575", "\uc639", "\uc67c" };
            letter_first_db[11].ExpectedStrOfSplit = "ㅇ";

            /* ㅈ */
            letter_first_db[12].NameOfKorean = new string[] { "재", "조", "주", "좨", "줴", "쟁", "종", "죈" };
            letter_first_db[12].Unicode = new string[] { "\uc7ac", "\uc870", "\uc8fc", "\uc8a8", "\uc934", "\uc7c1", "\uc885", "\uc8c8" };
            letter_first_db[12].ExpectedStrOfSplit = "ㅈ";

            /* ㅉ */
            letter_first_db[13].NameOfKorean = new string[] { "째", "쪼", "쭈", "쫴", "쮀", "쨍", "쫑", "쬔" };
            letter_first_db[13].Unicode = new string[] { "\uc9f8", "\ucabc", "\ucb48", "\ucaf4", "\ucb80", "\uca0d", "\ucad1", "\ucb14" };
            letter_first_db[13].ExpectedStrOfSplit = "ㅉ";

            /* ㅊ */
            letter_first_db[14].NameOfKorean = new string[] { "채", "초", "추", "쵀", "췌", "챙", "총", "쵠" };
            letter_first_db[14].Unicode = new string[] { "\ucc44", "\ucd08", "\ucd94", "\ucd40", "\ucdcc", "\ucc59", "\ucd1d", "\ucd60" };
            letter_first_db[14].ExpectedStrOfSplit = "ㅊ";

            /* ㅋ */
            letter_first_db[15].NameOfKorean = new string[] { "캐", "코", "쿠", "쾌", "퀘", "캥", "콩", "쾬" };
            letter_first_db[15].Unicode = new string[] { "\uce90", "\ucf54", "\ucfe0", "\ucf8c", "\ud018", "\ucea5", "\ucf69", "\ucfac" };
            letter_first_db[15].ExpectedStrOfSplit = "ㅋ";

            /* ㅌ */
            letter_first_db[16].NameOfKorean = new string[] { "태", "토", "투", "퇘", "퉤", "탱", "통", "퇸" };
            letter_first_db[16].Unicode = new string[] { "\ud0dc", "\ud1a0", "\ud22c", "\ud1d8", "\ud264", "\ud0f1", "\ud1b5", "\ud1f8" };
            letter_first_db[16].ExpectedStrOfSplit = "ㅌ";

            /* ㅍ */
            letter_first_db[17].NameOfKorean = new string[] { "패", "포", "푸", "퐤", "풰", "팽", "퐁", "푄" };
            letter_first_db[17].Unicode = new string[] { "\ud328", "\ud3ec", "\ud478", "\ud424", "\ud4b0", "\ud33d", "\ud401", "\ud444" };
            letter_first_db[17].ExpectedStrOfSplit = "ㅍ";

            /* ㅎ */
            letter_first_db[18].NameOfKorean = new string[] { "해", "호", "후", "홰", "훼", "행", "홍", "횐" };
            letter_first_db[18].Unicode = new string[] { "\ud574", "\ud638", "\ud6c4", "\ud670", "\ud6fc", "\ud589", "\ud64d", "\ud690" };
            letter_first_db[18].ExpectedStrOfSplit = "ㅎ";

            for(int i=0; i< FIRST_LETTER_NUM; i++)
            {
                letter_first_db[i].imagePath = new string[FIRST_LETTER_VERSE_NUM];
            }

        }

    }
}
