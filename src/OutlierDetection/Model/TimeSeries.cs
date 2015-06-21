using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutlierDetection.Algorithms;
using OutlierDetection.Extensions;

namespace OutlierDetection.Model
{
    public class TimeSeries
    {
        //private IOutlierAlgorithm _outlierAlgorithm;
        private int _pointCount;
        private double _pointSum;
        private double _prevMean;
        private readonly double _latestPointWeight;


        public ObservableCollection<TimeSeriesDataPoint> DataPoints { get; private set; }
        public double Mean { get; private set; }
        public double StdDev { get; private set; }

        public long SeriesCount
        {
            get { return _pointCount; }
        }

        public double SeriesSum
        {
            get { return _pointSum; }
        }

        //public bool LatestPointIsOutlier { get; private set; }

        public TimeSeries(double latestPointWeighting)//, IOutlierAlgorithm outlierAlgorithm)
        {
            //if(outlierAlgorithm == null) throw new ArgumentException("Provide outlier algo. Outlier algo cannot be null");
            //_outlierAlgorithm = outlierAlgorithm;
            DataPoints = new ObservableCollection<TimeSeriesDataPoint>();
            _latestPointWeight = latestPointWeighting;
            _pointCount = 0;
            _pointSum = 0.0;
            _prevMean = 0.0;

            DataPoints.CollectionChanged += DataPointsOnCollectionChanged;
        }

        private void DataPointsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //LatestPointIsOutlier = false;
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                TimeSeriesDataPoint dp = (TimeSeriesDataPoint)e.NewItems[0];
                _prevMean = Mean;
                _pointCount++;
                _pointSum += dp.Value;

                if (_pointCount == 0)
                {
                    Mean = 0;
                }
                else if (_pointCount == 1)
                {
                    Mean = _pointSum;
                }
                else
                {
                    //if (_outlierAlgorithm.IsOutLier(this, dp))
                    //{
                    //    LatestPointIsOutlier = true;
                    //}

                    Mean = ((1 - _latestPointWeight) * _prevMean) + (_latestPointWeight * (dp.Value));
                }

                StdDev = (_pointCount == 0 || _pointCount == 1) ? 0 : Math.Sqrt((DataPoints.Sum(p => Math.Pow(p.Value - Mean, 2))) / _pointCount);
            }
        }

        public void AddPointToSeries(TimeSeriesDataPoint dataPoint)
        {
            DataPoints.Add(dataPoint);
        }
    }

}
