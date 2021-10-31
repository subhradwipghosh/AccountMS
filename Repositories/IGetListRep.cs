using AccountMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountMicroservice.Repositories
{
    interface IGetListRep
    {
        public List<CustomerAccount> GetCustomeraccountsList();
        /*  public List<CurrentAccount> GetCurrentAccountsList();
          public List<SavingsAccount> GetSavingsAccountsList();*/
        public List<AccountStatement> GetAccountStatementsList();
        //public List<Statement> GetStatementsList();
    }
}
