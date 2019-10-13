using System;
using System.Threading.Tasks;

namespace AirlyAccessing.TechnicalRequesting
{
    public interface IApiRequestExecutor
    {
        Task<TechnicalResponse> GetRawResponse(string requestText, DateTime timeOfRequest);
    }
}