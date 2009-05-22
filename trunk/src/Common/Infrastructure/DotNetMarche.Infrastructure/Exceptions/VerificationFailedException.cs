using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DotNetMarche.Infrastructure.Helpers;

namespace DotNetMarche.Infrastructure.Exceptions
{
	/// <summary>
	/// Exception throw from a failed verification. <see cref="Verify"/>
	/// </summary>
	[Serializable]
	public class VerificationFailedException : DotNetMarcheInfrastructureException
	{
		/// <summary>
		/// Creates a  <see cref="DotNetMarcheInfrastructureException"/> exception instance.
		/// </summary>
		public VerificationFailedException()
		{
		}

		/// <summary>
		/// Creates a  <see cref="DotNetMarcheInfrastructureException"/> exception instance.
		/// </summary>
		/// <param name="message">Message of the exception</param>
		public VerificationFailedException(string message) : base(message)
		{
		}

		/// <summary>
		/// Creatse a  <see cref="DotNetMarcheInfrastructureException"/> exception instance.
		/// </summary>
		/// <param name="message">Message of the exception</param>
		/// <param name="innerException">Inner exception.</param>
		public VerificationFailedException(string message, Exception innerException) : base(message, innerException)
		{
		}
		/// <summary>
		/// Create a  <see cref="DotNetMarcheInfrastructureException"/> exception instance.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination. </param>
		protected VerificationFailedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
