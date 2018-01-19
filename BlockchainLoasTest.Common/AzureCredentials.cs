using System;
namespace BlockchainLoadTest.Common
{
    public class AzureCredentials
    {
        protected readonly String username;
        protected readonly String clientId;
        protected readonly String clientSecret;
        protected readonly String tenantId;
        protected readonly String subscriptionId;
        
        public AzureCredentials()
        {
        }
    }
}
