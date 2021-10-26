using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Core.Fluent.Enums
{
    public enum GenderType
    {
        Men,
        Woman
    }

    public enum HashAlgorithmType
    {
        sha256crypt,
        sha512crypt,
        sha3_256crypt,
        sha3_512crypt,
        shake128crypt,
        shake256crypt
    }
}
