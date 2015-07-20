using System;
using System.Collections.Generic;
﻿using System.Linq;
﻿using OutlierDetection.Algorithms;
﻿using OutlierDetection.Data;
﻿using OutlierDetection.Managers;
using OutlierDetection.Matlab;
using OutlierDetection.Model;

namespace OutlierDetection
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            DataSourceBase data = new CSVDataSource("C:\\Users\\Justin\\Desktop\\Outliers.csv", true);

            List<TimeSeriesDataPoint> clean = new List<TimeSeriesDataPoint>();
            List<TimeSeriesDataPoint> outliers = new List<TimeSeriesDataPoint>();

            TimeSeriesManager tsm = new TimeSeriesManager(
                    new MovingAverageThresholdAlgorithm(0.055),
                    MeanType.Weighted,
                    clean.Add,
                    outliers.Add,
                    0.4);

            data.StartCollectData(tsm.Update);

            //TODO: Make StartCollectData async so can use await (execute data gather on separate thread)
            // and pass handlers as list of subscriptions (with classifier algo)
            // so that potentially in a long running gather (i.e stream for x amount of time)
            // as new data points are recieved and classified, necessary events can be fired.
            // For a stream based gather, the use of while(DataConnectionOpen) with whatever
            // logic inside would allow for 1 thread to carry on performing tasks on incoming 
            // data while the connection to the data source is present and recieving new data.


            Console.WriteLine("Reading Errors: " + data.Errors);

            Console.WriteLine("------ CLEANED TIME SERIES ------");

            foreach (TimeSeriesDataPoint dataPoint in clean)
            {
                Console.WriteLine("{0} : {1}", dataPoint.Period, dataPoint.Value);
            }

            Console.WriteLine("----------- OUTLIERS ------------");

            foreach (TimeSeriesDataPoint dataPoint in outliers)
            {
                Console.WriteLine("{0} : {1}", dataPoint.Period, dataPoint.Value);
            }


            /******* All below will need Matlab installed (and possibly running) to work *******/

            //List<double> dates = tsm.Series.DataPoints.Select(p => p.Period.ToOADate()).ToList();
            //List<double> values = tsm.Series.DataPoints.Select(p => p.Value).ToList();

            //List<double> valuesMean = tsm.MeanSeries.ToList();

            //MatlabIO.MyGraph2D(dates, values, "Raw Data", "Date", "Price");
            //MatlabIO.MyGraph2D(dates, valuesMean, "Mean", "Date", "Mean");

            Console.ReadLine();
        }
    }
}