using CommandLine;
using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTCentralRegisterDevice
{
    public class OptionParameters
    {
        [Option(
            's',
            "IdScope",
            Required = true,
            HelpText = "The Id Scope of the DPS instance")]
        public string? IdScope { get; set; }

        [Option(
            'i',
            "Id",
            Required = true,
            HelpText = "The registration Id when using individual enrollment, or the desired device Id when using group enrollment.")]
        public string? Id { get; set; }

        [Option(
            'p',
            "PrimaryKey",
            Required = true,
            HelpText = "The primary key of the individual enrollment or the derived primary key of the group enrollment. See the ComputeDerivedSymmetricKeyGroupSample for how to generate the derived key.")]
        public string? PrimaryKey { get; set; }

        [Option(
            'f',
            "DevicePrefix",
            Required = true,
            HelpText = "The Device Prefix for all Device on group enrolment"
        )]
        public string? DevicePrefix { get; set; }

        [Option(
            'n',
            "number",
            Required = false,
            Default = 1,
            HelpText = "How Many device you want to auto-matic generate"
        )]
        public int number { get; set; }

        [Option(
            'e',
            "EnrollmentType",
            Default = EnrollmentType.Individual,
            HelpText = "The type of enrollment: Individual or Group")]
        public EnrollmentType EnrollmentType { get; set; }

        [Option(
            'g',
            "GlobalDeviceEndpoint",
            Default = "global.azure-devices-provisioning.net",
            HelpText = "The global endpoint for devices to connect to.")]
        public string? GlobalDeviceEndpoint { get; set; }

        [Option(
            't',
            "TransportType",
            Default = TransportType.Mqtt,
            HelpText = "The transport to use to communicate with the device provisioning instance. Possible values include Mqtt, Mqtt_WebSocket_Only, Mqtt_Tcp_Only, Amqp, Amqp_WebSocket_Only, Amqp_Tcp_only, and Http1.")]
        public TransportType TransportType { get; set; }
    }

    public enum EnrollmentType
    {
        /// <summary>
        ///  Enrollment for a single device.
        /// </summary>
        Individual,

        /// <summary>
        /// Enrollment for a group of devices.
        /// </summary>
        Group,
    }
}
