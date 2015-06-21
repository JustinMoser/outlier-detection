using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlierDetection.Model
{
    public class TimeSeriesDataPoint
    {
        public DateTime Period { get; set; }
        public double Value { get; set; }
    }
}
