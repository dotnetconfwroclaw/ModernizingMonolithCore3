using System;

namespace AirlyAccessing.TechnicalRequesting
{
    public class ApiLimitChecker : IApiLimitChecker
    {
        private DateTime currentMinute = default;
        int queriesLeftThisMinute = 1;
        int queriesLeftToday = 1;

        public bool CanExecuteRequest(DateTime timeOfRequesting)
        {
            return IsDaLimitCheckPassing(timeOfRequesting) && IsMinuteLimitCheckPassing(timeOfRequesting);
        }

        private bool IsMinuteLimitCheckPassing(DateTime timeOfRequesting)
        {
            bool minuteLimitCheckPassed;
            if (!IsSameMinute(currentMinute, timeOfRequesting))
            {
                minuteLimitCheckPassed = true;
            }
            else
            {
                minuteLimitCheckPassed = queriesLeftThisMinute > 0;
            }

            return minuteLimitCheckPassed;
        }

        private bool IsDaLimitCheckPassing(DateTime timeOfRequesting)
        {
            bool dayLimitCheckPassed;
            if (!IsSameDate(currentMinute, timeOfRequesting))
            {
                dayLimitCheckPassed = true;
            }
            else
            {
                dayLimitCheckPassed = queriesLeftToday > 0;
            }

            return dayLimitCheckPassed;
        }

        private bool IsSameDate(DateTime first, DateTime second)
        {
            return first.Date == second.Date;
        }

        private bool IsSameMinute(DateTime first, DateTime second)
        {
            return first.Year == second.Year
                && first.Month == second.Month
                && first.Day == second.Day
                && first.Hour == second.Hour
                && first.Minute == second.Minute;
        }
        public void SetResultingApiLimits(DateTime timeOfRequesting, int queriesLeftThisMinuteFromApi, int queriesLeftTodayFromApi)
        {
            queriesLeftToday = queriesLeftTodayFromApi;
            queriesLeftThisMinute = queriesLeftThisMinuteFromApi;
            currentMinute = timeOfRequesting;
        }
    }
}


