using System.Collections.Generic;

namespace Data
{
    public abstract class Bundle
    {
        public Bundle(string name, List<Product> productList, int value)
        {
            this.Name = name;
            this.Value = value;
            this.ProductList = productList;
        }

        public string Name { get; set; }

        public int Value { get; set; }

        public List<Product> ProductList { get; set; }

        public string CheckIfSuitsForCustomer(Customer customer)
        {
            var notSuitableProductList = string.Empty;

            foreach (var product in ProductList)
            {
                var errorList = product.CheckIfSuitsForBundle(customer);

                if (!string.IsNullOrEmpty(errorList))
                {
                    notSuitableProductList += (notSuitableProductList == string.Empty ? string.Empty : ", ") + string.Format("{0} (rules not satisfied: {1})", product.Name, errorList);
                }
            }

            return notSuitableProductList;
        }
    }
}
