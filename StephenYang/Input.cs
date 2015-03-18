using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;

namespace StephenYang
{
    /// <summary>
    /// 输入和控件值相关
    /// </summary>
    public static class Input
    {
        #region Member

        static int keyPressCount = 0;      //←定义某按键计数

        #endregion


        /// <summary>
        /// 只允许输入框的值为数字或者小数点
        /// </summary>
        /// <param name="e"></param>
        /// <param name="text"></param>
        public static void KeyPressIsNumberFloat(KeyPressEventArgs e, TextBox text)
        {
            int i = text.Text.Trim().IndexOf('.');	//←查询是否输入小数点
            if (i >= 0)
            {
                keyPressCount = 1;
            }
            else
            {
                keyPressCount = 0;
            }
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (byte)(e.KeyChar) == 8 || e.KeyChar == '.')
            {
                if (e.KeyChar == '.')
                {
                    if (keyPressCount == 0)
                    {
                        keyPressCount++;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
                else
                {

                }
            }
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 只允许输入框的值为数字或者小数点或负号
        /// </summary>
        /// <param name="e"></param>
        /// <param name="text"></param>
        public static void KeyPressIsNumberFloatNegative(KeyPressEventArgs e, TextBox text)
        {
            int i = text.Text.Trim().IndexOf('.');	//←查询是否输入小数点
            //int n = text.Text.Trim().IndexOf('-');  //←查询符号的输入位置
            if (i >= 0)
            {
                keyPressCount = 1;
            }
            else
            {
                keyPressCount = 0;
            }

            //if (!ControlIsNull(text))
            //{
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (byte)(e.KeyChar) == 8 || e.KeyChar == '.' || e.KeyChar == '-')
            {
                if (e.KeyChar == '.')
                {
                    if (keyPressCount == 0)
                    {
                        keyPressCount++;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
                else if (e.KeyChar == '-')
                {
                    if (!string.IsNullOrEmpty(text.Text.Trim()))
                    {
                        e.Handled = true;
                    }
                }
            }
            else
            {
                e.Handled = true;
            }
            //}
        }


        /// <summary>
        /// 只允许输入框的值为数字
        /// </summary>
        /// <param name="e"></param>
        /// <param name="text"></param>
        public static void KeyPressIsNumberInt(KeyPressEventArgs e, TextBox text)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (byte)(e.KeyChar) == 8)
            {

            }
            else
            {
                e.Handled = true;
            }
        }


        /// <summary>
        /// 只允许输入框的值为数字
        /// </summary>
        /// <param name="e"></param>
        /// <param name="text"></param>
        public static void KeyPressIsNumberIntNegative(KeyPressEventArgs e, TextBox text)
        {
            if (!ControlIsNull(text))
            {
                if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (byte)(e.KeyChar) == 8 || e.KeyChar == '-')
                {
                    if (e.KeyChar == '-')
                    {
                        if (!string.IsNullOrEmpty(text.Text.Trim()))
                        {
                            e.Handled = true;
                        }
                    }
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// 只允许输入框的值为数字
        /// </summary>
        /// <param name="e"></param>
        /// <param name="text"></param>
        public static void KeyPressIsNumberInt(KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (byte)(e.KeyChar) == 8)
            {

            }
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 只允许输入框的值为数字或者大小写字母
        /// </summary>
        /// <param name="e"></param>
        public static void KeyPressIsNumAndWords(KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (byte)(e.KeyChar) == 8 || (e.KeyChar > 'A' && e.KeyChar < 'z'))
            {

            }
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 判断空间里面的值是否为空
        /// </summary>
        /// <param name="listControl"></param>
        /// <returns></returns>
        public static bool ControlIsNull(Control control)
        {
            bool result;
            if (string.IsNullOrEmpty(control.Text))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }


        /// <summary>
        /// 全角转换成半角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                {
                    c[i] = (char)(c[i] - 65248);
                }
            }
            return new string(c);
        }


        /// <summary>
        /// 半角转换成全角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                {
                    c[i] = (char)(c[i] + 65248);
                }
            }
            return new string(c);
        }
    }
}
