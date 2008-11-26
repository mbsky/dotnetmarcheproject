using System;

namespace NHibernate.Linq.Tests.Entities
{
	public class User
	{
		private string name;
		private DateTime registeredAt;
        private DateTime? lastLoginDate;

		public User()
		{
		}

		public User(string name, DateTime registeredAt)
		{
			this.name = name;
			this.registeredAt = registeredAt;
		}

		public virtual DateTime RegisteredAt
		{
			get { return registeredAt; }
			set { registeredAt = value; }
		}

        public virtual DateTime? LastLoginDate
        {
            get { return lastLoginDate; }
            set { lastLoginDate = value; }
        }

		public virtual int Id { get; set; }

		public virtual string Name
		{
			get { return name; }
			set { name = value; }
		}
	}
}
