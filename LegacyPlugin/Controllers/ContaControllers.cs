using LegacyPlugin.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyPlugin.Controller
{
    public class ContaControllers
    {
        public IOrganizationService ServiceClient { get; set; }
        public Conta Conta { get; set; }

        public ContaControllers(IOrganizationService crmServiceClient)
        {
            ServiceClient = crmServiceClient;
            this.Conta = new Conta(ServiceClient);
        }

        public Entity GetAccountById(Guid id, string[] columns)
        {
            return Conta.GetAccountById(id, columns);
        }

        public void IncrementNumberOfOpp(Entity oppAccount, bool? incrementOrDecrement)
        {
            Conta.IncrementNumberOfOpp(oppAccount, incrementOrDecrement);
        }
    }
}
