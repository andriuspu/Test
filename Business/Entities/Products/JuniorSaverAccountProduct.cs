using Business;

namespace Data
{
    public class JuniorSaverAccountProduct : Product
    {
        public JuniorSaverAccountProduct(int id, string name) : base(id, name)
        {
        }

        public override string CheckIfSuitsForBundle(Customer customer)
        {
            var errorList = string.Empty;

            if (!(customer.AgeId < (int)AgeEnum.FromEighteenToSixtyFour))
            {
                errorList = Globalization.Rule.RuleAgeLessThan18;
            };

            return errorList;
        }
    }
}
