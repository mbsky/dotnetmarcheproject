//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using DotNetMarche.Validator.BaseClasses;
//using DotNetMarche.Validator.Interfaces;

//namespace DotNetMarche.Validator.Validators.Concrete
//{
//   /// <summary>
//   /// This represent a rule that validate with a custom action 
//   /// expressed with a lambda or delegate.
//   /// </summary>
//   public class NotEmptyValidator  : BaseValidator  
//   {
//      public NotEmptyValidator(IValueExtractor valueExtractor)
//         : base(valueExtractor)
//      {

//      }

//      public override SingleValidationResult Validate(object objectToValidate)
//      {
//         Object valueToCheck = mValueExtractor.ExtractValue(objectToValidate);
//         //Verify if it is some form of a collection, or if it is possibile to iterate
//         //into it.

//      }
//   }
//}
