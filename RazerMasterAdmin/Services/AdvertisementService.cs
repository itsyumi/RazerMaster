using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RazerMasterLibrary.Models;
using RazerMasterLibrary.Repositories.Interface;
using RazerMasterLibrary.Repositories;

namespace RazerMasterAdmin.Services
{
    public class AdvertisementService
    {
        private IGenericRepository<Advertisements> repository = new GenericRepository<Advertisements>();

        public OperationResult Create(Advertisements instance)
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

        public OperationResult Update(Advertisements instance)
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

        public Advertisements GetByID(int? AdID)
        {
            return repository.Get(x => x.AdID == AdID);
        }
        public List<Advertisements> GetAll()
        {
            var result = repository.GetAll().ToList();
            return result;
        }
    }
}