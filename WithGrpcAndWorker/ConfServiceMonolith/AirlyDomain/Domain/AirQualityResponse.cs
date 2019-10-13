using System;
using System.Collections.Generic;

namespace AirlyInterface.Domain
{
    public class AirQualityResponse
    {
        public AirQualityResponse(AirlyStatusCode statusCode, string errorText, DateTime fromDateTime, IEnumerable<AirlyNamedValue> values)
        {
            StatusCode = statusCode;
            ErrorText = errorText;
            FromDateTime = fromDateTime;
            Values = values;
        }

        public DateTime FromDateTime { get; }
        public IEnumerable<AirlyNamedValue> Values { get; }
        public AirlyStatusCode StatusCode { get; }
        public string ErrorText { get; }

        public static AirQualityResponse Empty => new AirQualityResponse(
                    statusCode: AirlyStatusCode.OtherError,
                    errorText: "Empty",
                    fromDateTime: DateTime.UtcNow,
                    values: new List<AirlyNamedValue>()
                    );
    }


    public enum AirlyStatusCode
    {
        Ok,
        NotFound,
        OtherError
    }
}
