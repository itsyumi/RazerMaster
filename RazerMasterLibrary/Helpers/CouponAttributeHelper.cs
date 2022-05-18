using RazerMasterLibrary.Models;
using RazerMasterLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RazerMasterLibrary.Helpers
{
    public class UniqueKeyAttribute : ValidationAttribute
    {
        public GenericRepository<Coupons> repo = new GenericRepository<Coupons>();
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString() == "Create" && value != null)
            {


                if (repo.GetAll().Any(x => x.Code == value.ToString()))
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return null;
        }
    }
    public class GreaterThanAttribute : ValidationAttribute
    {
        private readonly string _targetName;

        public GreaterThanAttribute(string targetFieldName)
        {
            this._targetName = targetFieldName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            this.ErrorMessage = this.ErrorMessageString;
            var sourceName = validationContext.DisplayName;
            var sourceValue = (IComparable)value;
            var targetValue = validationContext.ObjectType.GetProperty(this._targetName).GetValue(validationContext.ObjectInstance);

            if (sourceValue.CompareTo((IComparable)targetValue) < 0)
            {
                this.ErrorMessage = $" {sourceName} must be greater than {this._targetName}";
                return new ValidationResult(FormatErrorMessage(sourceName));
            }

            return ValidationResult.Success;
        }

    }
}