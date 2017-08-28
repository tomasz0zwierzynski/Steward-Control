using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Stewart_Control
{
    public class InverseParams
    {
        public double[] XYZrangeMin = new double[3]; //0-X 1-Y 2-Z
        public double[] XYZrangeMax = new double[3];

        public double[] ABCrangeMin = new double[3]; //0-A 1-B 2-C
        public double[] ABCrangeMax = new double[3];

        public InverseParams(double[] min_xyz, double[] max_xyz, double[] min_abc, double[] max_abc)
        {
            try
            {
                XYZrangeMin = min_xyz;
                XYZrangeMax = max_xyz;
                ABCrangeMin = min_abc;
                ABCrangeMax = max_abc;
            }
            catch
            {

            }

        }
    }
}