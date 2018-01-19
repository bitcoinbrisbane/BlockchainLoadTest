using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Compute.Fluent.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlockchainLoadTest.Models.Cloud.MicrosoftAzure
{
    public class CreateVMViewModel
    {
        public String ResourceGroup { get; set; }

        public Region Region { get; set; }

        private readonly String username;
        private readonly String clientId;
        private readonly String clientSecret;
        private readonly String tenantId;
        private readonly String subscriptionId;

        private const String templateJSON = "AzureVMs.json";
        private const String paramsJSON = "AzureParams.json";


        public CreateVMViewModel()
        {
            this.Region = Region.AustraliaEast;
            this.ResourceGroup = "BlockchainGroup";

            username = "admin@bitcoinbrisbane.com.au";
            tenantId = "db427121-a365-4f43-b04e-117bec05cb45";
            //clientId = "67da947e-800e-4e51-b20a-671c0cd2fc11"; //5a396a8d-87e6-4bff-a6e6-a2aa143f36f2
            clientId = "5a396a8d-87e6-4bff-a6e6-a2aa143f36f2";
            clientSecret = "";
            subscriptionId = "ac74a6ab-5208-4d5b-9818-83010882b4e0";
        }

        public async Task Create()
        {
            try
            {
                var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(clientId, clientSecret, tenantId, AzureEnvironment.AzureGlobalCloud);

                var azure = Azure
                    .Configure()
                    .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                    .Authenticate(credentials)
                    .WithSubscription(subscriptionId);

                //var groupName = ResourceGroup;
                var location = this.Region;

                var resourceGroup = azure.ResourceGroups.Define(ResourceGroup) //groupname
                    .WithRegion(location)
                    .Create();

                //Storage
                string storageAccountName = SdkContext.RandomResourceName("st", 10);

                //Console.WriteLine("Creating storage account...");
                var storage = azure.StorageAccounts.Define(storageAccountName)
                    .WithRegion(this.Region)
                    .WithExistingResourceGroup(resourceGroup)
                    .Create();

                var storageKeys = storage.GetKeys();
                string storageConnectionString = "DefaultEndpointsProtocol=https;"
                    + "AccountName=" + storage.Name
                    + ";AccountKey=" + storageKeys[0].Value
                    + ";EndpointSuffix=core.windows.net";

                var account = CloudStorageAccount.Parse(storageConnectionString);
                var serviceClient = account.CreateCloudBlobClient();

                //Console.WriteLine("Creating container...");
                var container = serviceClient.GetContainerReference("templates");
                container.CreateIfNotExistsAsync().Wait();

                var containerPermissions = new BlobContainerPermissions()
                { 
                    PublicAccess = BlobContainerPublicAccessType.Container 
                };

                container.SetPermissionsAsync(containerPermissions).Wait();

                //Console.WriteLine("Uploading template file...");
                CloudBlockBlob templateblob = container.GetBlockBlobReference(templateJSON);
                await templateblob.UploadFromFileAsync(templateJSON);

                //Console.WriteLine("Uploading parameters file...");
                CloudBlockBlob paramblob = container.GetBlockBlobReference(paramsJSON);
                await paramblob.UploadFromFileAsync(paramsJSON);

                //Deploy
                var templatePath = String.Format("https://{0}.blob.core.windows.net/templates/{1}", storageAccountName, templateJSON);
                var paramPath = String.Format("https://{0}.blob.core.windows.net/templates/{1}", storageAccountName, paramsJSON);

                var deployment = azure.Deployments.Define("myDeployment")
                    .WithExistingResourceGroup(ResourceGroup)
                    .WithTemplateLink(templatePath, "1.0.0.0")
                    .WithParametersLink(paramPath, "1.0.0.0")
                    .WithMode(Microsoft.Azure.Management.ResourceManager.Fluent.Models.DeploymentMode.Incremental)
                    .Create();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
