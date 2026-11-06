using System.Security.Cryptography;

namespace Slots.Core
{
    // Live game randomness: can't be predicted from earlier spins and has no modulo bias
    public sealed class SecureRandomSource : IRandomSource
    {
        public int Next(int maxExclusive)
        {
            return RandomNumberGenerator.GetInt32(maxExclusive);
        }
    }
}
