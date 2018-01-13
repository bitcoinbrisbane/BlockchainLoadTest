using System;
using System.Collections.Generic;

namespace BlockchainLoadTest.Data
{
    public interface IData
    {
        Byte[] GetNext();

        IEnumerable<Byte[]> GetNext(Int64 length);
    }
}
