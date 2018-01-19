using System;
using Xunit;
using System.Threading.Tasks;

namespace BlockchainLoadTest.Tests
{
    public class AzureTests
    {
        [Fact]
        public async Task Should_Create_New_VM()
        {

            var task3 = new Task(() => MyLongRunningMethod(),
                    TaskCreationOptions.LongRunning);
            task3.Start();

            Models.Cloud.MicrosoftAzure.CreateVMViewModel vm = new Models.Cloud.MicrosoftAzure.CreateVMViewModel();
            await vm.Create();
        }
    }
}
