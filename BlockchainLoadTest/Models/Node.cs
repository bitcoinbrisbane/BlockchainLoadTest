using System;
namespace BlockchainLoadTest.Models
{
    public class Node
    {
        public String Url { get; set; }

        public Byte[] PublicKey { get; set; }

        public Node()
        {
        }
    }
}
