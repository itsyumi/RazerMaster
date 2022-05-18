using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RazerMasterLibrary.Models;
using RazerMasterLibrary.Repositories.Interface;
using RazerMasterLibrary.Repositories;

namespace RazerMasterAdmin.Services
{
    public class ProductService
    {

        private IGenericRepository<Products> repository = new GenericRepository<Products>();


        public OperationResult Create(Products instance)
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

        public OperationResult Update(Products instance)
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
      

        public Products GetByID(int? productID)
        {
            return repository.Get(x => x.ProductID == productID);

        }

        public IEnumerable<Products> GetAll()
        {
            return repository.GetAll();
             
        }


    }
}