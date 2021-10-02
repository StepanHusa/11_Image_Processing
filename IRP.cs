using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using PdfSharp;

namespace _13_Testing_Software_PNGused
{
    public static class IRP
    {
        public static void IRPM(this List<Rectangle> list) 
        {
            foreach (var item in list)
            {
                Logical(item);
            }
        }

        static public bool Logical(this Rectangle space)
        {

            return true;
        }
    }
}
