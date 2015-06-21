using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using OutlierDetection.Model;

namespace OutlierDetection.Data
{
    public class CSVDataSource : IDataSourceAdapter
    {
        private readonly string _file;
        private int _errors;
        private bool _skipHeaders;

        public CSVDataSource(string file, bool skipHeaders = false)
        {
            if (String.IsNullOrEmpty(file)) throw new Exception("please provide a file to read");
            if (File.Exists(file))
            {
                _file = file;
                _skipHeaders = skipHeaders;
            }
            else
            {
                throw new ArgumentException("Cannot find specified file");
            }
        }

        public IEnumerable<TimeSeriesDataPoint> GetData()
        {
            using (StreamReader sr = new StreamReader(_file))
            {
                string currentLine;
                int index = 1;
                while ((currentLine = sr.ReadLine()) != null)
                {
                    if (_skipHeaders && index == 1)
                    {
                        currentLine = sr.ReadLine();
                    }

                    string[] components = currentLine.Split(',');
                    string[] dateComponents = components[0].Split('/');

                    if (dateComponents[0].Length == 1)
                    {
                        dateComponents[0] = String.Format("0{0}", dateComponents[0]);
                    }

                    if (dateComponents[1].Length == 1)
                    {
                        dateComponents[1] = String.Format("0{0}", dateComponents[1]);
                    }

                    components[0] = String.Join("/", dateComponents);

                    DateTime period = new DateTime();

                    if (DateTime.TryParseExact(components[0], "dd/MM/yyyy", null, DateTimeStyles.None, out period))
                    {
                        double value;
                        if (double.TryParse(components[1], out value))
                        {
                            yield return new TimeSeriesDataPoint() { Period = period, Value = value };
                        }
                        else
                        {
                            _errors++;
                        }
                    }
                    else
                    {
                        _errors++;
                    }

                    index++;
                }
            }
        }

        public int Errors()
        {
            return _errors;
        }
    }
}
