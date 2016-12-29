using System.Collections.Generic;
using Data;

namespace Business
{
    public class BundleFactory : IBundleFactory
    {
        public Bundle Create(int bundleId)
        {
            switch (bundleId)
            {
                case (int)BundleEnum.JuniorSaver:
                    {
                        var productList = new List<Product>()
                    {
                        new JuniorSaverAccountProduct((int)ProductEnum.JuniorSaverAccount, Globalization.Rule.AccountJuniorSaver)
                    };

                        return new JuniorSaverBundle(Globalization.Rule.BundleJuniorSaver, productList, 0);
                    }
                case (int)BundleEnum.Student:
                    {
                        var productList = new List<Product>()
                    {
                        new StudentAccountProduct((int)ProductEnum.StudentAccount, Globalization.Rule.AccountStudent),
                        new DebitCardProduct((int)ProductEnum.DebitCard, Globalization.Rule.CardDebit),
                        new CreditCardProduct((int)ProductEnum.CreditCard, Globalization.Rule.CardCredit)
                    };

                        return new StudentBundle(Globalization.Rule.BundleStudent, productList, 0);
                    }
                case (int)BundleEnum.Classic:
                    {
                        var productList = new List<Product>()
                    {
                        new CurrentAccountProduct((int)ProductEnum.CurrentAccount, Globalization.Rule.AccountCurrent),
                        new DebitCardProduct((int)ProductEnum.DebitCard, Globalization.Rule.CardDebit)
                    };

                        return new ClassicBundle(Globalization.Rule.BundleClassic, productList, 1);
                    }
                case (int)BundleEnum.ClassicPlus:
                    {
                        var productList = new List<Product>()
                    {
                        new CurrentAccountProduct((int)ProductEnum.CurrentAccount, Globalization.Rule.AccountCurrent),
                        new DebitCardProduct((int)ProductEnum.DebitCard, Globalization.Rule.CardDebit),
                        new CreditCardProduct((int)ProductEnum.CreditCard, Globalization.Rule.CardCredit)
                    };

                        return new ClassicPlusBundle(Globalization.Rule.BundleClassicPlus, productList, 2);
                    }
                case (int)BundleEnum.Gold:
                    {
                        var productList = new List<Product>()
                    {
                        new CurrentAccountPlusProduct((int)ProductEnum.CurrentAccountPlus, Globalization.Rule.AccountCurrentPlus),
                        new DebitCardProduct((int)ProductEnum.DebitCard, Globalization.Rule.CardDebit),
                        new GoldCreditCardProduct((int)ProductEnum.GoldCreditCard, Globalization.Rule.CardGoldCredit)
                    };

                        return new GoldBundle(Globalization.Rule.BundleGold, productList, 3);
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
