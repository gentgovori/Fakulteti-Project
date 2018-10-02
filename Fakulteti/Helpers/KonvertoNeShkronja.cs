//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Web;

//namespace Fakulteti.Helpers
//{
//    public sealed class KonvertoNeShkronja
//    {
//        //clsKontrollat objKontrollat = new clsKontrollat();

//        public String changeNumericToWords(double numb)
//        {
//            String num = numb.ToString();
//            return changeToWords(num, false);
//        }

//        public String changeCurrencyToWords(String numb)
//        {
//            return changeToWords(numb, true);
//        }

//        public String changeNumericToWords(String numb)
//        {
//            return changeToWords(numb, false);
//        }

//        public String changeCurrencyToWords(double numb)
//        {
//            return changeToWords(numb.ToString(), true);
//        }

//        public string changeToWords(String numb, bool isCurrency)
//        {
//            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
//            String endStr = (isCurrency) ? ("") : ("");

//            try
//            {
//                int decimalPlace = numb.IndexOf(",");
//                if (decimalPlace > 0)
//                {
//                    wholeNo = numb.Substring(0, decimalPlace);
//                    points = numb.Substring(decimalPlace + 1);
//                    if (Convert.ToInt32(points) > 0)
//                    {
//                        andStr = (isCurrency) ? ("Euro e ") : (Resources.Resource.Konvertopikë );// just to separate whole numbers from points/cents 
//                        endStr = (isCurrency) ? (Resources.Resource.KonvertoCent + endStr) : ("");
//                        //andStr = (isCurrency) ? (objKontrollat.MerrePershkrimin("Konvertodhe")) : (objKontrollat.MerrePershkrimin("Konvertopikë"));// just to separate whole numbers from points/cents 
//                        //endStr = (isCurrency) ? (objKontrollat.MerrePershkrimin("KonvertoCent") + endStr) : ("");
//                        pointStr = translateCents(points);
//                    }
//                }
//                val = String.Format("{0} {1}{2} {3}", translateWholeNumber(wholeNo), andStr, pointStr, endStr);
//            }
//            catch {; }
//            return val;
//        }

