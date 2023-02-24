using LegacyPlugin.Controller;
using LegacyPlugin.Model;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyPlugin
{
    public class OpportunityManager : IPlugin
    {
        public IOrganizationService Service { get; set; }

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Entity opportunity = new Entity();
            bool? incrementOrDecrement = null;

            if (context.MessageName == "Delete")
            {
                opportunity = (Entity)context.PreEntityImages["PreImage"];
                incrementOrDecrement = false;
            }
            else
                incrementOrDecrement = null;

            ExecuteOpportunityProcess(context, opportunity, incrementOrDecrement);
        }

        private void ExecuteOpportunityProcess(IPluginExecutionContext context, Entity opportunity, bool? incrementOrDecrement)
        {
            EntityReference accountReference = opportunity.Contains("parentaccountid") ? (EntityReference)opportunity["parentaccountid"] : null;

            if (accountReference != null)
            {
                Entity oppAccount = UpdateAccount(incrementOrDecrement, accountReference);

                if (context.MessageName == "Update")
                {
                    Entity opportunityPostImage = (Entity)context.PostEntityImages["PostImage"];
                    EntityReference postAccountReference = (EntityReference)opportunityPostImage["parentaccountid"];
                    UpdateAccount(true, postAccountReference);
                }
            }
        }

        private Entity UpdateAccount(bool? incrementOrDecrement, EntityReference accountReference)
            {
                ContaControllers contaController = new ContaControllers(this.Service);
                Entity oppAccount = contaController.GetAccountById(accountReference.Id, new string[] { "dcp_nmr_total" });
                contaController.IncrementNumberOfOpp(oppAccount, incrementOrDecrement);
                return oppAccount;
            }
    }
}
