using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebProj.Common.Models;

namespace WebProj.Common.Persister
{
	public abstract class Persister<T> : IPersister where T : Entity
	{
		protected string path;

		private readonly char SEPARATOR = '|';

		private Dictionary<long, T> collection = new Dictionary<long, T>(64);
		public Dictionary<long, T> Collection
		{
			get
			{
				return new Dictionary<long, T>(collection);
			}
		}

		public Type PersisterType
		{
			get
			{
				return typeof(T);
			}
		}

		public Persister()
		{
			//LoadData();
		}

		public Persister(string path)
		{
			this.path = path;
			LoadData();
		}

		private void LoadData()
		{
			try
			{
				string tempPath = Path.GetTempPath() + path;

				if (!File.Exists(tempPath))
				{
					FileStream fs = File.Create(tempPath);
					fs.Close();
				}
				using (TextReader reader = new StreamReader(tempPath))
				{
					string line = string.Empty;
					while ((line = reader.ReadLine()) != null)
					{
						string[] splited = line.Split(SEPARATOR);
						T entity = InstantiateEntity(splited);
						collection.Add(entity.Id, entity);
					}
				}
			}
			catch (Exception ex)
			{
				// TODO [CKP] implement
			}
		}

		private void SaveData()
		{
			//TaskFactory f = new TaskFactory();
			//f.StartNew(() =>
			//{
				try
				{
					string tempPath = Path.GetTempPath() + path;
					if (!File.Exists(tempPath))
					{
						FileStream fs = File.Create(tempPath);
						fs.Close();
					}
					using (TextWriter writer = new StreamWriter(tempPath, false))
					{
						foreach (T entity in Collection.Values)
						{
							writer.Write($"{entity.ToString()}{Environment.NewLine}");
						}
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
			//}
			//);
		}

		protected abstract T InstantiateEntity(string[] splited);

		public void AddEntity<T1>(T1 entity) where T1 : Entity
		{
			collection.Add(entity.Id, entity as T);
			SaveData();
        }

		public bool ValidateInput<T1>(T1 entity)
		{
			return ValidateInput(entity as T);
		}

		protected abstract bool ValidateInput(T entity);

		public void UpdateEntity<T1>(T1 entity) where T1 : Entity
		{
			collection[entity.Id] = entity as T;
			SaveData();
		}
	}
}