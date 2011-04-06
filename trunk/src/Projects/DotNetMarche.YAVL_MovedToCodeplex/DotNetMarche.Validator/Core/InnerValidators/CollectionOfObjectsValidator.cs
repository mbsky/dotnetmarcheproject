//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using DotNetMarche.Validator.BaseClasses;
//using DotNetMarche.Validator.Interfaces;

//namespace DotNetMarche.Validator.Core.InnerValidators
//{
//   public class CollectionOfObjectsValidator : ObjectValidator
//   {
//      internal CollectionOfObjectsValidator(
//         IValueExtractor valueExtractor, 
//         Dictionary<Type, ValidationUnitCollection> ruleMap) : base(valueExtractor, ruleMap)
//      {
//      }

//      public override IEnumerable<SingleValidationResult> Validate(object objectToValidate, ValidationFlags validationFlags)
//      {
//         Object obj = Extract<Object>(objectToValidate);
//         //This should be some form of IEnumerable object.
//         IEnumerable list = obj as IEnumerable;
//         Int32 index = 0;
//         foreach (Object innerObjectToValidate in list)
//         {

//            index++;
//         }
//      }
//   }
//}
