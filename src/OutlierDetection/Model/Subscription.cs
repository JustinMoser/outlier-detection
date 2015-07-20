using System;
using OutlierDetection.Model;
using OutlierDetection.Algorithms;
using System.Collections.Generic;

namespace OutlierDetection
{
	public class Subscription
	{
		public Subscription (string name, IOutlierAlgorithm algo)
		{
			Name = name;
			Algo = algo;
			Results = new List<TimeSeriesDataPoint> ();
		}

		public string Name { get; private set; }
		public IOutlierAlgorithm Algo { get; private set; }
		public ICollection<TimeSeriesDataPoint> Results { get; set; }
	}
}

