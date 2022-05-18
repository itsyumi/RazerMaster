using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RazerMasterLibrary.Models;
using RazerMasterLibrary.Repositories.Interface;
using RazerMasterLibrary.Repositories;

namespace RazerMasterAdmin.Services
{
    public class PromotionSerivce
    {
        private IGenericRepository<Promotions> repository = new GenericRepository<Promotions>();

        public OperationResult Create(Promotions instance)
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

        public OperationResult Update(Promotions instance)
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
     

        public Promotions GetByID(int? evnetID)
        {
            return repository.Get(x => x.EventID == evnetID);
        }
        public List<Promotions> GetAll()
        {
            var result = repository.GetAll().ToList();
            return result;
        }
    }
}