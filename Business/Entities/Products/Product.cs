using System.Collections.Generic;
using System.Linq;
using Business;

namespace Data
{
    public abstract class Product
    {
        public Product(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public abstract string CheckIfSuitsForBundle(Customer customer);

        public bool ValidateForDuplicateAccount(List<int> listOfAssignedProducts, int newProductId)
        {
            var accountList = new List<int>()
            {
                (int)ProductEnum.CurrentAccount,
                (int)ProductEnum.CurrentAccountPlus,
                (int)ProductEnum.JuniorSaverAccount,
                (int)ProductEnum.StudentAccount
            };

            if (accountList.Contains(newProductId) && listOfAssignedProducts.Intersect(accountList).Any())
            {
                return false;
            }

            return true;
        }
    }
}
