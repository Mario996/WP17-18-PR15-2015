using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProj.Common.Models;

namespace WebProj.Common.Persister
{
	class CommentPersister : Persister<Comment>
	{
		public CommentPersister() : base("comments.txt") { }

		protected override Comment InstantiateEntity(string[] splited)
		{
			return new Comment(splited);
		}

		protected override bool ValidateInput(Comment entity)
		{
			throw new NotImplementedException();
		}
	}
}
