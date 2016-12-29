using System.Collections.Generic;

namespace Data
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AgeId { get; set; }

        public int IncomeId { get; set; }

        public bool IsStudent { get; set; }

        public List<CustomerProduct> CustomerProducts { get; set; }
    }
}
