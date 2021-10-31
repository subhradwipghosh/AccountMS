using AccountMicroservice.Models;
using AccountMicroservice.Repositories;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccountMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class AccountController : ControllerBase
    {
        Uri baseAddress = new Uri("http://localhost:16374/api");   //Port No.
        HttpClient client;

        //readonly log4net.ILog _log4net;



        public AccountController()
        {
            client = new HttpClient();
           client.BaseAddress = baseAddress;
        //    _log4net = log4net.logmanager.getlogger(typeof(accountcontroller));


        }

        int acid = 2;
        /*public static List<customeraccount> customeraccounts = new List<customeraccount>()
        {
            new customeraccount{custId=1,CAId=101,SAId=102}
        };*/
        public static List<CurrentAccount> currentAccounts = new List<CurrentAccount>()
        {
            new CurrentAccount{CAId=202,CBal=1000}
        };
        public static List<SavingsAccount> savingsAccounts = new List<SavingsAccount>()
        {
            new SavingsAccount{SAId=203,SBal=3000}
        };

        // GET: api/<AccountController>
        //[HttpGet]
        //[Route("getCurrentAccountList")]
        //public IActionResult GetCurrent()
        //{
        //    return Ok(currentAccounts);
        //}

        //[HttpGet]
        //[Route("getSavingAccountList")]
        //public IActionResult GetSavings()
        //{
        //    return Ok(savingsAccounts);
        //}

        // GET api/<AccountController>/
        [HttpGet]
        [Route("getCustomerAccounts/{id}")]
        public AccountSummary getCustomerAccounts(int id)
        {
            GetListRep ob = new GetListRep();
            var customeraccounts = ob.GetCustomeraccountsList();

            //_log4net.Info(" Got Customer Account");
            var a = customeraccounts.Find(c => c.custId == id);
            var ca = currentAccounts.FirstOrDefault(cac => cac.CAId == a.CAId);
            var sa = savingsAccounts.Find(sac => sac.SAId == a.SAId);
            AccountSummary summary = new AccountSummary()
            {
                CAid = ca.CAId,
                CAbal = ca.CBal,
                SAid = sa.SAId,
                SAbal = sa.SBal
            };
            return summary;
        }

        // POST api/<AccountController>
        [HttpPost]
        [Route("createAccount")]
        public CustomerAccount createAccount([FromBody] Customer customer)
        {
            GetListRep ob = new GetListRep();
            var customeraccounts = ob.GetCustomeraccountsList();

            //_log4net.Info("Account Created");
            CustomerAccount a = new CustomerAccount
            {
                custId = customer.id,
                CAId = (customer.id * 100) + acid,
                SAId = (customer.id * 100) + (acid + 1)
            };
            customeraccounts.Add(a);
            var cust = customeraccounts.Find(c => c.custId == customer.id);
            CurrentAccount ca = new CurrentAccount
            {
                CAId = (customer.id * 100) + acid,
                CBal = 0.00
            };
            SavingsAccount sa = new SavingsAccount
            {
                SAId = (customer.id * 100) + (acid + 1),
                SBal = 0.00
            };
            return cust;
        }
        [HttpGet]
        [Route("getAccount/{id}")]
        public object getAccount(int id)
        {
            GetListRep ob = new GetListRep();
            var customeraccounts = ob.GetCustomeraccountsList();

           // _log4net.Info(" Getting Account Info");
            if (id % 2 != 0)
            {
                var ca = currentAccounts.Find(a => a.CAId == id);
                //var t = "Current Account(" + ca.CAId + "):: Rs." + ca.CBal + ".00";
                return ca ;
            }

            var sa = savingsAccounts.Find(a => a.SAId == id);
            //return "Savings Account(" + sa.SAId + "):: Rs." + sa.SBal + ".00";
            return sa;

        }
        [HttpGet]
        [Route("getAccountStatement/{AccountId}/{from_date}/{to_date}")]
        public IEnumerable<Statement> getAccountStatement(int AccountId, int from_date, int to_date)
        {
           // _log4net.Info("Account Statement Shown");
            GetListRep ob = new GetListRep();
            var ac = ob.GetAccountStatementsList();
            if (from_date != 0 || to_date != 0)
            {
                var accs = ac.Find(a => a.AccId == AccountId);
                var s = accs.Statements;
                foreach (var n in s)
                {
                    if (n.date >= from_date && n.date <= to_date)
                    {
                        return s;
                    }
                }
            }
            var accs1 = ac.Find(a => a.AccId == AccountId);
            var s1 = accs1.Statements;
            foreach (var n in s1)
            {
                if (n.date > 01092020 && n.date < 30092020)
                {
                    return s1;
                }
            }
            return null;
        }
        [HttpPost]
        [Route("deposit")]
        public string deposit([FromBody] DwAcc value)
        {
            GetListRep ob = new GetListRep();
            var customeraccounts = ob.GetCustomeraccountsList();

            string data = JsonConvert.SerializeObject(value);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Transaction/deposit/", content).Result;
            if (response.IsSuccessStatusCode)
            {
               // _log4net.Info(" Amount Deposited ");
                string data1 = response.Content.ReadAsStringAsync().Result;
                if (data1 == "Success")
                {
                    if (value.AccountId % 2 == 0)
                    {
                        var sa = savingsAccounts.Find(a => a.SAId == value.AccountId);
                        sa.SBal = sa.SBal + value.Balance;
                        return "Deposited Successfully.New Account Rs." + sa.SBal + ".00";
                    }
                    var ca = currentAccounts.Find(a => a.CAId == value.AccountId);
                    ca.CBal = ca.CBal + value.Balance;
                    return "Deposited Successfully.New Account Rs." + ca.CBal + ".00";
                }
                return "Deposition Failed";
            }
            return "Link Failure";
        }
        [HttpPost]
        [Route("withdraw")]
        public string withdraw([FromBody] DwAcc value)
        {
            GetListRep ob = new GetListRep();
            var customeraccounts = ob.GetCustomeraccountsList();

            string data = JsonConvert.SerializeObject(value);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Transaction/withdraw/", content).Result;
            if (response.IsSuccessStatusCode)
            {
              //  _log4net.Info("Amount Withdrawn");
                string data1 = response.Content.ReadAsStringAsync().Result;
                if (data1 == "No Warning")
                {
                    if (value.AccountId % 2 == 0)
                    {
                        var sa = savingsAccounts.Find(a => a.SAId == value.AccountId);
                        sa.SBal = sa.SBal - value.Balance;
                        if (sa.SBal >= 0)
                            return "Withdrawn Successfully.New Balance Rs." + sa.SBal + ".00";
                        else
                        {
                            sa.SBal = sa.SBal + value.Balance;
                            return "Insufficient Fund";
                        }

                    }

                    var car = currentAccounts.Find(a => a.CAId == value.AccountId);
                    car.CBal = car.CBal - value.Balance;
                    if (car.CBal >= 0)
                        return "Withdrawn Successfully.New Balance Rs." + car.CBal + ".00";
                    else
                    {
                        car.CBal = car.CBal + value.Balance;
                        return "Insufficient Fund";
                    }

                }
                if (value.AccountId % 2 == 0)
                {
                    var sa = savingsAccounts.Find(a => a.SAId == value.AccountId);
                    sa.SBal = sa.SBal - value.Balance;
                    if (sa.SBal >= 0)
                        return "Withdrawn Successfully.New Balance Rs." + sa.SBal + ".00 but service charge will be deducted at the end of month";
                    else
                    {
                        sa.SBal = sa.SBal + value.Balance;
                        return "Insufficient Fund";
                    }
                }
                var ca = currentAccounts.Find(a => a.CAId == value.AccountId);
                ca.CBal = ca.CBal - value.Balance;
                if (ca.CBal >= 0)
                    return "Withdrawn Successfully.New Balance Rs." + ca.CBal + ".00 but service charge will be deducted at the end of month";
                else
                {
                    ca.CBal = ca.CBal + value.Balance;
                    return "Insufficient Fund";
                }



            }
            return "Link Failure";
        }

    //    [HttpPost]
    //    [Route("Transfer")]
    //    public string transfer([FromBody] Transfers value)
    //    {
    //        GetListRep ob = new GetListRep();
    //        var customeraccounts = ob.GetCustomeraccountsList();
    //        //    var currentAccounts = ob.GetCurrentAccountsList();
    //        //  var savingsAccounts = ob.GetSavingsAccountsList();
    //        double sb = 0.0, db = 0.0;
    //        string data = JsonConvert.SerializeObject(value);
    //        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

    //        HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Transaction/transfer/", content).Result;
    //        if (response.IsSuccessStatusCode)
    //        {
    //           // _log4net.Info("Amount Transferred");
    //            string data1 = response.Content.ReadAsStringAsync().Result;
    //            if (data1 == "No Warning")
    //            {
    //                if (value.source_accid % 2 == 0)
    //                {
    //                    var sas = savingsAccounts.Find(a => a.SAId == value.source_accid);
    //                    sas.SBal = sas.SBal - value.amount;
    //                    if (sas.SBal >= 0)
    //                        sb = sas.SBal;
    //                    else
    //                    {
    //                        sas.SBal = sas.SBal + value.amount;
    //                        return "Insufficient Fund";
    //                    }
    //                }
    //                else
    //                {
    //                    var cas = currentAccounts.Find(a => a.CAId == value.source_accid);
    //                    cas.CBal = cas.CBal - value.amount;
    //                    if (cas.CBal >= 0)
    //                        sb = cas.CBal;
    //                    else
    //                    {
    //                        cas.CBal = cas.CBal + value.amount;
    //                        return "Insufficient Fund";
    //                    }

    //                }
    //                if (value.destination_accid % 2 == 0)
    //                {
    //                    var sa = savingsAccounts.Find(a => a.SAId == value.destination_accid);
    //                    sa.SBal = sa.SBal + value.amount;
    //                    db = sa.SBal;
    //                }
    //                else
    //                {
    //                    var ca = currentAccounts.Find(a => a.CAId == value.destination_accid);
    //                    ca.CBal = ca.CBal + value.amount;
    //                    db = ca.CBal;
    //                }
    //                return "Sender Account Balance Rs." + sb + ".00\n" + "Receiver Account Balance Rs." + db + ".00";
    //            }
    //            else
    //            {
    //                if (value.source_accid % 2 == 0)
    //                {
    //                    var sas = savingsAccounts.Find(a => a.SAId == value.source_accid);
    //                    sas.SBal = sas.SBal - value.amount;
    //                    if (sas.SBal >= 0)
    //                        sb = sas.SBal;
    //                    else
    //                    {
    //                        sas.SBal = sas.SBal + value.amount;
    //                        return "Insufficient Fund";
    //                    }

    //                }
    //                else
    //                {
    //                    var cas = currentAccounts.Find(a => a.CAId == value.source_accid);
    //                    cas.CBal = cas.CBal - value.amount;
    //                    if (cas.CBal >= 0)
    //                        sb = cas.CBal;
    //                    else
    //                    {
    //                        cas.CBal = cas.CBal + value.amount;
    //                        return "Insufficient Fund";
    //                    }

    //                }
    //                if (value.destination_accid % 2 == 0)
    //                {
    //                    var sa = savingsAccounts.Find(a => a.SAId == value.destination_accid);
    //                    sa.SBal = sa.SBal + value.amount;
    //                    db = sa.SBal;
    //                }
    //                else
    //                {
    //                    var ca = currentAccounts.Find(a => a.CAId == value.destination_accid);
    //                    ca.CBal = ca.CBal + value.amount;
    //                    db = ca.CBal;
    //                }
    //                return "Sender Account Balance Rs." + sb + ".00\n" + "Receiver Account Balance Rs." + db + ".00\n but service charge will be deducted at the end of month from your account";

    //            }

    //        }
    //        return "Link Failure";
    //    }
    }
}
