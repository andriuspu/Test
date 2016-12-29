using System.Collections.Generic;
using System.Linq;
using Business;

namespace Data
{
    public class DebitCardProduct : Product
    {
        public DebitCardProduct(int id, string name) : base(id, name)
        {
        }

        public override string CheckIfSuitsForBundle(Customer customer)
        {
            ////all bundles, except "Junior saver" has account (so the condition is satisfied) and "Junior saver" does not need this product
            return string.Empty;
        }

        public bool CheckIfCanAddToExistingProducts(List<int> listOfAssignedProducts)
        {
            var accountList = new List<int>()
            {
                (int)ProductEnum.CurrentAccount,
                (int)ProductEnum.CurrentAccountPlus,
                (int)ProductEnum.StudentAccount
            };

            if (listOfAssignedProducts.Intersect(accountList).Any())
            {
                return true;
            }

            return false;
        }
    }
}
