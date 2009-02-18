using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Linq.Tests.Entities
{
    public class EntityContainer
    {
        public Int32 Id { get; set; }
        public DateTime RegDate { get; set; }
        public String PStr { get; set; }
        public IList<EntityTest> tests;
        public IList<EntityTest> Tests
        {
            get { return tests ?? (tests = new List<EntityTest>()); }
            set { tests = value; }
        }
    }
}
