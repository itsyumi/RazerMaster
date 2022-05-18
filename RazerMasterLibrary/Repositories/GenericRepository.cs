using RazerMasterLibrary.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using RazerMasterLibrary.Models;
using System.Data.Entity.Migrations;
using RazerMasterLibrary.DbContextFactory.Interfaces;
using RazerMasterLibrary.UnitOfWork.Interface;

namespace RazerMasterLibrary.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public DbContext _context { get; set; }
        public IUnitOfWork _UnitOfWork { get; set; }
        private DbSet<TEntity> _dbSet { get; set; }
        public GenericRepository()
        {
            this._context = new RazerMasterDataContext();
        }

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            this._UnitOfWork = unitOfWork;
            this._context = unitOfWork.Context;
            this._dbSet = unitOfWork.Context.Set<TEntity>();
        }

        public void Create(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                this._context.Set<TEntity>().Add(instance);
                this.SaveChanges();
            }
        }

        public void Update(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                //this._context.Entry(instance).State = EntityState.Modified;
                this._context.Set<TEntity>().AddOrUpdate(instance);
                this.SaveChanges();
            }
        }

        public void Delete(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                this._context.Set<TEntity>().Remove(instance);
                //this._context.Set<TEntity>().AddOrUpdate(instance);
                this.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return this._context.Set<TEntity>().FirstOrDefault(predicate);

        }

        public IQueryable<TEntity> GetAll()
        {
            return this._context.Set<TEntity>().AsQueryable();
        }

        public void SaveChanges()
        {
            this._context.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._context != null)
                {
                    this._context.Dispose();
                    this._context = null;
                }
            }
        }
    }
}
