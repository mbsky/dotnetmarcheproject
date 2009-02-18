using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Linq.Visitors
{
    enum TokenType
    {
        Unknown = 0,
        
        /// <summary>
        /// A simple container of criterion.
        /// </summary>
        CriterionContainer,

        /// <summary>
        /// This token is a constant and contains an object
        /// </summary>
        ConstantObject,

        /// <summary>
        /// This token contains a property path that can be use to 
        /// build a criterion.
        /// </summary>
        PropertyPath,

        /// <summary>
        /// this token contains an invocation to a sql function.
        /// </summary>
        SqlFunction,

        /// <summary>
        /// Represents a cast function.
        /// </summary>
        Cast,
    }
}
