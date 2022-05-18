using RazerMasterLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazerMasterLibrary.DbContextFactory.Interfaces
{
	public interface IDbContextFactory
	{
		RazerMasterDataContext GetDbContext();
	}
}
