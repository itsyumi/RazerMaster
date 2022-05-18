using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RazerMasterLibrary.Models;
using RazerMasterLibrary.Repositories.Interface;
using RazerMasterLibrary.Repositories;

namespace RazerMasterAdmin.Services
{
    public class ProductTagService
    {
        private IGenericRepository<ProductTags> repository = new GenericRepository<ProductTags>();

        public OperationResult Create(ProductTags instance)
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

        public OperationResult Update(ProductTags instance)
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


        public ProductTags GetByID(int? tagID)
        {
            return repository.Get(x => x.TagID == tagID);

        }
        public List<ProductTags> GetAll()
        {
            var result = repository.GetAll().ToList();
            return result;
        }
    }
}