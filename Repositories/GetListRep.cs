using AccountMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountMicroservice.Repositories
{
    public class GetListRep : IGetListRep
    {
        public List<AccountStatement> GetAccountStatementsList()
        {
            List<AccountStatement> accountStatements = new List<AccountStatement>()
            {
                new AccountStatement{
                    AccId=202,
                    Statements= new List<Statement>()
                    {
                        new Statement{
                            date=01022021,
                            Narration="Transfer to Shobhan",
                            refno=12345,
                            valueDate=01022021,
                            withdrawal=1000.00,
                            deposit=0.00,
                            closingBalance=1000.00
                        },

                        new Statement{
                            date=04022021,
                            Narration="Transfer from Subhradwip",
                            refno=21345,
                            valueDate=04022021,
                            withdrawal=0.00,
                            deposit=2000.00,
                            closingBalance=3000.00
                        }
                    },
                },
                new AccountStatement{
                    AccId=203,
                    Statements= new List<Statement>()
                    {
                        new Statement{
                            date=01022021,
                            Narration="Transfer to Bijit",
                            refno=12345,
                            valueDate=01022021,
                            withdrawal=1000.00,
                            deposit=0.00,
                            closingBalance=1000.00
                        },

                        new Statement{
                            date=04022021,
                            Narration="Transfer from Subham",
                            refno=21345,
                            valueDate=04022021,
                            withdrawal=0.00,
                            deposit=2000.00,
                            closingBalance=3000.00
                        }
                    },

                }
            };
            return accountStatements;
        }
        /*    public List<CurrentAccount> GetCurrentAccountsList()
       {
           List<CurrentAccount> currentAccounts = new List<CurrentAccount>()
           {
               new CurrentAccount{CAId=101,CBal=1000}
           };
           return currentAccounts;
       }*/

        public List<CustomerAccount> GetCustomeraccountsList()
        {
            List<CustomerAccount> customeraccounts = new List<CustomerAccount>()
            {
                new CustomerAccount{custId=2,CAId=202,SAId=203}
            };
            return customeraccounts;
        }

       

        /*  public List<SavingsAccount> GetSavingsAccountsList()
          {
              List<SavingsAccount> savingsAccounts = new List<SavingsAccount>()
              {
                  new SavingsAccount{SAId=102,SBal=500}
              };
              return savingsAccounts;
          }
        */
        /*public List<Statement> GetStatementsList()
        {
            throw new NotImplementedException();
        }*/
    }
}
