using Business;

namespace Data
{
    public class StudentAccountProduct : Product
    {
        public StudentAccountProduct(int id, string name) : base(id, name)
        {
        }

        public override string CheckIfSuitsForBundle(Customer customer)
        {
            var errorList = string.Empty;

            if (!(customer.IsStudent))
            {
                errorList = Globalization.Rule.RuleIsStudent;
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
