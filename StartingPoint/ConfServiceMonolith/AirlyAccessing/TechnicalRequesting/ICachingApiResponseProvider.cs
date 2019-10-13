using System;
using System.Threading.Tasks;

namespace AirlyAccessing.TechnicalRequesting
{
    public interface ICachingApiResponseProvider
    {
        Task<TechnicalResponse> GetApiResponse(string requestText, DateTime now);
    }
}