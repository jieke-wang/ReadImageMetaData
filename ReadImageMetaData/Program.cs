using Shell32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Linq;

namespace ReadImageMetaData
{
    class Program
    {
        static void Main(string[] args)
        {
            #region
            //List<string> arrHeaders = new List<string>();
            //Shell32.Shell shell = new Shell32.Shell();
            //Shell32.Folder objFolder;

            //objFolder = shell.NameSpace(@"C:\Users\Public\Pictures\Car logo");

            //for (int i = 0; i < short.MaxValue; i++)
            //{
            //    string header = objFolder.GetDetailsOf(null, i);
            //    if (String.IsNullOrEmpty(header))
            //        break;
            //    arrHeaders.Add(header);
            //}

            //foreach (Shell32.FolderItem2 item in objFolder.Items())
            //{
            //    for (int i = 0; i < arrHeaders.Count; i++)
            //    {
            //        Console.WriteLine("{0}\t{1}: {2}", i, arrHeaders[i], objFolder.GetDetailsOf(item, i));
            //    }
            //}

            //Shell32.Shell shell = new Shell32.Shell();
            //shell.MinimizeAll();
            #endregion

            #region
            //要获取属性的文件路径  
            string filePath = @"D:\D\svn\BoxLet\资源\69945-image1.jpg";
            //初始化Shell接口  
            //Shell32.Shell shell = new Shell32.ShellClass();
            Shell32.Shell shell = new Shell32.Shell();
            //获取文件所在父目录对象  
            //Folder folder = shell.NameSpace(filePath.Substring(0, filePath.LastIndexOf("//")));
            Folder folder = shell.NameSpace(Path.GetDirectoryName(filePath));
            //获取文件对应的FolderItem对象  
            //FolderItem item = folder.ParseName(filePath.Substring(filePath.LastIndexOf("//") + 1));
            FolderItem item = folder.ParseName(Path.GetFileName(filePath));
            //字典存放属性名和属性值的键值关系对  
            Dictionary<string, string> Properties = new Dictionary<string, string>();
            int i = 0;
            while (true)
            {
                //获取属性名称  
                string key = folder.GetDetailsOf(null, i);
                if (string.IsNullOrEmpty(key))
                {
                    //当无属性可取时，推出循环  
                    break;
                }
                //获取属性值  
                string value = folder.GetDetailsOf(item, i);
                i++;
                //保存属性  
                if (string.IsNullOrEmpty(value))
                    continue;
                Properties.Add(key, value);
            }

            Properties = Properties.OrderBy(r => r.Key).ToDictionary(r => r.Key, r => r.Value);

            foreach (var Property in Properties)
            {
                Console.WriteLine("{0}: {1}", Property.Key, Property.Value);
            }
            #endregion
        }
    }
}

//http://www.tryexcept.com/articles/2007/10/11/reading-image-metadata-with-net.html
//http://blog.csdn.net/ioiott/article/details/7342733
//http://stackoverflow.com/questions/5708434/how-to-use-shell32-within-a-c-sharp-application
//http://stackoverflow.com/questions/220097/read-write-extended-file-properties-c/2096315#2096315
//http://stackoverflow.com/questions/16712752/read-image-file-metadatas
//http://blog.csdn.net/iamke1987/article/details/6306084
