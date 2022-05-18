using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RazerMasterLibrary.Models;
using RazerMasterLibrary.Repositories;
using RazerMasterLibrary.Repositories.Interface;

namespace RazerMaster.Services
{
    public class AdvertisementService
    {

        private IGenericRepository<Advertisements> _adRepository;
        public AdvertisementService()
        {
            _adRepository = new GenericRepository<Advertisements>();
        }

        public List<Advertisements> GetAll()
        {
            return _adRepository.GetAll().Where(x => x.Status == 1 && x.StartTime <= DateTime.Now && x.EndTime >= DateTime.Now).ToList();

        }
    }
}