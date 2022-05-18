using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RazerMasterLibrary.Models;
using RazerMasterLibrary.Repositories.Interface;
using RazerMasterLibrary.Repositories;
using RazerMaster.ViewModels;

namespace RazerMaster.Services
{
    public class ProductService
    {
        private IGenericRepository<Products> _productRepository;
        private IGenericRepository<ProductTags> _productTagRepository;
        private IGenericRepository<Categories> _categoryRepository;
        public ProductService()
        {
            _productRepository = new GenericRepository<Products>();
            _productTagRepository = new GenericRepository<ProductTags>();
            _categoryRepository = new GenericRepository<Categories>();
        }


        public Products GetByID(int? productID)
        {
            return _productRepository.Get(x => x.ProductID == productID);

        }

        public IEnumerable<Products> GetAll()
        {
            return _productRepository.GetAll().Where(x => x.Status == 1 && x.StartSellTime <= DateTime.Now && x.EndSellTime >= DateTime.Now).OrderBy(x => x.Sequence).ToList();

        }

        public ProductTags GetProductTagById(int? id)
        {
            return _productTagRepository.GetAll().Where(c => c.TagID == id && c.Status ==1).FirstOrDefault();
        }

        public List<ProductViewModel> SearchProduct(string keyword)
        {
            return _productRepository.GetAll().Where(p => p.ProductName.Contains(keyword) && p.StartSellTime <= DateTime.Now && p.EndSellTime >= DateTime.Now)
            .OrderBy(x => x.Sequence).ToList().Select(p => new ProductViewModel
            {
                ProductID = p.ProductID,
                Picture = p.Picture,
                ProductName = p.ProductName,
                OriginalPrice = p.OriginalPrice,
                SalePrice = p.SalePrice,
                CategoryID = p.CategoryID,
                Stock = p.Stock,
                TagID = p.TagID
            }).ToList();
        }

        public List<ProductTags> GetProductTagList()
        {
            return _productTagRepository.GetAll().Where(x => x.Status == 1).ToList();
        }

        public List<Categories> GetCategoryList()
        {
            return _categoryRepository.GetAll().Where(x => x.Status == 1).OrderBy(x => x.Sequence).ToList();
        }

        public ProductIndexViewModel GetTotalProductList(IEnumerable<Products> products, IEnumerable<Categories> categories, IEnumerable<ProductTags> productTags)
        {
            return new ProductIndexViewModel
            {
                ProductCollection = products,
                CategoryCollection = categories,
                ProductTagCollection = productTags
            };
        }

        public SearchProductViewModel GetSearchProductList(IEnumerable<ProductViewModel> products, IEnumerable<ProductTags> productTags)
        {
            return new SearchProductViewModel
            {
                products = products,
                productTags = productTags
            };
        }

        public ProductDetailViewModel GetProductDetail(Products product, ProductTags productTag,IEnumerable<CommentViewModel> comments)
        {
            return new ProductDetailViewModel
            {
                product = product,
                productTag = productTag,
                comments = comments
            };
        }

    }
}