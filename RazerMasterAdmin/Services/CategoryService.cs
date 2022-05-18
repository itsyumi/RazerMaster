using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RazerMasterLibrary.Models;
using RazerMasterLibrary.Repositories.Interface;
using RazerMasterLibrary.Repositories;

namespace RazerMasterAdmin.Services
{
    public class CategoryService
    {
        private IGenericRepository<Categories> repository = new GenericRepository<Categories>();

        public OperationResult Create(Categories instance)
        {
            var result = new OperationResult();

            try
            {
                repository.Create(instance);
                result.IsSuccessful = true;

            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.exception = ex;
            }
            return result;

        }

        public OperationResult Update(Categories instance)
        {
            var result = new OperationResult();

            try
            {
                repository.Update(instance);
                result.IsSuccessful = true;

            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.exception = ex;
            }
            return result;

        }

        public Categories GetByID(int? categoryID)
        {
            return repository.Get(x => x.CategoryID == categoryID);
        }
        public List<Categories> GetAll()
        {
            var result = repository.GetAll().ToList();
            return result;
        }
    }
}