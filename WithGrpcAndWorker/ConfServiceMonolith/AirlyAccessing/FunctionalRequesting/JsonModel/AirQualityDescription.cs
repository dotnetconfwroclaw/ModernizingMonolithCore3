using System;
using System.Collections.Generic;

namespace AirlyAccessing.FunctionalRequesting.JsonModel
{
    public class NamedValue
    {
        public string name { get; set; }
        public double? value { get; set; }
    }

    public class Index
    {
        public string name { get; set; }
        public double? value { get; set; }
        public string level { get; set; }
        public string description { get; set; }
        public string advice { get; set; }
        public string color { get; set; }
    }

    public class Standard
    {
        public string name { get; set; }
        public string pollutant { get; set; }
        public double? limit { get; set; }
        public double? percent { get; set; }
    }

    public class Current
    {
        public DateTime fromDateTime { get; set; }
        public DateTime tillDateTime { get; set; }
        public List<NamedValue> values { get; set; }
        public List<Index> indexes { get; set; }
        public List<Standard> standards { get; set; }
    }

    public class History
    {
        public DateTime fromDateTime { get; set; }
        public DateTime tillDateTime { get; set; }
        public List<NamedValue> values { get; set; }
        public List<Index> indexes { get; set; }
        public List<Standard> standards { get; set; }
    }

    public class Forecast
    {
        public DateTime fromDateTime { get; set; }
        public DateTime tillDateTime { get; set; }
        public List<NamedValue> values { get; set; }
        public List<Index> indexes { get; set; }
        public List<Standard> standards { get; set; }
    }

    public class AirQualityDescription
    {
        public Current current { get; set; }
        public List<History> history { get; set; }
        public List<Forecast> forecast { get; set; }
    }

}
