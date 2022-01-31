using ImageProcessor.Imaging.Filters.EdgeDetection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_Image_Processing
{
    static class Extensions
    {
        public static Bitmap LaplasTransform1(this Bitmap bitmap)
        {
            return new ConvolutionFilter(new LaplacianOWNEdgeFilter(), true).ProcessFilter(bitmap);
        }
        public static Bitmap LaplasTransform2(this Bitmap bitmap)
        {
            return new ConvolutionFilter(new LaplacianOWNEdgeFilter2(), true).ProcessFilter(bitmap);
        }
        public static Bitmap LaplasTransform3(this Bitmap bitmap)
        {
            return new ConvolutionFilter(new LaplacianOWNEdgeFilter3(), true).ProcessFilter(bitmap);
        }

    }
}
