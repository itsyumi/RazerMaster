using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazerMasterLibrary.UnitOfWork.Interface
{
	public interface IUnitOfWork : IDisposable
	{
		DbContext Context { get; }

		int SaveChange();
	}
}
