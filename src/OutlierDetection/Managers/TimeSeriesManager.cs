using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutlierDetection.Algorithms;
using OutlierDetection.Data;
using OutlierDetection.Model;

namespace OutlierDetection.Managers
{
    public class TimeSeriesManager
    {
        private readonly IOutlierAlgorithm _outlierAlgorithm;
        private readonly IDataSourceAdapter _dataSource;
        private readonly ICollection<TimeSeriesDataPoint> _cleanSet;
        private readonly ICollection<TimeSeriesDataPoint> _outliers;
        private readonly int _frequency;

        public TimeSeries Series { get; private set; }
        public ICollection<double> MeanSeries { get; private set; }


        public TimeSeriesManager(IDataSourceAdapter dataSource, IOutlierAlgorithm outlierAlgo, int frequency = 0)
        {
            if (dataSource == null) throw new ArgumentException("Provide a data source");
            if (outlierAlgo == null) throw new ArgumentException("Provide an outlier algorithm");

            _outlierAlgorithm = outlierAlgo;
            _dataSource = dataSource;
            _cleanSet = new List<TimeSeriesDataPoint>();
            _outliers = new List<TimeSeriesDataPoint>();
            _frequency = frequency;

            Series = new TimeSeries(0.4);//, outlierAlgo);
            MeanSeries = new List<double>();
        }

        public void CreateTimeSeries()
        {
            foreach (TimeSeriesDataPoint dp in _dataSource.GetData())
            {
                if (Series.SeriesCount > 0)
                {
                    if (_outlierAlgorithm.IsOutLier(Series, dp))
                    {
                        _outliers.Add(dp);
                    }
                    else
                    {
                        _cleanSet.Add(dp);
                    }
                }

                Series.AddPointToSeries(dp);
                MeanSeries.Add(Series.Mean);
            }

            Console.WriteLine("Reading Errors: " + _dataSource.Errors());
        }

        public ICollection<TimeSeriesDataPoint> CleanData()
        {
            return _cleanSet;
        }

        public IEnumerable<TimeSeriesDataPoint> Outliers()
        {
            return _outliers;
        }
    }
}
