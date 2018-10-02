using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fakulteti.Helpers
{
    public class Utilities
    {
        public static int GetGjuhaId(Gjuha gjuha)
        {
            switch (gjuha)
            {
                case Gjuha.Shqip:
                    return 1;
                //case Gjuha.English:
                //    return 2;
                //case Gjuha.Srpski:
                //    return 3;
                default:
                    return 1;
            }
        }
    }
}