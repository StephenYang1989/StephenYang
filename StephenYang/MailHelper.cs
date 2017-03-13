using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StephenYang
{
    public class MailHelper
    {
        #region Member

        string sendName = "StephenYang1989@163.com";

        string sendPassword = "3735wade2584";

        string sendSMTPAddress = "smtp.163.com";

        int sendSMTPPort = 25;

        string receiveName = "ywmblue@163.com";

        #endregion


        #region Methods

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sendName">发送人</param>
        /// <param name="sendPassword">发送人密码</param>
        /// <param name="sendSMTPAddress">发送服务器地址</param>
        /// <param name="sendSMTPPort">发送服务器端口</param>
        public MailHelper(string sendName, string sendPassword, string sendSMTPAddress, int sendSMTPPort)
        {
            this.sendName = sendName;
            this.sendPassword = sendPassword;
            this.sendSMTPAddress = sendSMTPAddress;
            this.sendSMTPPort = sendSMTPPort;
        }


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sendReceives"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <param name="listFileAddress"></param>
        public void SendContent(List<string> sendReceives, string subject, string content, List<string> listFileAddress)
        {
            //设置消息体
            MailMessage message = new MailMessage();
            //设置发件人
            message.From = new MailAddress(sendName);
            //设置收件人
            foreach (var item in sendReceives)
            {
                //检查收件人用户名是否合法

                //添加收件人
                message.To.Add(receiveName);
            }

            //添加附件
            if (listFileAddress != null && listFileAddress.Count > 0)
            {
                Attachment att = null;
                foreach (var item in listFileAddress)
                {
                    //声明附件
                    att = new Attachment(item);
                    //添加附件
                    message.Attachments.Add(att);
                }               
            }
            //设置标题
            message.Subject = subject;


            SmtpClient client = new SmtpClient(sendSMTPAddress, sendSMTPPort);
            client.Credentials = new NetworkCredential(sendName, sendPassword);
            client.EnableSsl = false;
            client.SendCompleted += Client_SendCompleted;

            

            client.Send(message);
        }

        private void Client_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("发送完成");
        }

        #endregion
    }
}
