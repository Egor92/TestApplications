using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace NHibernateDetachedCriteriaTest
{
    public abstract class DbObject
    {
        public int ID { get; set; }
    }

    public class House : DbObject
    {
        
    }

    public class PersonHouse : DbObject
    {
        public int PersonID { get; set; }
        public int HouseID { get; set; }
    }

    public class Person : DbObject
    {

    }

    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new NHibernate.Cfg.Configuration();
            configuration.Properties.Add("use_proxy_validator", "false");
            configuration.Properties.Add("connection.connection_string", string.Format("Data Source={0};Version=3;Compress=True", "test.sqlite"));
            configuration.Properties.Add("dialect", "NHibernate.Dialect.SQLiteDialect");
            configuration.Properties.Add("adonet.batch_size", "250");
            var sessionFactory = configuration.BuildSessionFactory();
            using (var session = sessionFactory.OpenSession())
            {
                ICriteria crit = session.CreateCriteria<Person>();
                DetachedCriteria roleIdsForSite = DetachedCriteria.For<PersonHouse>()
                    .SetProjection(Projections.Id())
                    .CreateCriteria("Sites", "site")
                    .Add(Restrictions.Eq("site.Id", 123));
                /*DetachedCriteria userIdsForRoles = DetachedCriteria.For(typeof(User))
                    .SetProjection(Projections.Distinct(Projections.Property("Id")))
                    .CreateCriteria("Roles")
                        .Add(Subqueries.PropertyIn("Id", roleIdsForSite));
                crit.Add(Subqueries.PropertyIn("Id", userIdsForRoles));*/
            }
        }
    }
}
