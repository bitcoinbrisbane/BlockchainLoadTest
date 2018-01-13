using System;
using Xunit;
using System.Threading.Tasks;

namespace BlockchainLoadTest.Tests
{
    public class AzureTests
    {
        [Fact]
        public void Should_Create_New_VM()
        {
            Models.Cloud.MicrosoftAzure.CreateVMViewModel vm = new Models.Cloud.MicrosoftAzure.CreateVMViewModel();
            vm.Create();
        }
    }
}
