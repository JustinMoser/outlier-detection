using System;
using OutlierDetection.Model;

namespace OutlierDetection.Algorithms
{
    public class StationarySeriesStandardDeviationFactor : IOutlierAlgorithm
    {
        private readonly double _factor;

        public StationarySeriesStandardDeviationFactor(double factor)
        {
            if (factor <= 0.0) throw new ArgumentException("factor must be larger than 0.0");
            _factor = factor;
        }

        public bool IsOutLier(TimeSeries series, TimeSeriesDataPoint samplePoint)
        {
            double diff = Math.Abs(samplePoint.Value - series.Mean);
            return diff > (_factor * series.StdDev);
        }
    }
}
