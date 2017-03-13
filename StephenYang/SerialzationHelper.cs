using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace StephenYang
{
    /// <summary>
    /// 序列化助手
    /// </summary>
    public static class SerialzationHelper
    {
        #region 二进制序列化和反序列化

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool SerialBinaryFile<T>(T t, string fileName) where T : new()
        {
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, t);
            }

            return false;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeserialBinaryFile<T>(string fileName) where T : new()
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                T t = (T)bf.Deserialize(fs);
                return t;
            }
        }

        #endregion
    }
}
