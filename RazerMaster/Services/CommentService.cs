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
    public class CommentService
    {
        private IGenericRepository<ProductComments> _commentCRUDRepository;
        private CommentRepository _commentRepository;

        public CommentService()
        {
            _commentCRUDRepository = new GenericRepository<ProductComments>();
            _commentRepository = new CommentRepository();
        }

        public List<CommentViewModel> GetCommentListByProduct(int? productID)
        {
            return _commentRepository.GetCommentListByProduct(productID).Select(c => new CommentViewModel
            {
                NickName = c.NickName,
                Content = c.Content,
                CreateDate = c.CreateDate.ToString("yyyy-MM-dd"),
                Score = c.Score
            }).ToList();
        }


        public List<dynamic> GetRatingScoreByProduct(int? productID)
        {
           
                return _commentRepository.GetRatingScoreByProduct(productID);
        }

        public OperationResult Create(ProductComments instance)
        {
            var result = new OperationResult();

            try
            {
                _commentCRUDRepository.Create(instance);
                result.IsSuccessful = true;

            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.exception = ex;
            }
            return result;

        }
    }
}