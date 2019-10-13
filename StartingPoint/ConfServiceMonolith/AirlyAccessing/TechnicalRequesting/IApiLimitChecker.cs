using System;

namespace AirlyAccessing.TechnicalRequesting
{
    public interface IApiLimitChecker
    {
        void SetResultingApiLimits(DateTime timeOfRequesting, int queriesLeftThisMinute, int queriesLeftToday);
        bool CanExecuteRequest(DateTime timeOfRequesting);
    }
}