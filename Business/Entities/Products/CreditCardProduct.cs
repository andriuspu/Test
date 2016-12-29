using Business;

namespace Data
{
    public class CreditCardProduct : Product
    {
        public CreditCardProduct(int id, string name) : base(id, name)
        {
        }

        public override string CheckIfSuitsForBundle(Customer customer)
        {
            var errorList = string.Empty;

            if (!(customer.IncomeId > (int)IncomeEnum.FromOneToTwelveThousand))
            {
                errorList = Globalization.Rule.RuleIncomeMoreThan12000;
            };

            if (!(customer.AgeId > (int)AgeEnum.FromZeroToSeventeen))
            {
                if (errorList == string.Empty)
                {
                    errorList = Globalization.Rule.RuleAgeMoreThan17;
                }
                else
                {
                    errorList = errorList + ", " + Globalization.Rule.RuleAgeMoreThan17;
                }                
            };

            return errorList;
        }
    }
}