//        private String translateWholeNumber(String number)
//        {
//            string word = "";
//            try
//            {
//                bool beginsZero = false;//tests for 0XX 
//                bool isDone = false;//test if already translated 
//                double dblAmt = (Convert.ToDouble(number));
//                //if ((dblAmt > 0) && number.StartsWith("0")) 
//                if (dblAmt > 0)
//                {//test for zero or digit zero in a nuemric 
//                    beginsZero = number.StartsWith("0");
//                    int numDigits = number.Length;
//                    int pos = 0;//store digit grouping 
//                    String place = "";//digit grouping name:hundres,thousand,etc... 
//                    switch (numDigits)
//                    {
//                        case 1://ones' range 
//                            word = ones(number);
//                            isDone = true;
//                            break;
//                        case 2://tens' range 
//                            word = tens(number);
//                            isDone = true;
//                            break;
//                        case 3://hundreds' range 
//                            pos = (numDigits % 3) + 1;
//                            place = Resources.Resource.KonvertoQind;// objKontrollat.MerrePershkrimin("KonvertoQind");
//                            break;
//                        case 4://thousands' range 
//                        case 5:
//                        case 6:
//                            pos = (numDigits % 4) + 1;
//                            place = Resources.Resource.KonvertoMije;// objKontrollat.MerrePershkrimin("KonvertoMije");
//                            break;
//                        case 7://millions' range 
//                        case 8:
//                        case 9:
//                            pos = (numDigits % 7) + 1;
//                            place = Resources.Resource.KonvertoMilion;// objKontrollat.MerrePershkrimin("KonvertoMilion");
//                            break;
//                        case 10://Billions's range 
//                            pos = (numDigits % 10) + 1;
//                            place = Resources.Resource.KonvertoMiliard;// objKontrollat.MerrePershkrimin("KonvertoMiliard");
//                            break;
//                        //add extra case options for anything above Billion... 
//                        default:
//                            isDone = true;
//                            break;
//                    }
//                    if (!isDone)
//                    {//if transalation is not done, continue...(Recursion comes in now!!) 
//                        word = translateWholeNumber(number.Substring(0, pos)) + place + " e " + translateWholeNumber(number.Substring(pos));
//                        //check for trailing zeros 
//                        if (beginsZero) word = Resources.Resource.Konvertodhe + word.Trim();
//                    }
//                    //ignore digit grouping names 
//                    if (word.Trim().Equals(place.Trim())) word = "";
//                }
//            }
//            catch {; }
//            return word/*.Trim()*/;
//        }
//        private String tens(String digit)
//        {
//            int digt = Convert.ToInt32(digit);
//            String name = null;
//            switch (digt)
//            {
//                case 10:
//                    name = Resources.Resource.KonvertoDhjete;// objKontrollat.MerrePershkrimin("KonvertoDhjetë");
//                    break;
//                case 11:
//                    name = Resources.Resource.KonvertoNjembdhjete;// objKontrollat.MerrePershkrimin("KonvertoNjëmbdhjetë");
//                    break;
//                case 12:
//                    name = Resources.Resource.KonvertoDymbdhjete;// objKontrollat.MerrePershkrimin("KonvertoDymbdhjetë");
//                    break;
//                case 13:
//                    name = Resources.Resource.KonvertoTrembdhjete;// objKontrollat.MerrePershkrimin("KonvertoTrembdhjetë");
//                    break;
//                case 14:
//                    name = Resources.Resource.KonvertoKatermbdhjete;// objKontrollat.MerrePershkrimin("KonvertoKatërmbdhjetë");
//                    break;
//                case 15:
//                    name = Resources.Resource.KonvertoPesembdhjete;// objKontrollat.MerrePershkrimin("KonvertoPesëmbdhjetë");
//                    break;
//                case 16:
//                    name = Resources.Resource.KonvertoGjashtembdhjete;// objKontrollat.MerrePershkrimin("KonvertoGjashtëmbdhjetë");
//                    break;
//                case 17:
//                    name = Resources.Resource.KonvertoShtatembdhjete;// objKontrollat.MerrePershkrimin("KonvertoShtatëmbdhjetë");
//                    break;
//                case 18:
//                    name = Resources.Resource.KonvertoTetembdhjete;// objKontrollat.MerrePershkrimin("KonvertoTetëmbdhjetë");
//                    break;
//                case 19:
//                    name = Resources.Resource.KonvertoNentembdhjete;// objKontrollat.MerrePershkrimin("KonvertoNëntëmbdhjetë");
//                    break;
//                case 20:
//                    name = Resources.Resource.KonvertoNjezete;// objKontrollat.MerrePershkrimin("KonvertoNjëzetë");
//                    break;
//                case 30:
//                    name = Resources.Resource.KonvertoTridhjete;// objKontrollat.MerrePershkrimin("KonvertoTridhjetë");
//                    break;
//                case 40:
//                    name = Resources.Resource.KonvertoKaterdhjete;// objKontrollat.MerrePershkrimin("KonvertoKatërdhjetë");
//                    break;
//                case 50:
//                    name = Resources.Resource.KonvertoPesedhjete;// objKontrollat.MerrePershkrimin("KonvertoPesëdhjetë");
//                    break;
//                case 60:
//                    name = Resources.Resource.KonvertoGjashtedhjete;// objKontrollat.MerrePershkrimin("KonvertoGjashtëdhjetë");
//                    break;
//                case 70:
//                    name = Resources.Resource.KonvertoShtatedhjete;// objKontrollat.MerrePershkrimin("KonvertoShtatëdhjetë");
//                    break;
//                case 80:
//                    name = Resources.Resource.KonvertoTetedhjete;// objKontrollat.MerrePershkrimin("KonvertoTetëdhjetë");
//                    break;
//                case 90:
//                    name = Resources.Resource.KonvertoNentedhjete;// objKontrollat.MerrePershkrimin("KonvertoNëntëdhjetë");
//                    break;
//                default:
//                    if (digt > 0)
//                    {
//                        name = tens(digit.Substring(0, 1) + "0") + " " + ones(digit.Substring(1));
//                    }
//                    break;
//            }
//            return name;
//        }
//        private String ones(String digit)
//        {
//            int digt = Convert.ToInt32(digit);
//            String name = "";
//            switch (digt)
//            {
//                case 1:
//                    name = Resources.Resource.KonvertoNje;// objKontrollat.MerrePershkrimin("KonvertoNjë");
//                    break;
//                case 2:
//                    name = Resources.Resource.KonvertoDy;// objKontrollat.MerrePershkrimin("KonvertoDy");
//                    break;
//                case 3:
//                    name = Resources.Resource.KonvertoTre;// objKontrollat.MerrePershkrimin("KonvertoTre");
//                    break;
//                case 4:
//                    name = Resources.Resource.KonvertoKater;// objKontrollat.MerrePershkrimin("KonvertoKatër");
//                    break;
//                case 5:
//                    name = Resources.Resource.KonvertoPese;// objKontrollat.MerrePershkrimin("KonvertoPesë");
//                    break;
//                case 6:
//                    name = Resources.Resource.KonvertoGjashte;// objKontrollat.MerrePershkrimin("KonvertoGjashtë");
//                    break;
//                case 7:
//                    name = Resources.Resource.KonvertoShtate;// objKontrollat.MerrePershkrimin("KonvertoShtatë");
//                    break;
//                case 8:
//                    name = Resources.Resource.KonvertoTete;//objKontrollat.MerrePershkrimin("KonvertoTetë");
//                    break;
//                case 9:
//                    name = Resources.Resource.KonvertoNente;//         objKontrollat.MerrePershkrimin("KonvertoNëntë");
//                    break;
//            }
//            return name;
//        }
//        private String translateCents(String number)
//        {
//            string word = "";
//            try
//            {
//                bool beginsZero = false;//tests for 0XX 
//                bool isDone = false;//test if already translated 
//                double dblAmt = (Convert.ToDouble(number));
//                //if ((dblAmt > 0) && number.StartsWith("0")) 
//                if (dblAmt > 0)
//                {//test for zero or digit zero in a nuemric 
//                    beginsZero = number.StartsWith("0");
//                    int numDigits = number.Length;
//                    int pos = 0;//store digit grouping 
//                    String place = "";//digit grouping name:hundres,thousand,etc... 
//                    switch (numDigits)
//                    {
//                        case 1://ones' range 
//                            word = ones(number);
//                            isDone = true;
//                            break;
//                        case 2://tens' range 
//                            word = tens(number);
//                            isDone = true;
//                            break;
//                        case 3://hundreds' range 
//                            pos = (numDigits % 3) + 1;
//                            place = Resources.Resource.KonvertoQind;// objKontrollat.MerrePershkrimin("KonvertoQind");
//                            break;
//                        case 4://thousands' range 
//                        case 5:
//                        case 6:
//                            pos = (numDigits % 4) + 1;
//                            place = Resources.Resource.KonvertoMije;// objKontrollat.MerrePershkrimin("KonvertoMije");
//                            break;
//                        case 7://millions' range 
//                        case 8:
//                        case 9:
//                            pos = (numDigits % 7) + 1;
//                            place = Resources.Resource.KonvertoMilion;// objKontrollat.MerrePershkrimin("KonvertoMilion");
//                            break;
//                        case 10://Billions's range 
//                            pos = (numDigits % 10) + 1;
//                            place = Resources.Resource.KonvertoMiliard;// objKontrollat.MerrePershkrimin("KonvertoMiliard");
//                            break;
//                        //add extra case options for anything above Billion... 
//                        default:
//                            isDone = true;
//                            break;
//                    }
//                    if (!isDone)
//                    {//if transalation is not done, continue...(Recursion comes in now!!) 
//                        word = translateWholeNumber(number.Substring(0, pos)) + place + translateWholeNumber(number.Substring(pos));
//                        //check for trailing zeros 
//                        if (beginsZero) word = Resources.Resource.Konvertodhe + word.Trim();
//                    }
//                    //ignore digit grouping names 
//                    if (word.Trim().Equals(place.Trim())) word = "";
//                }
//            }
//            catch {; }
//            return word/*.Trim()*/;
//            //String cts = "", digit = "", engOne = "";
//            //for (int i = 0; i < cents.Length; i++)
//            //{
//            //    digit = cents[i].ToString();
//            //    if (digit.Equals("0"))
//            //    {
//            //        engOne = Resources.Resource.KonvertoZero;// objKontrollat.MerrePershkrimin("KonvertoZero");
//            //    }
//            //    else
//            //    {
//            //        engOne = ones(digit);
//            //    }
//            //    cts += " " + engOne;
//            //}
//            //return cts;

