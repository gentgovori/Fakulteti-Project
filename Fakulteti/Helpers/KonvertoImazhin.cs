using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Fakulteti.Helpers
{
    public sealed class KonvertoImazhin
    {
        public static byte[] KonvertoImageNeByteArray(System.Drawing.Image img, System.Drawing.Imaging.ImageFormat Formati)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, Formati);
            return ms.ToArray();
        }

        public static Image ByteArrayNeImage(byte[] byteArray)
        {
            MemoryStream ms = new MemoryStream(byteArray);
            Image img = Image.FromStream(ms);
            return img;
        }
    }

}


