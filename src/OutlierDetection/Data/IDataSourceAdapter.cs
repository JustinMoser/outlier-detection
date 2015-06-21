using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutlierDetection.Model;

namespace OutlierDetection.Data
{

    public interface IDataSourceAdapter
    {
        IEnumerable<TimeSeriesDataPoint> GetData();
        int Errors();
    }

}
