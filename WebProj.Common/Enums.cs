using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProj.Common
{
	public enum EntityType : short
	{
		USER = 0x01,
		LOCATION = 0x2,
		ADDRESS = 0x3,
		VEHICLE = 0x4,
		FARE = 0x5,
		DRIVER = 0x6,
		COMMENT = 0x7,
	}

	public enum Gender : short
	{
		MALE = 0x01,
		FEMALE = 0x02,
	}

	public enum UserType : short
	{
		DISPATCHER = 0x01,
		DRIVER = 0x02,
		CUSTOMER = 0x03,
	}

	public enum VehicleType : short
	{
		PASSENGER_CAR = 0x01,
		VAN = 0x02
	}

	public enum FareStatus : short
	{
		ON_HOLD = 0x01,
		FORMED =0x02,
		PROCESSED = 0x03,
		ACCEPTED = 0x04,
		CANCELED = 0x05,
		FAILED = 0x06,
		SUCCESFUL = 0x07,
	}
}