//        }
//    }
//}

////    public class KonvertoNeShkronja
////    {     
////        public String changeNumericToWords(double numb)
////        {
////            String num = numb.ToString();
////            return changeToWords(num, false);
////        }

////        public String changeCurrencyToWords(String numb)
////        {
////            return changeToWords(numb, true);
////        }

////        public String changeNumericToWords(String numb)
////        {
////            return changeToWords(numb, false);
////        }

////        public String changeCurrencyToWords(double numb)
////        {
////            return changeToWords(numb.ToString(), true);
////        }

////        public string changeToWords(string numb, bool isCurrency)
////        {
////            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
////            String endStr = (isCurrency) ? (Resources.Resource.KonvertoVetëm) : ("");//objKontrollat.MerrePershkrimin("KonvertoVetëm")) : ("");

////            try
////            {
////                int decimalPlace = numb.IndexOf(".");
////                if (decimalPlace > 0)
////                {
////                    wholeNo = numb.Substring(0, decimalPlace);
////                    points = numb.Substring(decimalPlace + 1);
////                    if (Convert.ToInt32(points) > 0)
////                    {
////                        andStr = (isCurrency) ? (Resources.Resource.Konvertodhe) : ("");// just to separate whole numbers from points/cents 
////                        endStr = (isCurrency) ? (Resources.Resource.KonvertoCent + endStr) : ("");
////                        pointStr = translateCents(points);
////                    }
////                }
////                val = String.Format("{0} {1} {2} {3}", translateWholeNumber(wholeNo).Trim() + " euro", andStr, pointStr, endStr);
////            }
////            catch {; }
////            val = (val.Substring(0, 1).ToUpper() + val.Substring(1).ToLower());
////            val =Regex.Replace(val, @"\s+", " ");
////            return val;
////        }

