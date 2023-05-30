public struct ParticleWithEquals
{
    private readonly int _x;
    private readonly InternalData _internalData;

    public ParticleWithEquals(int x, InternalData internalData)
    {
        _x = x;
        _internalData = internalData;
    }

    public override bool Equals(object obj) =>
        obj is ParticleWithEquals particleWithEquals &&
        Equals(particleWithEquals);

    public bool Equals(ParticleWithEquals particleWithEquals) =>
        particleWithEquals._x.Equals(_x) &&
        particleWithEquals._internalData.Equals(_internalData);

    public override int GetHashCode() =>
        _x.GetHashCode() ^ _internalData.GetHashCode();
}