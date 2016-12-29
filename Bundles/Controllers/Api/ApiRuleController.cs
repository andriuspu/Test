using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bundles.Models;
using Business;
using Data;

namespace Service.Controllers.Api
{
    public class ApiRuleController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private BundlesRepository BundlesRepository;

        private IBundleFactory BundleFactory;

        private IProductFactory ProductFactory;

        public ApiRuleController()
        {
            this.BundlesRepository = new BundlesRepository();
            this.BundleFactory = new BundleFactory();
            this.ProductFactory = new ProductFactory();
        }

        public ApiRuleController(IBundleFactory bundleFactory, IProductFactory productFactory)
        {
            this.BundleFactory = bundleFactory;
            this.ProductFactory = productFactory;
        }

        [Route("api/rule")]
        public HttpResponseMessage Post(Customer customer)
        {
            var validationResult = ValidateRequestData(customer);
            if (validationResult != null)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, validationResult);
            }

            var bundle = this.GetBundleWithBiggestValue(customer);

            if (bundle == null)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Globalization.Rule.ThereIsNoSuitableBundle);
            }

            try
            {
                BundlesRepository.SaveCustomerAndOfferedProducts(customer, bundle.ProductList.Select(p => p.Id).ToList());
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Globalization.Rule.UnhandledErrorHasOccured);
            }

            return Request.CreateResponse(HttpStatusCode.Created, bundle);
        }

        [Route("api/match")]
        public HttpResponseMessage Post(MatchViewModel matchViewModel)
        {
            var validationResult = ValidateRequestData(matchViewModel.Customer);
            if (validationResult != null)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, validationResult);
            }

            var bundle = this.BundleFactory.Create(matchViewModel.BundleId);
            
            if (bundle == null)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Globalization.Rule.NoBundleFoundBySuchName);
            }

            var notSuitableProduct = bundle.CheckIfSuitsForCustomer(matchViewModel.Customer);

            if (notSuitableProduct == string.Empty)
            {
                return Request.CreateResponse(HttpStatusCode.OK, Globalization.Rule.RequestedBundleIsSuitableForThisCustomer);
            }
            else
            {
                return Request.CreateResponse(
                    HttpStatusCode.InternalServerError, 
                    string.Format(Globalization.Rule.ProductsNotSuitedForBundle, notSuitableProduct));  
            }
        }

        [Route("api/addProduct")]
        public HttpResponseMessage Post(AddProductViewModel productViewModel)
        {
            var customer = BundlesRepository.GetCustomerWithProducts(productViewModel.CustomerId);

            var validationResult = this.ValidateAddingProduct(customer, productViewModel.ProductId);

            if (!string.IsNullOrEmpty(validationResult))
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, validationResult);
            }
            else
            {
                try
                {
                    BundlesRepository.InsertProductForCustomer(productViewModel.CustomerId, productViewModel.ProductId);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, Globalization.Rule.UnhandledErrorHasOccured);
                }                
            }
        }

        [NonAction]
        public Bundle GetBundleWithBiggestValue(Customer customer)
        {
            var bundleList = this.InitializeBundleData();

            foreach (var bundle in bundleList)
            {
                if (string.IsNullOrEmpty(bundle.CheckIfSuitsForCustomer(customer)))
                {
                    return bundle;
                }                
            }

            return null;
        }

        private List<Bundle> InitializeBundleData()
        {
            var bundleFactory = new BundleFactory();

            var bundleList = new List<Bundle>()
            {
                bundleFactory.Create((int)BundleEnum.JuniorSaver),
                bundleFactory.Create((int)BundleEnum.Student),
                bundleFactory.Create((int)BundleEnum.Classic),
                bundleFactory.Create((int)BundleEnum.ClassicPlus),
                bundleFactory.Create((int)BundleEnum.Gold)
            };
        
            return bundleList.OrderByDescending(b => b.Value).ToList();
        }        

        private string ValidateRequestData(Customer customer)
        {
            if (customer == null)
            {
                return Globalization.Rule.CustomerAnswersAreNotSupplied;
            }

            if (customer.AgeId == 0)
            {
                return Globalization.Rule.CustomerAgeIsNotSupplied;
            }

            if (customer.IncomeId == 0)
            {
                return Globalization.Rule.CustomerIncomeIsNotSupplied;
            }

            if (string.IsNullOrEmpty(customer.Name))
            {
                return Globalization.Rule.CustomerNameIsNotSupplied;
            }

            return null;
        }

        [NonAction]
        public string ValidateAddingProduct(Customer customer, int productId)
        {      
            var customerProductIds = customer.CustomerProducts.Select(cp => cp.ProductId).ToList();

            var product = this.ProductFactory.Create(productId);

            var errorList = product.CheckIfSuitsForBundle(customer);

            if (!string.IsNullOrEmpty(errorList))
            {
                return Globalization.Rule.ProductIsNotSuitableForThisCustomer;
            }

            var validationSucceded = product.ValidateForDuplicateAccount(customerProductIds, productId);

            if (!validationSucceded)
            {
                return Globalization.Rule.CustomerCantHaveMoreThanOneAccount;
            }

            var debitCardProduct = product as DebitCardProduct;

            if (debitCardProduct != null && !debitCardProduct.CheckIfCanAddToExistingProducts(customerProductIds))
            {
                return Globalization.Rule.DebitCardCanNotBeAddedBecauseNoAccountWasFoundForThisCustomer;
            }

            return null;
        }
    }
}
