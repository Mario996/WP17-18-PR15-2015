using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common;
using WebProj.Common.Utils;

namespace WebProj.Common.Models
{
	public abstract class Entity
	{
		protected readonly char SEPARATOR = '|';

		long id;

		public long Id
		{
			get
			{
				return id;
			}
			protected set
			{
				id = value;
			}
		}

		public Entity(EntityType type)
		{
			this.id = IdHelper.Instance.GenerateId(type);
        }

		public Entity(Entity other)
		{
			this.id = other.Id;
		}

		public Entity()
		{

		}

		public override string ToString()
		{
			return $"{id}{SEPARATOR}";
		}
	}
}
