namespace Slots.Core
{
    // Where spin results get their randomness, so tests can swap in a predictable source
    public interface IRandomSource
    {
        int Next(int maxExclusive);
    }
}
