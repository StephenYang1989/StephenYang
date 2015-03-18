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
    public static class MsgBox
    {
        /// <summary>
        /// 无消息提示
        /// </summary>
        /// <returns></returns>
        public static DialogResult Show()
        {
            return MessageBox.Show("");
        }

        /// <summary>
        /// 无标题消息提示
        /// </summary>
        /// <param name="str">提示内容</param>
        /// <returns></returns>
        public static DialogResult Show(string str)
        {
            return MessageBox.Show(str);
        }

        /// <summary>
        /// 有标题消息提示
        /// </summary>
        /// <param name="i">提示类型:1.正常提示;2.警告提示;3.错误提示;无标题提示</param>
        /// <param name="str">提示内容</param>
        /// <returns></returns>
        public static DialogResult Show(int i, string str)
        {
            if (i == 1)
            {
                //↓正常消息提示
                return MessageBox.Show(str, MessageCaption.Notice, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (i == 2)
            {
                //↓警告提示
                return MessageBox.Show(str, MessageCaption.Danger, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (i == 3)
            {
                //↓错误提示
                return MessageBox.Show(str, MessageCaption.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //↓无标题提示
                return MessageBox.Show(str, MessageCaption.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 消息提示，包含“确定”和“取消"
        /// </summary>
        /// <param name="i">提示类型:1.正常提示;2.警告提示;2.错误提示;无标题提示</param>
        /// <param name="str">提示内容</param>
        /// <returns></returns>
        public static DialogResult ShowOKCancel(int i, string str)
        {
            if (i == 1)
            {
                //↓正常消息提示
                return MessageBox.Show(str, MessageCaption.Notice, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
            else if (i == 2)
            {
                //↓警告提示
                return MessageBox.Show(str, MessageCaption.Danger, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            else if (i == 3)
            {
                //↓错误提示
                return MessageBox.Show(str, MessageCaption.Error, MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            else
            {
                //↓无标题提示
                return MessageBox.Show(str, MessageCaption.Empty, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }


        /// <summary>
        /// 消息提示，包含“是”和“否"
        /// </summary>
        /// <param name="i">提示类型:1.正常提示;2.警告提示;2.错误提示;无标题提示</param>
        /// <param name="str">提示内容</param>
        /// <returns></returns>
        public static DialogResult ShowYesNo(int i, string str)
        {
            if (i == 1)
            {
                //↓正常消息提示
                return MessageBox.Show(str, MessageCaption.Notice, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            }
            else if (i == 2)
            {
                //↓警告提示
                return MessageBox.Show(str, MessageCaption.Danger, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }
            else if (i == 3)
            {
                //↓错误提示
                return MessageBox.Show(str, MessageCaption.Error, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }
            else
            {
                //↓无标题提示
                return MessageBox.Show(str, MessageCaption.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 消息提示，包含“是”和“否"，并判断是否要包含”取消”
        /// </summary>
        /// <param name="i">提示类型:1.正常提示;2.警告提示;2.错误提示;无标题提示</param>
        /// <param name="str">提示内容</param>
        /// <param name="bl">是否包含"取消"</param>
        /// <returns></returns>
        public static DialogResult ShowYesNo(int i, string str, bool bl)
        {
            if (i == 1)
            {
                //↓正常消息提示
                if (bl)
                {
                    return MessageBox.Show(str, MessageCaption.Notice, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                }
                else
                {
                    return MessageBox.Show(str, MessageCaption.Notice, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                }
            }
            else if (i == 2)
            {
                //↓警告提示
                if (bl)
                {
                    return MessageBox.Show(str, MessageCaption.Danger, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                }
                else
                {
                    return MessageBox.Show(str, MessageCaption.Danger, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                }
            }
            else if (i == 3)
            {
                //↓错误提示
                if (bl)
                {
                    return MessageBox.Show(str, MessageCaption.Error, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
                }
                else
                {
                    return MessageBox.Show(str, MessageCaption.Error, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                }
            }
            else
            {
                //↓无标题提示
                if (bl)
                {
                    return MessageBox.Show(str, MessageCaption.Empty, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                }
                else
                {
                    return MessageBox.Show(str, MessageCaption.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                }
            }
        }
    }

    /// <summary>
    /// 消息类型枚举
    /// </summary>
    internal static class MessageCaption
    {
        public static string Empty = "";
        public static string Notice = "提示";
        public static string Danger = "危险";
        //public static string Danger = "提示";
        public static string Error = "错误";
    }
}
