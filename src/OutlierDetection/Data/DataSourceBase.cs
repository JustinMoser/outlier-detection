using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using OutlierDetection.Model;

namespace OutlierDetection
{
	public abstract class DataSourceBase
	{
		public DataSourceBase()
		{
			Data = new ObservableCollection<TimeSeriesDataPoint>();
		}

		public virtual void StartCollectData(Action<TimeSeriesDataPoint> f)
		{
			Data.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => 
			{
				if(e.Action == NotifyCollectionChangedAction.Add)
				{
					TimeSeriesDataPoint dp = (TimeSeriesDataPoint)e.NewItems[0];
					f(dp);
				}
			};

			DataConnectionOpen = true;
			GetData ();
		}		

		protected abstract void GetData();

		public virtual void StopCollectData()
		{
			if (DataConnectionOpen)
				DataConnectionOpen = false;
		}

		public int Errors { get; protected set; }

		public ObservableCollection<TimeSeriesDataPoint> Data { get; protected set; }

		public bool DataConnectionOpen { get; protected set; }
	}
}

