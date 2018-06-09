using System;
using WebProj.Common.Persister;
using WebProj.Common.Models;

namespace WebProj.Common.Persister
{
	public interface IPersister
	{ 
		void AddEntity<T>(T entity) where T :Entity;
		void UpdateEntity<T>(T entity) where T : Entity;
		Type PersisterType { get; }

		bool ValidateInput<T>(T entity);
	}
}