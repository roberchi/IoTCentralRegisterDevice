using System;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Microsoft.Azure.Devices.Provisioning.Client;
using Microsoft.Azure.Devices.Shared;
using System.Text;
using CommandLine;
using System.Security.Cryptography;


namespace IoTCentralRegisterDevice
{
    class Program
    {
        static async Task Main(string[] args)
        {
            OptionParameters parameters = null!;
            ParserResult<OptionParameters> result = Parser.Default.ParseArguments<OptionParameters>(args)
                .WithParsed(parsedParams =>
                {
                    parameters = parsedParams;
                })
                .WithNotParsed(errors =>
                {
                    Environment.Exit(1);
                });

            DeviceRegistration client = new DeviceRegistration(parameters);
            try
            {
                await client.RunSampleAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Failed: {e}\r\n{e.Message}");
            }
        }
    }
}
