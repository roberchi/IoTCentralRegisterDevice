using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Provisioning.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IoTCentralRegisterDevice
{
    internal class DeviceRegistration
    {
        private OptionParameters _parameters;

        public DeviceRegistration(OptionParameters parameters)
        {
            this._parameters = parameters;
        }

        public async Task RunSampleAsync()
        {
            Console.WriteLine($"Azure IoT Hub DPS...");

            string devicePrefix = _parameters.DevicePrefix!;
            int deviceNumber = _parameters.number;
            if (_parameters.EnrollmentType == EnrollmentType.Individual)
                deviceNumber = 1;

            for (int i = 0; i < deviceNumber; i++)
            {
                string originalPrimaryKey = _parameters.PrimaryKey!;
                byte[] decodeKey = Convert.FromBase64String(originalPrimaryKey);


                using HMACSHA256 hmac = new HMACSHA256(decodeKey);
                string deviceId;
                if (_parameters.EnrollmentType == EnrollmentType.Individual)
                    deviceId = _parameters.Id;
                else
                    deviceId = devicePrefix + "sn" + (i + 1);

                byte[] device_sign1 = hmac.ComputeHash(Encoding.ASCII.GetBytes(deviceId));

                string deviceKey = Convert.ToBase64String(device_sign1);


                Console.WriteLine($"Device id is {deviceId}");
                Console.WriteLine($"Device Key is {deviceKey}");

                using var security = new SecurityProviderSymmetricKey(
                    deviceId,
                    deviceKey,
                    null);

                using var transportHandler = GetTransportHandler();

                ProvisioningDeviceClient provClient = ProvisioningDeviceClient.Create(
                    _parameters.GlobalDeviceEndpoint,
                    _parameters.IdScope,
                    security,
                    transportHandler);

                Console.WriteLine($"Initialize DPS registration: The registration ID is: { security.GetRegistrationID()}.");
 
                Console.WriteLine("Sign up with DPS service...");
                Console.WriteLine($"Registering the {i + 1} device...");
                DeviceRegistrationResult result = await provClient.RegisterAsync();

                Console.WriteLine($"Registration Status: { result.Status}.");
                if (result.Status != ProvisioningRegistrationStatusType.Assigned)
                {
                    Console.WriteLine($"Failed to assign suitable Azure IoT Hub.");
                    return;
                }

                Console.WriteLine($"Device ID: {result.DeviceId} successfully registered to IoT Hub: {result.AssignedHub}.");
                
                var connectionString = $"HostName={result.AssignedHub};DeviceId={result.DeviceId};SharedAccessKey={deviceKey}";
                Console.WriteLine($"Connection String: {connectionString}");

                Console.WriteLine("Create SAS Key authentication for IoT Hub...");
                IAuthenticationMethod auth = new DeviceAuthenticationWithRegistrySymmetricKey(
                    result.DeviceId,
                    security.GetPrimaryKey());

                Console.WriteLine($"Test connection to Azure IoT Hub...");
                using DeviceClient iotClient = DeviceClient.Create(result.AssignedHub, auth, _parameters.TransportType);

                Console.WriteLine("Send test message...");
                using var message = new Message(Encoding.UTF8.GetBytes("TestMessage"));
                await iotClient.SendEventAsync(message);

                Console.WriteLine($"Completion of { i + 1} device registration and testing.");
            }
        }

        private ProvisioningTransportHandler GetTransportHandler()
        {
            return _parameters.TransportType switch
            {
                TransportType.Mqtt => new ProvisioningTransportHandlerMqtt(),
                TransportType.Mqtt_Tcp_Only => new ProvisioningTransportHandlerMqtt(TransportFallbackType.TcpOnly),
                TransportType.Mqtt_WebSocket_Only => new ProvisioningTransportHandlerMqtt(TransportFallbackType.WebSocketOnly),
                TransportType.Amqp => new ProvisioningTransportHandlerAmqp(),
                TransportType.Amqp_Tcp_Only => new ProvisioningTransportHandlerAmqp(TransportFallbackType.TcpOnly),
                TransportType.Amqp_WebSocket_Only => new ProvisioningTransportHandlerAmqp(TransportFallbackType.WebSocketOnly),
                TransportType.Http1 => new ProvisioningTransportHandlerHttp(),
                _ => throw new NotSupportedException($"Unsupported network protocol type {_parameters.TransportType}"),
            };
        }
    }
}