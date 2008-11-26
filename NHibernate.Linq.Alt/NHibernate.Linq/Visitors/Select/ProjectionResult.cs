using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NHibernate.Linq.Visitors.Select
{
    /// <summary>
    /// Abstract class that handle projection.
    /// </summary>
    abstract class ProjectionResult
    {
        /// <summary>
        /// function that get a value from a projected result.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract Object GetValue(Int32 index);
    }

    /// <summary>
    /// This class enumerate the result of the projection retrieving
    /// the real object T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class ProjectionReader<T> : ProjectionResult,  IEnumerable<T>, IEnumerable {

        private Func<ProjectionResult, T>   projector;
        private IList projectedResult;      
        internal ProjectionReader(IList projectedResult, Func<ProjectionResult, T> projector) {
            this.projector = projector;
            this.projectedResult = projectedResult; 
        }

        private Int32 currentTupleIndex = 0;
        public IEnumerator<T> GetEnumerator()
        {
            while (currentTupleIndex < projectedResult.Count) {
                yield return projector(this);
                currentTupleIndex++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override object GetValue(int index)
        {
            IList value = projectedResult[currentTupleIndex] as IList;

            return value == null ? projectedResult[currentTupleIndex] : value[index];
        }
    }

}
