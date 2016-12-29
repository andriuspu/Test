using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using Dapper;

namespace Data
{
    public class BundlesRepository
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["SqlServerConnString"] != null
            ? ConfigurationManager.ConnectionStrings["SqlServerConnString"].ConnectionString 
            : null;

        public void SaveCustomerAndOfferedProducts(Customer customer, List<int> productIdList)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                using (var ts = new TransactionScope())
                {
                    try
                    {
                        var customerQuery = @"Insert into Customer values (@Name, @AgeId, @IncomeId, @IsStudent);Select Cast (Scope_Identity() as int)";
                        int id = db.Query<int>(customerQuery, customer).Single();

                        var customerProductQuery = @"Insert into CustomerProduct values (@CustomerId, @ProductId)";
                        foreach (var productId in productIdList)
                        {
                            db.Query(customerProductQuery, new { CustomerId = id, ProductId = productId });
                        }

                        ts.Complete();
                    }

                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        public Customer GetCustomerWithProducts(int customerId)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    var lookup = new Dictionary<int, Customer>();

                    var test = db.Query<Customer, CustomerProduct, Customer>(@"
                                            SELECT c.*, cp.*
                                            FROM Customer c
                                            INNER JOIN CustomerProduct cp ON c.Id = cp.CustomerId WHERE c.Id = @Id",
                    (s, a) =>
                    {
                        Customer customer;
                        if (!lookup.TryGetValue(s.Id, out customer))
                        {
                            lookup.Add(s.Id, customer = s);
                        }

                        if (customer.CustomerProducts == null)
                        {
                            customer.CustomerProducts = new List<CustomerProduct>();
                        }

                        customer.CustomerProducts.Add(a);
                        return customer;
                    },
                    new { Id = customerId }).AsQueryable();

                    return lookup.Values.FirstOrDefault();
                }

                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void InsertProductForCustomer(int customerId, int newProductId)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                using (var ts = new TransactionScope())
                {
                    try
                    {
                        var customerProductQuery = @"Insert into CustomerProduct values (@CustomerId, @ProductId)";
                        db.Query(customerProductQuery, new { CustomerId = customerId, ProductId = newProductId });

                        ts.Complete();
                    }

                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }
    }
}
