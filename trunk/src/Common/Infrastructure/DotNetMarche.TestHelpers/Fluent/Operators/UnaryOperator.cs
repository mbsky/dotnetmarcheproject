using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework.Constraints;

namespace DotNetMarche.TestHelpers.Fluent.Operators
{
	/// <summary>
	/// Classe che decora un constraint, significa che aggiunge alcune funzionalità
	/// al constraint base.
	/// </summary>
	public abstract class UnaryOperator : Constraint  {

		protected Constraint BaseConstraint {
			get { return baseConstraint; }
			set { baseConstraint = value; }
		}
		protected Constraint baseConstraint;

		public virtual Constraint SetConstraint(Constraint constraint) {
			baseConstraint = constraint;
			return this;
		}
	}
}