using System;
using System.Collections.Generic;
using OutlierDetection.Algorithms;
using OutlierDetection.Data;
using OutlierDetection.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace OutlierDetection.Managers
{
    public class TimeSeriesManager
    {
        private readonly IOutlierAlgorithm _outlierAlgorithm;
		public ICollection<TimeSeriesDataPoint> _clean;
		public ICollection<TimeSeriesDataPoint> _outliers;
		private Action<TimeSeriesDataPoint> _cleanF;
		private Action<TimeSeriesDataPoint> _outlierF;

        public TimeSeries Series { get; private set; }
        public ICollection<double> MeanSeries { get; private set; }


		public TimeSeriesManager(IOutlierAlgorithm algo, 
			MeanType meanType, 
			Action<TimeSeriesDataPoint> cleanSubscriptionF,
			Action<TimeSeriesDataPoint> outlierSubscriptionF,
			double pointWeighting = 0.0)
        {
			if (algo == null) throw new ArgumentException("Please provide an outlier algo");
			_outlierAlgorithm = algo;
			_clean = new ObservableCollection<TimeSeriesDataPoint>();
			_outliers = new ObservableCollection<TimeSeriesDataPoint>();
			_cleanF = cleanSubscriptionF;
			_outlierF = outlierSubscriptionF;
			Series = new TimeSeries(meanType, pointWeighting);
            MeanSeries = new List<double>();
        }

        public void Update(TimeSeriesDataPoint dp)
        {
            Series.AddPointToSeries(dp);
            MeanSeries.Add(Series.Mean);

            if (Series.SeriesCount > 0)
            {
                if (_outlierAlgorithm.IsOutLier(Series, dp))
                {
					_outlierF(dp);
                }
                else
                {
					_cleanF(dp);
                }
            }
        }
    }
}
