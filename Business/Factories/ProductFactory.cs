using Data;

namespace Business
{
    public class ProductFactory : IProductFactory
    {
        public Product Create(int productId)
        {
            switch (productId)
            {
                case (int)ProductEnum.CurrentAccount:
                    {
                        return new CurrentAccountProduct((int)ProductEnum.CurrentAccount, Globalization.Rule.AccountCurrent);
                    }
                case (int)ProductEnum.CurrentAccountPlus:
                    {
                        return new CurrentAccountPlusProduct((int)ProductEnum.CurrentAccountPlus, Globalization.Rule.AccountCurrentPlus);
                    }
                case (int)ProductEnum.JuniorSaverAccount:
                    {
                        return new JuniorSaverAccountProduct((int)ProductEnum.JuniorSaverAccount, Globalization.Rule.AccountJuniorSaver);
                    }
                case (int)ProductEnum.StudentAccount:
                    {
                        return new StudentAccountProduct((int)ProductEnum.StudentAccount, Globalization.Rule.AccountStudent);
                    }
                case (int)ProductEnum.DebitCard:
                    {
                        return new DebitCardProduct((int)ProductEnum.DebitCard, Globalization.Rule.CardDebit);
                    }
                case (int)ProductEnum.CreditCard:
                    {
                        return new CreditCardProduct((int)ProductEnum.CreditCard, Globalization.Rule.CardCredit);
                    }
                case (int)ProductEnum.GoldCreditCard:
                    {
                        return new GoldCreditCardProduct((int)ProductEnum.GoldCreditCard, Globalization.Rule.CardGoldCredit);
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
