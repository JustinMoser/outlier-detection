using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutlierDetection.Model;

namespace OutlierDetection.Algorithms
{
    public interface IOutlierAlgorithm
    {
        bool IsOutLier(TimeSeries series, TimeSeriesDataPoint samplePoint);
    }
}
