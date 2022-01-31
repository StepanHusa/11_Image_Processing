using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor.Imaging.Filters.EdgeDetection;

namespace _11_Image_Processing
{
    public class LaplacianOWNEdgeFilter : IEdgeFilter
    {
        /// <summary>
        /// Gets the horizontal gradient operator.
        /// </summary>
        public double[,] HorizontalGradientOperator => new double[,]
        {
            { -1, -1, -1 },
            { -1,  8, -1 },
            { -1, -1, -1 }
        };
    }
    public class LaplacianOWNEdgeFilter2 : IEdgeFilter
    {
        /// <summary>
        /// Gets the horizontal gradient operator.
        /// </summary>
        public double[,] HorizontalGradientOperator => new double[,]
        {
            { 0, 0 ,  -1 , 0 ,  0 },
            { 0  , -1  ,-2 , -1 , 0 },
            { -1 , -2 , 16 , -2,  -1 },
            { 0  , -1  ,-2 , -1 , 0 },
            { 0, 0 ,  -1 , 0 ,  0 }
        };
    }
    public class LaplacianOWNEdgeFilter4 : IEdgeFilter
    {
        /// <summary>
        /// Gets the horizontal gradient operator.
        /// </summary>
        public double[,] HorizontalGradientOperator => new double[,]
        {
            {0,1,1,2,2,2,1,1,0 },
            {1,2,4,5,5,5,4,2,1 },
            {1,4,5,3,0,3,5,4,1 },
            {2,5,3,-12,-24,-12,3,5,2 },
            {2,5,0,-24,-40,-24,0,5,2 },
            {2,5,3,-12,-24,-12,3,5,2 },
            {1,4,5,3,0,3,5,4,1 },
            {1,2,4,5,5,5,4,2,1 },
            {0,1,1,2,2,2,1,1,0 }
        };
    }
    public class LaplacianOWNEdgeFilter3 : IEdgeFilter
    {
        /// <summary>
        /// Gets the horizontal gradient operator.
        /// </summary>
        public double[,] HorizontalGradientOperator => new double[,]
        {
            {0,.1,.1,.2,.2,.2,.1,.1,0 },
            {.1,.2,.4,.5,.5,.5,.4,.2,.1 },
            {.1,.4,.5,.3,0,.3,.5,.4,.1 },
            {.2,.5,.3,-1.2,-2.4,-1.2,.3,.5,.2 },
            {.2,.5,0,-2.4,-4.0,-2.4,0,.5,.2 },
            {.2,.5,.3,-1.2,-2.4,-1.2,.3,.5,.2 },
            {.1,.4,.5,.3,0,.3,.5,.4,.1 },
            {.1,.2,.4,.5,.5,.5,.4,.2,.1 },
            {0,.1,.1,.2,.2,.2,.1,.1,0 }
        };

    }
}
