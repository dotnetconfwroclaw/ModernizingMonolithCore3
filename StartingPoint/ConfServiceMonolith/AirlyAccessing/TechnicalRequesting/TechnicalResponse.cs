using System;
using System.Net;

namespace AirlyAccessing.TechnicalRequesting
{
    public class TechnicalResponse
    {
        public TechnicalResponse(bool isSuccess, string responseText, HttpStatusCode statusCode, int queriesLeftThisMinute, int queriesLeftToday, string requestText, DateTime timeOfRequest)
        {
            IsSuccess = isSuccess;
            ResponseText = responseText;
            StatusCode = statusCode;
            QueriesLeftThisMinute = queriesLeftThisMinute;
            QueriesLeftToday = queriesLeftToday;
            RequestText = requestText;
            TimeOfRequest = timeOfRequest;
        }

        public bool IsSuccess { get; }
        public HttpStatusCode StatusCode { get; }
        public int QueriesLeftThisMinute { get; }
        public int QueriesLeftToday { get; }
        public string RequestText { get; }
        public string ResponseText { get; }
        public DateTime TimeOfRequest { get; }
    }
}
