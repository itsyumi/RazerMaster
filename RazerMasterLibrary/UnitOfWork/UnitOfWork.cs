using RazerMasterLibrary.DbContextFactory.Interfaces;
using RazerMasterLibrary.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazerMasterLibrary.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private IDbContextFactory _dbContextFactory { get; set; }

		private DbContext context;

		private bool disposed = false;

		public DbContext Context
		{
			get
			{
				if (this.context != null)
				{
					return this.context;
				}

				this.context = _dbContextFactory.GetDbContext();
				return this.context;
			}
		}

		public UnitOfWork(IDbContextFactory dbContextFactory)
		{
			this._dbContextFactory = dbContextFactory;
		}

		public int SaveChange()
		{
			int result = this.Context.SaveChanges();
			return result;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.Context.Dispose();
					this.context = null;
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
