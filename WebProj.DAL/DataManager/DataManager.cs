using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebProj.Common;
using WebProj.Common.Models;
using WebProj.Common.Persister;

namespace WebProj.DataManager
{
	public class DataManager
	{
		private static DataManager instance;

		private Dictionary<EntityType, object> cache = new Dictionary<EntityType, object>();
		private Dictionary<Type, IPersister> persisterMap = new Dictionary<Type, IPersister>();

		public static DataManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new DataManager();
				}

				return instance;
			}
		}

		UserPersister userPersister;
		AddressPersister addressPersister;
		VehiclePersister vehiclePersister;
		LocationPersister locationPersister;
		DriverPersister driverPersister;
		FarePersister farePersister;
		CommentPersister commentPersister;

		private DataManager()
		{
			// TODO [CKP] inicijalizuj perzistere
			userPersister = new UserPersister();
			addressPersister = new AddressPersister();
			vehiclePersister = new VehiclePersister();
			locationPersister = new LocationPersister();
			driverPersister = new DriverPersister();
			farePersister = new FarePersister();
			commentPersister = new CommentPersister();

			cache.Add(EntityType.USER, userPersister.Collection);
			cache.Add(EntityType.ADDRESS, addressPersister.Collection);
			cache.Add(EntityType.VEHICLE, vehiclePersister.Collection);
			cache.Add(EntityType.LOCATION, locationPersister.Collection);
			cache.Add(EntityType.DRIVER, driverPersister.Collection);
			cache.Add(EntityType.COMMENT, commentPersister.Collection);
			cache.Add(EntityType.FARE, farePersister.Collection);

			persisterMap.Add((userPersister as IPersister).PersisterType, userPersister as IPersister);
			persisterMap.Add((addressPersister as IPersister).PersisterType, addressPersister as IPersister);
			persisterMap.Add((vehiclePersister as IPersister).PersisterType, vehiclePersister as IPersister);
			persisterMap.Add((locationPersister as IPersister).PersisterType, locationPersister as IPersister);
			persisterMap.Add((driverPersister as IPersister).PersisterType, driverPersister as IPersister);
			persisterMap.Add((commentPersister as IPersister).PersisterType, commentPersister as IPersister);
			persisterMap.Add((farePersister as IPersister).PersisterType, farePersister as IPersister);
		}

		public List<T> GetAllByType<T>()
		{
			EntityType type = GetEntityTypeFromType(typeof(T));
			return new List<T>((cache[type] as Dictionary<long, T>).Values);
		}

		private EntityType GetEntityTypeFromType(Type type)
		{
			if (type == typeof(User)) return EntityType.USER;
			if (type == typeof(Driver)) return EntityType.DRIVER;
			if (type == typeof(Address)) return EntityType.ADDRESS;
			if (type == typeof(Comment)) return EntityType.COMMENT;
			if (type == typeof(Fare)) return EntityType.FARE;
			if (type == typeof(Location)) return EntityType.LOCATION;
			return EntityType.VEHICLE;
		}

		public T GetById<T>(long id) where T : Entity
		{
			if (id > 0)
			{
				if ((cache[(EntityType)(id >> 48)] as Dictionary<long, T>).ContainsKey(id))
				{
					return (cache[(EntityType)(id >> 48)] as Dictionary<long, T>)[id];
				}
			}
			return null;
		}

		public D GetRelatedEntity<S, D>(long sourceId, string propertyName) where S : Entity where D : Entity
		{
			if (sourceId > 0)
			{
				if ((cache[(EntityType)(sourceId >> 48)] as Dictionary<long, S>).ContainsKey(sourceId))
				{
					S source = (cache[(EntityType)(sourceId >> 48)] as Dictionary<long, S>)[sourceId];
					PropertyInfo pi = source.GetType().GetProperty(propertyName);
					if(pi != null && pi.PropertyType == typeof(long))
					{
						long destinationId = (long)pi.GetValue(source);
						if (destinationId > 0)
						{
							if ((cache[(EntityType)(destinationId >> 48)] as Dictionary<long, D>).ContainsKey(destinationId))
							{
								return (cache[(EntityType)(destinationId >> 48)] as Dictionary<long, D>)[destinationId];
							}
						}
					}
				}
			}
			return null;
		}

		public void AddNewEntity<T>(T entity) where T : Entity
		{
			if (!(cache[(EntityType)(entity.Id >> 48)] as Dictionary<long, T>).ContainsKey(entity.Id))
			{
				(cache[(EntityType)(entity.Id >> 48)] as Dictionary<long, T>).Add(entity.Id, entity);
				persisterMap[typeof(T)].AddEntity<T>(entity);
			}
		}

		//internal void AddNewDriver(Driver driver)
		//{
		//	AddNewEntity<User>(driver as User);
  //          if (!(cache[EntityType.DRIVER] as Dictionary<long, Driver>).ContainsKey(driver.Id))
		//	{
		//		(cache[EntityType.DRIVER] as Dictionary<long, Driver>).Add(driver.Id, driver);
		//		persisterMap[typeof(Driver)].AddEntity<Driver>(driver);
		//	}
		//}

		public void UpdateEntity<T>(T entity) where T : Entity
		{
			(cache[(EntityType)(entity.Id >> 48)] as Dictionary<long, T>)[entity.Id] = entity;
			persisterMap[typeof(T)].UpdateEntity<T>(entity);
		}
		public bool ValidateInputData<T>(T entity)
		{
			return persisterMap[typeof(T)].ValidateInput(entity);
		}

		public bool ValidateUserLoginData(string username, string pass,out string report, out User loggedinUser)
		{
			bool retVal = true;
			User u;
			report = string.Empty;
			try
			{
				u = (cache[EntityType.USER] as Dictionary<long, User>).Values.First(user => string.Compare(user.UserName, username) == 0);
			}
			catch
			{
				u = (cache[EntityType.DRIVER] as Dictionary<long, Driver>).Values.FirstOrDefault(user => string.Compare(user.UserName, username) == 0);
			}

			if (u == null)
			{
				retVal = false;
				report = "Invalid username";
            }
			else if (string.Compare(pass, u.Password) != 0)
			{
				retVal = false;
				report = "Invalid password";
			}
			else if(u.Blocked)
			{
				retVal = false;
				report = "User is blocked";
			}
			loggedinUser = u;
			return retVal;
		}

	}
}
