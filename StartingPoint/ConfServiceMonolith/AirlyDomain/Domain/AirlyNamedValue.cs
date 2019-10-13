namespace AirlyInterface.Domain
{
    public class AirlyNamedValue
    {
        public AirlyNamedValue(string name, double? value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public double? Value { get; }
    }
}
