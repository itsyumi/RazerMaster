using RazerMasterLibrary.DbContextFactory.Interfaces;
using RazerMasterLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazerMasterLibrary.DbContextFactory
{
	public class DbContextFactory : IDbContextFactory
	{
		private RazerMasterDataContext dbContext;

		public DbContextFactory()
		{
		}

		private RazerMasterDataContext DBContext
		{
			get
			{
				if (this.dbContext == null)
				{
					Type type = typeof(RazerMasterDataContext);
					this.dbContext = (RazerMasterDataContext)Activator.CreateInstance(type);
				}
				return dbContext;
			}
		}

		public RazerMasterDataContext GetDbContext()
		{
			return DBContext;
		}
	}
}
