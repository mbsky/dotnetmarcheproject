using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;
using NHibernate.Linq;
using NHibernate.Linq2NhBySql.SqlClient;

namespace ExpressionTreeDumper
{
    public partial class Dumper : Form
    {
        private DumpVisitor mDumpVisitor = new DumpVisitor();
        private PostfixDumpVisitor mPostfixDumpVisitor = new PostfixDumpVisitor();

        public Dumper()
        {
            InitializeComponent();
            
            listBox1.DataSource = CreateExpressionList();
            listBox1.SelectedIndices.Clear();
        }

        private List<KeyValuePair<String, Expression>> CreateExpressionList()
        {
            List<KeyValuePair<String, Expression>> exps =
                new List<KeyValuePair<string, Expression>>();
            Expression<Func<Int32, Int32>> e1 = (Int32 a) => 3;
            exps.Add(new KeyValuePair<String, Expression>("a => 3", e1));
            e1 = (Int32 a) => a;
            exps.Add(new KeyValuePair<string, Expression>("a => a", e1));
            Expression<Func<Int32, Int32, Int32>> ex2 = (Int32 a, Int32 b) => a + b;
            exps.Add(new KeyValuePair<string, Expression>("(a, b) => a + b", ex2));
            //Customer customer;
            Expression<Func<Customer, String>> fieldAcc;
            fieldAcc = (customer => customer.field);
            exps.Add(new KeyValuePair<string, Expression>("(customer => customer.field)", fieldAcc));
            Expression<Func<Customer, Boolean>> fieldAcc2 = customer => customer.Property.StartsWith("A");
            exps.Add(new KeyValuePair<string, Expression>("customer => customer.Property.StartsWith(\"A\")", fieldAcc2));

            Expression<Func<NHibernate.Linq.Tests.Entities.User, Boolean>> w1 =
                user => user.Name == "ayende";
            exps.Add(new KeyValuePair<string, Expression>("user => user.Name == \"ayende\"", w1));

            Expression<Func<Northwind.Entities.Category, Boolean>> any1 =
               c => c.Products.Cast<Northwind.Entities.Product>().Any(p => p.Discontinued);
            exps.Add(new KeyValuePair<string, Expression>("c.Products.Cast<Product>().Any(p => p.Discontinued)", any1));


            Expression<Func<Northwind.Entities.Category, Boolean>> any2 =
                c => c.Products.Cast<Northwind.Entities.Product>().Any();
            exps.Add(new KeyValuePair<string, Expression>("c.Products.Cast<Product>().Any()", any2));
            //NHibernate.ISession session;
            //Expression exp = from user in session.Linq<NHibernate.Linq.Tests.Entities.User>()
            //                 select user.Name;
            //exps.Add(new KeyValuePair<string, Expression>("user.RegisteredAt == d", w2));

            DateTime d = DateTime.Now;
            Expression<Func<NHibernate.Linq.Tests.Entities.User, Boolean>> w2 =
                user => user.RegisteredAt == d;
            exps.Add(new KeyValuePair<string, Expression>("user.RegisteredAt == d", w2));

            NHibernate.Linq.Tests.Entities.NorthwindContext db = new NHibernate.Linq.Tests.Entities.NorthwindContext(null);
            Expression<Func<Northwind.Entities.Employee, Boolean>> e2 =
               e => db.Methods.Substring(e.FirstName, 1, 2) == "An";
            exps.Add(new KeyValuePair<string, Expression>("db.Methods.Substring(e.FirstName, 1, 2) == \"An\"", e2));

            Expression<Func<NHibernate.Linq.Tests.Entities.User, Boolean>> w3 =
               user => user.RegisteredAt == new DateTime(2000, 1, 1);
            exps.Add(new KeyValuePair<string, Expression>("user.RegisteredAt == new DateTime(2000, 1, 1)", w3));
            Expression<Func<NHibernate.Linq.Tests.Entities.User, Boolean>> w4 =
             user => user.LastLoginDate != null;
            exps.Add(new KeyValuePair<string, Expression>("user.LastLoginDate != null", w4));


            Expression<Func<NHibernate.Linq.Tests.Entities.EntityTest, Boolean>> w5 =
          ent => ent.Container.PStr == "Alkampfer";
            exps.Add(new KeyValuePair<string, Expression>("ent.Encontained.PStr == \"Alkampfer\"", w5));

            Expression<Func<Northwind.Entities.Employee, Boolean>> linq2NH1 =
                e => db.Methods.Month(e.BirthDate) == 1;
            exps.Add(new KeyValuePair<string, Expression>("e => db.Methods.Month(e.BirthDate) == 1", linq2NH1));

            DateTime dt = new DateTime(2007, 1, 1);
            linq2NH1 = e => db.Methods.Month(e.BirthDate) == dt.Month;
            exps.Add(new KeyValuePair<string, Expression>("e => db.Methods.Month(e.BirthDate) == dt.Month", linq2NH1));



            return exps;
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            visualizer.ClearExpressions();
            for (Int32 I = 0; I < listBox1.SelectedIndices.Count; ++I) {
                KeyValuePair<String, Expression> element =
                    (KeyValuePair<String, Expression>)listBox1.Items[listBox1.SelectedIndices[I]];
                visualizer.AddExpression(element.Value);
            }
        }



    }
}
