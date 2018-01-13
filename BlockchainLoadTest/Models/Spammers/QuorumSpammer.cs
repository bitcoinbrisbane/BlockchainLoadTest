using System;
using System.Threading.Tasks;

namespace BlockchainLoadTest.Models.Spammers
{
    public class QuorumSpammer : ISpammer
    {
        private readonly Repository.IReadOnlyRepository _repo;
        private DateTime started;

        public TimeSpan RunningTime 
        {
            get { return DateTime.UtcNow - started; }
        }

        public QuorumSpammer(Repository.IWriteOnlyRepository repo)
        {
            //_repo = repo;
        }

        public async Task Run(TimeSpan duration)
        {
            started = DateTime.UtcNow;
        }

        public async Task Run(Int64 iterations)
        {
            started = DateTime.UtcNow;
        }
    }
}