////        private String translateWholeNumber(String number)
////        {
////            string word = "";
////            try
////            {
////                bool beginsZero = false;//tests for 0XX 
////                bool isDone = false;//test if already translated 
////                double dblAmt = (Convert.ToDouble(number));
////                //if ((dblAmt > 0) && number.StartsWith("0")) 
////                if (dblAmt > 0)
////                {//test for zero or digit zero in a nuemric 
////                    beginsZero = number.StartsWith("0");
////                    int numDigits = number.Length;
////                    int pos = 0;//store digit grouping 
////                    String place = "";//digit grouping name:hundres,thousand,etc... 
////                    switch (numDigits)
////                    {
////                        case 1://ones' range 
////                            word = ones(number);
////                            isDone = true;
////                            break;
////                        case 2://tens' range 
////                            word = tens(number);
////                            isDone = true;
////                            break;
////                        case 3://hundreds' range 
////                            pos = (numDigits % 3) + 1;
////                            place = Resources.Resource.KonvertoQind;
////                            break;
////                        case 4://thousands' range 
////                        case 5:
////                        case 6:
////                            pos = (numDigits % 4) + 1;
////                            place = Resources.Resource.KonvertoMije;
////                            break;
////                        case 7://millions' range 
////                        case 8:
////                        case 9:
////                            pos = (numDigits % 7) + 1;
////                            place = Resources.Resource.KonvertoMilion;
////                            break;
////                        case 10://Billions's range 
////                            pos = (numDigits % 10) + 1;
////                            place = Resources.Resource.KonvertoMiliard;
////                            break;
////                        //add extra case options for anything above Billion... 
////                        default:
////                            isDone = true;
////                            break;
////                    }
////                    if (!isDone)
////                    {//if transalation is not done, continue...(Recursion comes in now!!) 
////                        word = translateWholeNumber(number.Substring(0, pos)) + place + " " + translateWholeNumber(number.Substring(pos));
////                        //check for trailing zeros 
////                        if (beginsZero) word = Resources.Resource.Konvertodhe + word.Trim();
////                    }
////                    //ignore digit grouping names 
////                    if (word.Trim().Equals(place.Trim())) word = "";
////                }
////            }
////            catch {; }
////            return word.Trim();
////        }
////        private String tens(String digit)
////        {
////            int digt = Convert.ToInt32(digit);
////            String name = null;
////            switch (digt)
////            {
////                case 10:
////                    name = Resources.Resource.KonvertoDhjete;// objKontrollat.MerrePershkrimin("KonvertoDhjete");
////                    break;
////                case 11:
////                    name = Resources.Resource.KonvertoNjembdhjete;// objKontrollat.MerrePershkrimin("KonvertoNjembdhjete");
////                    break;
////                case 12:
////                    name = Resources.Resource.KonvertoDymbdhjete;// objKontrollat.MerrePershkrimin("KonvertoDymbdhjete");
////                    break;
////                case 13:
////                    name = Resources.Resource.KonvertoTrembdhjete;// objKontrollat.MerrePershkrimin("KonvertoTrembdhjete");
////                    break;
////                case 14:
////                    name = Resources.Resource.KonvertoKatermbdhjete;// objKontrollat.MerrePershkrimin("KonvertoKatermbdhjete");
////                    break;
////                case 15:
////                    name = Resources.Resource.KonvertoPesembdhjete;// objKontrollat.MerrePershkrimin("KonvertoPesembdhjete");
////                    break;
////                case 16:
////                    name = Resources.Resource.KonvertoGjashtembdhjete;// objKontrollat.MerrePershkrimin("KonvertoGjashtembdhjete");
////                    break;
////                case 17:
////                    name = Resources.Resource.KonvertoShtatembdhjete;// objKontrollat.MerrePershkrimin("KonvertoShtatembdhjete");
////                    break;
////                case 18:
////                    name = Resources.Resource.KonvertoTetembdhjete;// objKontrollat.MerrePershkrimin("KonvertoTetembdhjete");
////                    break;
////                case 19:
////                    name = Resources.Resource.KonvertoNentembdhjete;// objKontrollat.MerrePershkrimin("KonvertoNentembdhjete");
////                    break;
////                case 20:
////                    name = Resources.Resource.KonvertoNjezete;// objKontrollat.MerrePershkrimin("KonvertoNjezete");
////                    break;
////                case 30:
////                    name = Resources.Resource.KonvertoTridhjete;// objKontrollat.MerrePershkrimin("KonvertoTridhjete");
////                    break;
////                case 40:
////                    name = Resources.Resource.KonvertoKaterdhjete;// objKontrollat.MerrePershkrimin("KonvertoKaterdhjete");
////                    break;
////                case 50:
////                    name = Resources.Resource.KonvertoPesedhjete;// objKontrollat.MerrePershkrimin("KonvertoPesedhjete");
////                    break;
////                case 60:
////                    name = Resources.Resource.KonvertoGjashtedhjete;// objKontrollat.MerrePershkrimin("KonvertoGjashtedhjete");
////                    break;
////                case 70:
////                    name = Resources.Resource.KonvertoShtatedhjete;// objKontrollat.MerrePershkrimin("KonvertoShtatedhjete");
////                    break;
////                case 80:
////                    name = Resources.Resource.KonvertoTetedhjete;// objKontrollat.MerrePershkrimin("KonvertoTetedhjete");
////                    break;
////                case 90:
////                    name = Resources.Resource.KonvertoNentedhjete;// objKontrollat.MerrePershkrimin("KonvertoNentedhjete");
////                    break;
////                default:
////                    if (digt > 0)
////                    {
////                        name = tens(digit.Substring(0, 1) + "0") + " " + ones(digit.Substring(1));
////                    }
////                    break;
////            }
////            return name;
////        }
////        private String ones(String digit)
////        {
////            int digt = Convert.ToInt32(digit);
////            String name = "";
////            switch (digt)
////            {
////                case 1:
////                    name = Resources.Resource.KonvertoNje;// objKontrollat.MerrePershkrimin("KonvertoNje");
////                    break;
////                case 2:
////                    name = Resources.Resource.KonvertoDy;// objKontrollat.MerrePershkrimin("KonvertoDy"); //KonvertoDy
////                    break;
////                case 3:
////                    name = Resources.Resource.KonvertoTre;// objKontrollat.MerrePershkrimin("KonvertoTre");
////                    break;
////                case 4:
////                    name = Resources.Resource.KonvertoKater;//objKontrollat.MerrePershkrimin("KonvertoKater");
////                    break;
////                case 5:
////                    name = Resources.Resource.KonvertoPese;// objKontrollat.MerrePershkrimin("KonvertoPese");
////                    break;
////                case 6:
////                    name = Resources.Resource.KonvertoGjashte;//objKontrollat.MerrePershkrimin("KonvertoGjashte");
////                    break;
////                case 7:
////                    name = Resources.Resource.KonvertoShtate;// objKontrollat.MerrePershkrimin("KonvertoShtate");
////                    break;
////                case 8:
////                    name = Resources.Resource.KonvertoTete;//objKontrollat.MerrePershkrimin("KonvertoTete");
////                    break;
////                case 9:
////                    name = Resources.Resource.KonvertoNente;//objKontrollat.MerrePershkrimin("KonvertoNente");
////                    break;
////            }
////            return name;
////        }
////        private String translateCents(String cents)
////        {
////            if (cents.Length == 1)
////                cents = cents + "0";
////            String cts = "", digit = "", engOne = "";
////            for (int i = 0; i < cents.Length; i++)
////            {
////                digit = cents[i].ToString();
////                if (i == 1 && digit == "0")
////                    cts += "";
////                else
////                {
////                    if (digit.Equals("0"))
////                    {
////                        engOne = Resources.Resource.KonvertoZero;// objKontrollat.MerrePershkrimin("KonvertoZero");
////                    }
////                    else
////                    {
////                        if (i == 0 && cents.Length == 2)
////                            engOne = tens(digit + "0");
////                        else
////                            engOne = ones(digit);
////                    }
////                    cts += " e " + engOne;
////                }
////            }
////            return cts + " cent";


////        }
////    }
////}