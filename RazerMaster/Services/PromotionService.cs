using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RazerMaster.ViewModels;
using RazerMasterLibrary.Models;
using RazerMasterLibrary.Repositories;
using RazerMasterLibrary.Repositories.Interface;

namespace RazerMaster.Services
{
    public class PromotionService
    {
        private IGenericRepository<Promotions> _promotionsRepository;
        public PromotionService()
        {
            _promotionsRepository = new GenericRepository<Promotions>();
        }

        public PromotionViewModel GetAllPromotions()
        {
            var promotions= _promotionsRepository.GetAll().Where(x => x.Status == 1 && x.StartTime <= DateTime.Now && x.EndTime >= DateTime.Now);

            return new PromotionViewModel
            {
                Promotions = promotions
            };
        }

    }
}