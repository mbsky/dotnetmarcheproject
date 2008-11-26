using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Linq.Tests.Entities
{
    public class EntityTest
    {
        public Int32 Id { get; set; }
        public Int16 PInt16 { get; set; }
        public Int32 PInt32 { get; set; }
        public Int64 PInt64 { get; set; }
        public Byte  PByte { get; set; }
        public Boolean PBool { get; set; }
        public String PString { get; set; }
        private EntityContainer container;
        public EntityContainer Container {
            get { return container; }
            set { container = value; value.Tests.Add(this); }
        }
    }
}
