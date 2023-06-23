namespace Zartis.OpenTelemetry.VehicleServiceApi.Contracts
{
    public class VehicleDto
    {
        public string Vin { get; set; }

        public IList<VehicleComponentDto> Components { get; set; }

    }
}
