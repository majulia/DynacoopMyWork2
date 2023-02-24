using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyPlugin.Model
{
    public class Conta
    {
        public IOrganizationService ServiceClient { get; set; }

        public string Logicalname { get; set; }

        public Conta(IOrganizationService crmServiceClient)
        {
            this.ServiceClient = crmServiceClient;
            this.Logicalname = "account";
        }

        public Conta(CrmServiceClient crmServiceClient)
        {
            this.ServiceClient = crmServiceClient;
            this.Logicalname = "account";
        }

        public Entity GetAccountById(Guid id, string[] columns)
        {
            return ServiceClient.Retrieve(Logicalname, id, new ColumnSet(columns));
        }

        public void IncrementNumberOfOpp(Entity oppAccount, bool? decrementOrIncrement)
        {
            int numberOfopp = oppAccount.Contains("dcp_nmr_total_opp") ? (int)oppAccount["dcp_nmr_total_opp"] : 0;
            numberOfopp += 1;

            if (Convert.ToBoolean(decrementOrIncrement))
            {
                numberOfopp += 1;
            }
            else
            {
                numberOfopp -= 1;
            }

            oppAccount["dcp_nmr_total_opp"] = numberOfopp;
            ServiceClient.Update(oppAccount);
        }

    }
}
