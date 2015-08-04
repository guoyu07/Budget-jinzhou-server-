using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class NumberToCurrency
    {
        private static string[] majorNames = { "千", "万", "亿" };
        private static string[] tenDoubleNams = { "", "一十", "二十", "三十", "四十", "五十", "六十", "七十", "八十", "九十" };
        private static string[] tenNames = { "", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九" };
        private static string[] numberNames = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };


        /// <summary>
        /// 转超过1000的数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ConvertMoreThanThousand(int number)
        {
            //越界
            if (number < 999)
            {
                throw new ArgumentException("number must be greater than 999");
            }
            string result = string.Empty;
            //处理千分位
            string strNumber = number.ToString();

            int numLen = strNumber.Length;


            int tempNumber = Convert.ToInt32(strNumber.Substring(numLen - 4, 1));   //取千位
            int lessThousand = Convert.ToInt32(strNumber.Substring(numLen - 3));    //千位之后所有位
            result = numberNames[tempNumber] + "千";
            if (lessThousand == 0)
            {
                //后面全为0
            }
            else if (lessThousand < 999)   //二位数，要补零
            {
                result += "零" + ConvertLessThanThousand(lessThousand);
            }
            else
            {
                result += ConvertLessThanThousand(lessThousand);
            }

            //千分位之上的数字，每4位作为一段进行处理
            strNumber = strNumber.Remove(numLen - 4);
            numLen = strNumber.Length;  //重新计算长度
            int index = 1;
            string tempCalculate = string.Empty;
            while (numLen > 0 && index < majorNames.Length)
            {

                tempCalculate = strNumber.Substring((numLen - 4) > 0 ? numLen - 4 : 0); //最后4位;
                if (Convert.ToInt32(tempCalculate) < 1000)
                {
                    tempCalculate = ConvertLessThanThousand(Convert.ToInt32(tempCalculate));
                }
                else
                {
                    tempCalculate = ConvertMoreThanThousand(Convert.ToInt32(tempCalculate));
                }
                result = tempCalculate + majorNames[index] + result;
                strNumber = strNumber.Remove((numLen - 4) > 0 ? numLen - 4 : 0);
                numLen = strNumber.Length;
                index++;
            }

            return result;
        }

        /// <summary>
        /// 转小于1000 (0-999) 的数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ConvertLessThanThousand(int number)
        {
            //越界
            if (number < 0 || number > 999)
            {
                throw new ArgumentException("number must between 0 and 999");
            }
            string result = string.Empty;

            if (number < 100)
            {
                return ConvertLessThanHundred(number);
            }
            else     //大于100 (100-999)
            {
                int tempNumber = number / 100;  //取百位
                result = numberNames[tempNumber] + "百";
                tempNumber = number - tempNumber * 100; //取个位与十位

                if (tempNumber == 0)    //tempNumber==0则就被整除
                {
                    //do nothing
                }
                else if (tempNumber < 10)    //存在零
                {
                    result += "零" + ConvertLessThanHundred(tempNumber);
                }
                else
                {
                    result += ConvertLessThanHundred(tempNumber);
                }
            }

            return result;
        }


        /// <summary>
        /// 转小于100 (0-99) 的数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ConvertLessThanHundred(int number)
        {
            //越界
            if (number < 0 || number > 99)
            {
                throw new ArgumentException("number must between 0 and 99");
            }

            string result = string.Empty;   //存储返回结果
            //0-10之间
            if (number < 10)
            {
                result = numberNames[number];
            }
            else    //10-99
            {
                if (number % 10 == 0)   //10的倍数
                {
                    result = tenDoubleNams[number / 10];
                }
                else     //非10的倍数
                {
                    result = tenDoubleNams[number / 10] + numberNames[number % 10]; //分别从十的倍数和数字数组中取
                }
            }

            return result;
        }


        public static string ConvertToABC(int number) 
        {
            string abc = "A、B、C、D、E、F、G、H、I、J、K、L、M、N、O、P、Q、R、S、T、U、V、W、X、Y、Z";
            string[] abclist = abc.Split('、');
            string result = abclist[number-1];
            return result;  
        }

    }
}
