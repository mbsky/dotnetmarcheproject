//using System;
//using System.Collections;
//using System.Text;
//using NUnit.Framework.Constraints;

//namespace SomeNunitHelpers {
//   public class NunitOneItemConstraint : PrefixConstraint {

//      public NunitOneItemConstraint(Constraint baseConstraint) : base(baseConstraint) {}

//      public override bool Matches(object actual) {
//         base.actual = actual;
//         base.PassModifiersToBase();
//         if (!(actual is ICollection)) {
//            throw new ArgumentException("The actual value must be a collection", "actual");
//         }
//         Int32 MatchCount = 0;
//         foreach (object obj2 in (ICollection)actual) {
//            if (base.baseConstraint.Matches(obj2)) {
//               MatchCount++;
//               if (MatchCount > 1) return false;
//            }
//         }
//         return MatchCount == 1;

//      }

//      public override void WriteDescriptionTo(NUnit.Framework.MessageWriter writer) {
//         writer.WritePredicate("nunitOne items");
//         base.baseConstraint.WriteDescriptionTo(writer);
//      }
//   }
//}
