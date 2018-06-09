using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common;

namespace WebProj.Common.Utils
{
	public class IdHelper
	{
		private static IdHelper instance;

		Dictionary<EntityType, long> countMap = new Dictionary<EntityType, long>(Enum.GetValues(typeof(EntityType)).Length);

		object lockObject = new object();

		public static IdHelper Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new IdHelper();
				}
				return instance;
			}
		}

		private IdHelper()
		{
			LoadConfig();
		}

		private void LoadConfig()
		{
			string tempPath = Path.GetTempPath() + "config.txt";

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
					string[] splited = line.Split(':');
					EntityType t;
					Enum.TryParse(splited[0], out t);
					long cnt;
					Int64.TryParse(splited[1], out cnt);
					countMap.Add(t, cnt);
				}
			}

			if (countMap.Count == 0)
			{
				foreach (var t in Enum.GetValues(typeof(EntityType)))
				{
					countMap.Add((EntityType)t, 0);
				}
			}
		}

		public void SaveConfig()
		{
			//TaskFactory f = new TaskFactory();
			//f.StartNew(() =>
			//{
				string tempPath = Path.GetTempPath() + "config.txt";

				if (!File.Exists(tempPath))
				{
					FileStream fs = File.Create(tempPath);
					fs.Close();
				}
				using (TextWriter writer = new StreamWriter(tempPath, false))
				{
					foreach (var t in countMap)
					{
						writer.Write($"{t.Key}:{t.Value}{Environment.NewLine}");
					}
				}
			//});
		}

		public long GenerateId(EntityType type)
		{
			long retVal = 0;
			lock (lockObject)
			{
				retVal = ((long)type << 48) ^ (++countMap[type]);
				SaveConfig();
			}
			return retVal;
		}

		public EntityType ExtracTypeFromId(long id)
		{
			return (EntityType)(id >> 48);
		}
	}
}
