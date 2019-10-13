namespace AirlyPublicApiTestConsoleApp
{
    public class QueryResult
    {
        public QueryResult(string city, string response, string error, double durationInSeconds)
        {
            City = city;
            Response = response;
            Error = error;
            DurationInSeconds = durationInSeconds;
        }

        public string City { get; }
        public string Response { get; }
        public string Error { get; }
        public double DurationInSeconds { get; }

        public override string ToString()
        {
            return ToString(41);
        }
        public string ToString(int maxCharsLength)
        {
            var responseFormatted = CutToMaxChars(Response, maxCharsLength);
            var errorFormatted = FormatError(Error, maxCharsLength);
            var formatted = Error == null
                ? $"Success: {responseFormatted}"
                : $"Error  : {errorFormatted}";
            return $"{City}, took {DurationInSeconds:F2}s: {formatted}";
        }

        private string FormatError(string error, int charsToKeepArg)
        {
            if (string.IsNullOrWhiteSpace(error))
                return "";
            if (error.ToUpper().Contains("INSTALLATION_NOT_FOUND"))
                return "Installation not found.";
            else if (error.ToUpper().Contains("API LIMIT"))
                return "Api limit exceeded.";
            return CutToMaxChars(error, charsToKeepArg);
        }

        private string CutToMaxChars(string text, int charsToKeepArg)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            var keepNumberOfChars = text.Length;
            if (keepNumberOfChars > charsToKeepArg)
                keepNumberOfChars = charsToKeepArg;
            return text.Substring(0, keepNumberOfChars);
        }
    }
}
