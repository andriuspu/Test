using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using Bundles.Models;
using Business;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.Controllers.Api;

namespace Bundles.Tests.Controllers
{
    [TestClass]
    public class RuleControllerTest
    {
        Customer CustomerWithJuniorSaverAccount;
        Customer CustomerWithCurrentAccount;
        int ProductDebitCard;
        int ProductCurrentAccount;
        int ProductGoldCreditCard;

        [TestInitialize]
        public void TestInitialize()
        {
            CustomerWithJuniorSaverAccount = new Customer
            {
                IncomeId = (int)IncomeEnum.FromOneToTwelveThousand,
                AgeId = (int)AgeEnum.FromEighteenToSixtyFour,

                CustomerProducts = new List<CustomerProduct>()
                {
                    new CustomerProduct { ProductId = (int)ProductEnum.JuniorSaverAccount }
                }
            };

            CustomerWithCurrentAccount = new Customer
            {
                CustomerProducts = new List<CustomerProduct>()
                {
                    new CustomerProduct { ProductId = (int)ProductEnum.CurrentAccount }
                }
            };

            ProductDebitCard = (int)ProductEnum.DebitCard;
            ProductCurrentAccount = (int)ProductEnum.CurrentAccount;
            ProductGoldCreditCard = (int)ProductEnum.GoldCreditCard;
        }

        [TestMethod]
        public void GetBundleWithBiggestValue()
        {
            var controller = new ApiRuleController();
            var customer = new Customer
            {
                Name = "John",
                AgeId = (int)AgeEnum.FromEighteenToSixtyFour,
                IncomeId = (int)IncomeEnum.FortyThousandOnePlus,
                IsStudent = false
            };

            var bundle = controller.GetBundleWithBiggestValue(customer);

            Assert.AreEqual(bundle.Name, "Gold");
        }

        [TestMethod]
        public void Post()
        {
            var controller = new ApiRuleController();
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var customer = new Customer
            {
                Name = "John",
                AgeId = (int)AgeEnum.FromEighteenToSixtyFour,
                IncomeId = (int)IncomeEnum.FromTwelveThousandOneToFortyThousand,
                IsStudent = false
            };

            var matchViewModel = new MatchViewModel
            {
                Customer = customer,
                BundleId = (int)BundleEnum.ClassicPlus
            };

            var result = controller.Post(matchViewModel);

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void AddDebitCardForCustomerWithJuniorSaverAccount()
        {
            var controller = new ApiRuleController();

            var result = controller.ValidateAddingProduct(this.CustomerWithJuniorSaverAccount, this.ProductDebitCard);

            Assert.AreEqual(Globalization.Rule.DebitCardCanNotBeAddedBecauseNoAccountWasFoundForThisCustomer, result);
        }

        [TestMethod]
        public void AddDebitCardForCustomerWithCurrentAccount()
        {
            var controller = new ApiRuleController();

            var result = controller.ValidateAddingProduct(this.CustomerWithCurrentAccount, this.ProductDebitCard);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TryAddSecondAccount()
        {
            var controller = new ApiRuleController();

            var result = controller.ValidateAddingProduct(this.CustomerWithJuniorSaverAccount, this.ProductCurrentAccount);

            Assert.AreEqual(Globalization.Rule.CustomerCantHaveMoreThanOneAccount, result);
        }

        [TestMethod]
        public void TryAddGoldCreditCard()
        {
            var controller = new ApiRuleController();

            var result = controller.ValidateAddingProduct(this.CustomerWithJuniorSaverAccount, this.ProductGoldCreditCard);

            Assert.AreEqual(Globalization.Rule.ProductIsNotSuitableForThisCustomer, result);
        }

        [TestMethod]
        public void MethodReturnsErrorIfBundleFactoryReturnsNull()
        {
            var controller = new ApiRuleController(new BundleFactoryMoq(), new ProductFactory());
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var customer = new Customer
            {
                Name = "John",
                AgeId = (int)AgeEnum.FromEighteenToSixtyFour,
                IncomeId = (int)IncomeEnum.FromTwelveThousandOneToFortyThousand,
                IsStudent = false
            };

            var matchViewModel = new MatchViewModel
            {
                Customer = customer,
                BundleId = (int)BundleEnum.ClassicPlus
            };

            var result = controller.Post(matchViewModel);

            Assert.AreEqual(result.StatusCode, HttpStatusCode.InternalServerError);
        }

    }
}
