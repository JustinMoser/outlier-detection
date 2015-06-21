﻿using System;
using System.Collections.Generic;
﻿using System.Linq;
﻿using OutlierDetection.Algorithms;
﻿using OutlierDetection.Data;
﻿using OutlierDetection.Managers;
﻿using OutlierDetection.Matlab;
﻿using OutlierDetection.Model;

namespace OutlierDetection
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            CSVDataSource data = new CSVDataSource("C:\\Users\\Justin\\Desktop\\outliers.csv", true);
            TimeSeriesManager tsm = new TimeSeriesManager(data, new MovingAverageThresholdAlgorithm(0.075));

            tsm.CreateTimeSeries();
            ICollection<TimeSeriesDataPoint> clean = tsm.CleanData();
            IEnumerable<TimeSeriesDataPoint> outliers = tsm.Outliers();


            Console.WriteLine("------ CLEANED TIME SERIES ------");

            foreach (TimeSeriesDataPoint dataPoint in clean)
            {
                Console.WriteLine("{0},{1}", dataPoint.Period, dataPoint.Value);
            }

            Console.WriteLine("----------- OUTLIERS ------------");

            foreach (TimeSeriesDataPoint dataPoint in outliers)
            {
                Console.WriteLine("{0},{1}", dataPoint.Period, dataPoint.Value);
            }

            List<double> dates = tsm.Series.DataPoints.Select(p => p.Period.ToOADate()).ToList();
            List<double> values = tsm.Series.DataPoints.Select(p => p.Value).ToList();

            List<double> valuesMean = tsm.MeanSeries.ToList();

            MatlabIO.MyGraph2D(dates, values, "Raw Data", "Date", "Price");
            MatlabIO.MyGraph2D(dates, valuesMean, "Mean", "Date", "Mean");

            Console.ReadLine();
        }
    }
}