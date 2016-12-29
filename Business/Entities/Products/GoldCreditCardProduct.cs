using Business;

namespace Data
{
    public class GoldCreditCardProduct : Product
    {
        public GoldCreditCardProduct(int id, string name) : base(id, name)
        {
        }

        public override string CheckIfSuitsForBundle(Customer customer)
        {
            var errorList = string.Empty;

            if (!(customer.IncomeId > (int)IncomeEnum.FromTwelveThousandOneToFortyThousand))
            {
                errorList = Globalization.Rule.RuleIncomeMoreThan40000;
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
