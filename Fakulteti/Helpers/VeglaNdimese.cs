using GenCode128;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace Fakulteti.Helpers
{
    public class VeglaNdimese
    { // gjenero imagjin per UNIREF. 
        public byte[] GjeneroImageUNIREF(string UNIREF)
        {
            System.Drawing.Image imgUNIREF = null;
            byte[] bUNIREF = null;

            imgUNIREF = Code128Rendering.MakeBarcodeImage(UNIREF, 2, true);
            bUNIREF = KonvertoImazhin.KonvertoImageNeByteArray(imgUNIREF, ImageFormat.Jpeg);

            return bUNIREF;
        }
    }
    public class MbeshtjellesiUnirefit
    {
        public byte[] ImazhiUNIREF { get; set; }
    }
    public class UnirefWrapper
    {
        public byte[] Unirefi { get; set; }
    }
}
