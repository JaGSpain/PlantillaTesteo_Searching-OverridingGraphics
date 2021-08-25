using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaTesteo
{
    public static class Util
    {
        public static void HandleError(Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.Source);
            Debug.WriteLine(ex.StackTrace);
        }
    }
}
