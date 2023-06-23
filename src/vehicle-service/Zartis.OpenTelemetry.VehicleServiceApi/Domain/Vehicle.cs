namespace Zartis.OpenTelemetry.VehicleServiceApi.Domain
{
    public class Vehicle
    {
        public string Vin { get; }

        public List<Guid> ComponentSerialNumbers { get; }

        public Vehicle(string vin)
        {
            if (string.IsNullOrWhiteSpace(vin))
                throw new ArgumentException("Vehicle Identifier Number (VIN) cannot be null, empty or a set of white spaces", nameof(vin));

            Vin = vin;
            ComponentSerialNumbers = new List<Guid>();
        }

        public Vehicle WithComponentSerialNumber(Guid componentSerialNumber)
        {
            ComponentSerialNumbers.Add(componentSerialNumber);

            return this;
        }

        protected Vehicle()
        {
            // Remark :: Needed for EF
        }

    }
}
