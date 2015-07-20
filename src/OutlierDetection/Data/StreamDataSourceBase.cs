using System;

namespace OutlierDetection
{
	public class StreamDataSourceBase : DataSourceBase
	{
		private string _address;

		public StreamDataSourceBase (string address)
		{
			if (!String.IsNullOrEmpty (address))
				throw new Exception ("Provide a stream address");

			_address = address;
		}

		protected override void GetData ()
		{
			throw new NotImplementedException ();
		}
	}
}

