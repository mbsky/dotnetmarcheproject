using System;
using System.Collections.Generic;
using DotNetMarche.TestHelpers.Fluent.Expression;
using DotNetMarche.TestHelpers.Fluent.Operators;
using DotNetMarche.Utils.Expressions;
using Nablasoft.Test.UnitTest;
using Nablasoft.Test.UnitTest.Operators;
using NUnit.Framework.Constraints;

namespace DotNetMarche.TestHelpers.Fluent
{
	/// <summary>
	/// This is to accomodate the new strcture, when we deal with a chain of
	/// Constraint objects we does not need to tokenize anything.
	/// </summary>
	internal class DummyTokenizer : ITokenizer<List<Constraint>, Constraint>
	{

		#region ITokenizer<List<Constraint>,Constraint> Members

		public List<Constraint> Tokenize(List<Constraint> expressionSource)
		{
			return expressionSource;
		}

		#endregion

	}

	internal class DummyTokenConverter : ITokenConverter<Constraint, Constraint>
	{

		#region ITokenConverter<Constraint,Constraint> Members

		public Constraint Convert(Constraint source)
		{
			return source;
		}

		#endregion
	}

	public class CollectionConstraintBuilder<T> {

		private Stack<Constraint> constraints = new Stack<Constraint>();
		private IEnumerable<T> sut;

		internal CollectionConstraintBuilder(IEnumerable<T> sut) {
			this.sut = sut;
		}

		private  Constraint GetConstraint() {

			ExpressionConverterExt<List<Constraint>, Constraint, Constraint>  converter = 
				new ExpressionConverterExt<List<Constraint>, Constraint, Constraint>(
					new ConstraintOperatorChecker(), new DummyTokenizer(), new DummyTokenConverter() );
			PostfixConstraintExecutor executor = new PostfixConstraintExecutor();
			List<Constraint> list = new List<Constraint>();
			list.AddRange(constraints);
			list.Reverse();
			IList<Constraint> postfix = converter.InfixToPostfix(list);
			return executor.ResolveExpression(postfix);
		}

		public void Assert() {
			NUnit.Framework.Assert.That(sut, GetConstraint());
		}

		#region Constraint on count

		public CollectionConstraintBuilder<T> None(Func<T, Boolean> discriminator) {
			constraints.Push(new CountWithLambdaConstraint<T>(0, discriminator));
			return this;
		}


		public CollectionConstraintBuilder<T> One(Func<T, Boolean> discriminator) {
			constraints.Push(new CountWithLambdaConstraint<T>(1, discriminator));
			return this;
		}

		public CollectionConstraintBuilder<T> Twice(Func<T, Boolean> discriminator) {
			constraints.Push(new CountWithLambdaConstraint<T>(2, discriminator));
			return this;
		}

		public CollectionConstraintBuilder<T> Count(Int32 numOccurrence, Func<T, Boolean> discriminator) {
			constraints.Push(new CountWithLambdaConstraint<T>(numOccurrence, discriminator));
			return this;
		}

		public CollectionConstraintBuilder<T> All(Func<T, Boolean> discriminator) {
			constraints.Push(new CountWithLambdaConstraint<T>(-1, discriminator));
			return this;
		}

		#endregion

		#region Logical operators

		public CollectionConstraintBuilder<T> And {
			get {
				constraints.Push(new AndOperator());
				return this;
			}
		}

		public CollectionConstraintBuilder<T> Or {
			get {
				constraints.Push(new OrOperator());
				return this;
			}
		}

		public CollectionConstraintBuilder<T> Not {
			get {
				constraints.Push(new NotOperator());
				return this;
			}
		}


		#endregion
	}
}