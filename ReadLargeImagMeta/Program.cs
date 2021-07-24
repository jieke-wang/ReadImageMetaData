using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ReadLargeImagMeta.Classes;

namespace ReadLargeImagMeta
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "0836bbca-38cc-46eb-8451-07c010b740d4.JPG";

            #region
            //Image img = Image.FromFile(fileName);
            //ImageFormat format = img.RawFormat;
            //Console.WriteLine("Image Type : " + format.ToString());
            //Console.WriteLine("Image width : " + img.Width);
            //Console.WriteLine("Image height : " + img.Height);
            //Console.WriteLine("Image resolution : " + (img.VerticalResolution * img.HorizontalResolution));

            //Console.WriteLine("Image Pixel depth : " + Image.GetPixelFormatSize(img.PixelFormat));
            //Console.WriteLine("Image Creation Date : " + File.GetCreationTime(fileName).ToString("yyyy-MM-dd"));
            //Console.WriteLine("Image Creation Time : " + File.GetCreationTime(fileName).ToString("hh:mm:ss"));
            //Console.WriteLine("Image Modification Date : " + File.GetLastWriteTime(fileName).ToString("yyyy-MM-dd"));
            //Console.WriteLine("Image Modification Time : " + File.GetLastWriteTime(fileName).ToString("hh:mm:ss"));
            #endregion

            #region
            //Size size = GetJpegImageSize(fileName);
            #endregion

            #region
            //EXIFextractor er = new EXIFextractor(fileName, "", "");
            //foreach (System.Web.UI.Pair s in er)
            //{
            //    Console.WriteLine(s.First + " : " + s.Second);
            //}
            //Console.WriteLine(er["User Comment"]);

            //PropertyItem[] arrayOfProperty = EXIFextractor.GetExifProperties(fileName);
            //foreach (var property in arrayOfProperty)
            //{
            //    Console.WriteLine("{0}: {1}", property.Type, string.Join(",", property.Value));
            //}

            EXIFextractor er = new EXIFextractor(fileName, "", "");
            Console.WriteLine("image file[{0}]\n\twidth:{1}\n\theight{2}", Path.GetFileName(fileName), er["PixXDim"], er["PixYDim"]);
            #endregion
        }

        public static Size GetJpegImageSize(string filename)
        {
            FileStream stream = null;
            BinaryReader rdr = null;
            try
            {
                stream = File.OpenRead(filename);
                rdr = new BinaryReader(stream);
                // keep reading packets until we find one that contains Size info
                for (; ; )
                {
                    byte code = rdr.ReadByte();
                    if (code != 0xFF) throw new ApplicationException(
                               "Unexpected value in file " + filename);
                    code = rdr.ReadByte();
                    switch (code)
                    {
                        // filler byte
                        case 0xFF:
                            stream.Position--;
                            break;
                        // packets without data
                        case 0xD0:
                        case 0xD1:
                        case 0xD2:
                        case 0xD3:
                        case 0xD4:
                        case 0xD5:
                        case 0xD6:
                        case 0xD7:
                        case 0xD8:
                        case 0xD9:
                            break;
                        // packets with size information
                        case 0xC0:
                        case 0xC1:
                        case 0xC2:
                        case 0xC3:
                        case 0xC4:
                        case 0xC5:
                        case 0xC6:
                        case 0xC7:
                        case 0xC8:
                        case 0xC9:
                        case 0xCA:
                        case 0xCB:
                        case 0xCC:
                        case 0xCD:
                        case 0xCE:
                        case 0xCF:
                            ReadBEUshort(rdr);
                            rdr.ReadByte();
                            ushort h = ReadBEUshort(rdr);
                            ushort w = ReadBEUshort(rdr);
                            return new Size(w, h);
                        // irrelevant variable-length packets
                        default:
                            int len = ReadBEUshort(rdr);
                            stream.Position += len - 2;
                            break;
                    }
                }
            }
            finally
            {
                if (rdr != null) rdr.Close();
                if (stream != null) stream.Close();
            }
        }

        private static ushort ReadBEUshort(BinaryReader rdr)
        {
            ushort hi = rdr.ReadByte();
            hi <<= 8;
            ushort lo = rdr.ReadByte();
            return (ushort)(hi | lo);
        }
    }
}

//http://www.codeproject.com/Articles/11305/EXIFextractor-library-to-extract-EXIF-information api
//http://stackoverflow.com/questions/552467/how-do-i-reliably-get-an-image-dimensions-in-net-without-loading-the-image
