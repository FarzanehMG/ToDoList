using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoList.Application.Contracts.Account
{
    public interface IAccountApplication
    {
        public string Create(CreateAccount command);
        public string Edit(UpdateAccount command);
        List<ViewModelAccount> GetAccounts();

        public Guid Login(Login command);
        public void Logout();

        ViewModelAccount GetAccountBy(Guid id);

        //UpdateAccount GetDetails(Guid id);        

    }
}
