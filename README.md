# IoTCentralRegisterDevice
this project is a reviewed version of the code provided in this article https://www.azuredeveloper.cn/article/how-to-sas-key-group-dps.

This console application in .net core allow you to:
- register an individual device on Device Provisioning Service using Device ID and Sas key
- register a group of device on Device Provisioning Service using Sas key

> iot-central-register-device 1.0.0  
Copyright (C) 2022 iot-central-register-device
  
 	 -s, --IdScope                 Required. The Id Scope of the DPS instance.  	
	 -i, --Id                      Required. The registration Id when using individual enrollment, or the desired device Id when using group enrollment.  
	 -p, --PrimaryKey              Required. The primary key of the individual enrollment or the derived primary key of the group enrollment. See the ComputeDerivedSymmetricKeyGroupSample for how to generate the derived key.  
	 -f, --DevicePrefix            Required. The Device Prefix for all Device on group enrolment.  
	 -n, --number                  (Default: 1) How Many device you want to auto-matic generate.  
	 -e, --EnrollmentType          (Default: Individual) The type of enrollment: Individual or Group.  
	 -g, --GlobalDeviceEndpoint    (Default: global.azure-devices-provisioning.net) The global endpoint for devices to connect to.  
	 -t, --TransportType           (Default: Mqtt) The transport to use to communicate with the device provisioning instance. Possible values include Mqtt, Mqtt_WebSocket_Only, Mqtt_Tcp_Only, Amqp, Amqp_WebSocket_Only, Amqp_Tcp_only, and Http1.  
	 --help                        Display this help screen.  
	 --version                     Display version information.  
  
  
