using Clients;
using Microsoft.EntityFrameworkCore;
using Zartis.OpenTelemetry.VehicleServiceApi.Contracts;
using Zartis.OpenTelemetry.VehicleServiceApi.Domain;
using Zartis.OpenTelemetry.VehicleServiceApi.Persistence;


namespace Zartis.OpenTelemetry.VehicleServiceApi.Services
{
    public class VehicleService
    {
        private readonly VehiclesContext _vehiclesContext;
        private readonly ComponentsApiClient _componentsApiClient;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(VehiclesContext vehiclesContext, ComponentsApiClient componentssApiClient, ILogger<VehicleService> logger)
        {
            _vehiclesContext = vehiclesContext;
            _componentsApiClient = componentssApiClient;
            _logger = logger;
        }

        public async Task<VehicleDto> GetFullInformationFromSystemAsync(string vin)
        {
            if (string.IsNullOrWhiteSpace(vin))
                throw new ArgumentException("Provided VIN was null, empty or a set of white spaces. Invalid value for Vehicle retrieval.", nameof(vin));

            var vehicle = await _vehiclesContext.Vehicles.SingleAsync(v => v.Vin == vin);

            //var listOfComponents = await GetListOfComponentsSequentiallyAsync(vehicle);
            var listOfComponents = await GetListOfComponentsInParallelAsync(vehicle);

            return new VehicleDto
            {
                Vin = vehicle.Vin,
                Components = listOfComponents.Select(c => new VehicleComponentDto { SerialNumber = c.SerialNumber, Type = c.Type }).ToList()
            };
        }

        private async Task<IList<ComponentDto>> GetListOfComponentsSequentiallyAsync(Vehicle vehicle)
        {
            var result = new List<ComponentDto>();

            foreach (var component in vehicle.ComponentSerialNumbers)
            {
                _logger.LogInformation("Retrieving component information for serial number {ComponentSerialNumber}", component.ToString());
                result.Add(await _componentsApiClient.GetComponentUsingGETAsync(component.ToString()));
            }

            return result;
        }

        private async Task<IList<ComponentDto>> GetListOfComponentsInParallelAsync(Vehicle vehicle)
        {
            var tasks = vehicle.ComponentSerialNumbers.Select(async component =>
            {
                _logger.LogInformation("Retrieving component information for serial number {ComponentSerialNumber}", component.ToString());
                return await _componentsApiClient.GetComponentUsingGETAsync(component.ToString());
            });

            var result = await Task.WhenAll(tasks);

            return result.ToList();
        }
    }
}
