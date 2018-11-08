using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;

namespace ClownFish.KitLib
{
    /// <summary>
    /// Windows自启动的工具类
    /// </summary>
    public static class AutoRunManager
    {
        private static readonly string s_AutoRunRegPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";


        /// <summary>
        /// 设置程序的自启动（随Windows启动）
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="enabled"></param>
        public static void SetAutoRun(string filePath, bool enabled)
        {
            if( string.IsNullOrEmpty(filePath) )
                throw new ArgumentNullException("filePath");

            if( File.Exists(filePath) == false )
                throw new FileNotFoundException(string.Format("文件 {0} 不存在，无法完成设置自启动的操作。", filePath));

            RegistryKey key = null;

            try {
                key = Registry.LocalMachine.OpenSubKey(s_AutoRunRegPath, true);
                if( key == null )
                    key = Registry.LocalMachine.CreateSubKey(s_AutoRunRegPath);

                if( key != null ) {
                    string keyName = System.IO.Path.GetFileNameWithoutExtension(filePath);

                    if( enabled ) {
                        string current = key.GetValue(keyName, string.Empty).ToString();
                        if( current != filePath )
                            key.SetValue(keyName, string.Format("\"{0}\"", filePath));
                    }
                    else
                        key.DeleteValue(keyName, false);
                }
            }
            //catch( Exception ex ) {
            //	System.Windows.Forms.MessageBox.Show("设置自启动失败，原因：" + ex.Message, MainForm.DialogTitle,
            //		System.Windows.Forms.MessageBoxButtons.OK,
            //		System.Windows.Forms.MessageBoxIcon.Warning);
            //}
            finally {
                if( key != null )
                    key.Close();
            }
        }

        /// <summary>
        /// 判断程序有没有注册自启动
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool CheckIsNotSet(string filePath)
        {
            if( string.IsNullOrEmpty(filePath) )
                throw new ArgumentNullException("filePath");

            using( RegistryKey key = Registry.LocalMachine.OpenSubKey(s_AutoRunRegPath, false) ) {
                if( key != null ) {
                    string keyName = System.IO.Path.GetFileNameWithoutExtension(filePath);

                    string current = key.GetValue(keyName, string.Empty).ToString();
                    return string.IsNullOrEmpty(current);
                }
            }

            return false;
        }


    }
}
