using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RazerMasterLibrary.Models;
using RazerMasterLibrary.Repositories;
using RazerMasterLibrary.Repositories.Interface;
using RazerMaster.ViewModels;

namespace RazerMaster.Services
{
    public class HomeService
    {
        public HomeService()
        {
            
        }

        public HomeIndexViewModel GetAllDisplayData(IEnumerable<Products> productList1,IEnumerable<Products> productList2,IEnumerable<ProductTags> tagList,IEnumerable<Advertisements> adList)
        {
            return new HomeIndexViewModel
            {
                ProductNewCollection = productList1,
                ProductHotCollection = productList2,
                TagCollection = tagList,
                AdCollection = adList,
            };
        }
    }
}