public struct ParticleWithoutEquals
{
    private readonly int _x;
    private readonly InternalData _internalData;

    public ParticleWithoutEquals(int x, InternalData internalData)
    {
        _x = x;
        _internalData = internalData;
    }
}
