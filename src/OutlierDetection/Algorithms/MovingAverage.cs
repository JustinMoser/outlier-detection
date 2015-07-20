using System;
using OutlierDetection.Model;

namespace OutlierDetection.Algorithms
{
    /// <summary>
    /// Simple moving average algo. 
    /// </summary>
    public class MovingAverageThresholdAlgorithm : IOutlierAlgorithm
    {
        private readonly double _threshold;

        public MovingAverageThresholdAlgorithm(double threshold)
        {
            if (threshold <= 0.0) throw new ArgumentException("factor must be larger than 0.0");
            _threshold = threshold;
        }

        public bool IsOutLier(TimeSeries series, TimeSeriesDataPoint samplePoint)
        {
            double diff = Math.Abs(samplePoint.Value - series.Mean)/series.Mean;
            return (diff > _threshold);
        }
    }
}
