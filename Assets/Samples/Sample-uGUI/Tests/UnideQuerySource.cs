public sealed class UnideQuerySource
{
    private const int DefaultTimeout = 1000;
    private const int DefaultDelay = 500;

    public IUnideDriver TestDriver { get; }

    public int Timeout { get; set; } = DefaultTimeout;
    public int Delay { get; set; } = DefaultDelay;

    public UnideQuerySource(IUnideDriver testDriver)
    {
        TestDriver = testDriver;
    }

    public UnideQuery CreateQueryContext()
    {
        return new UnideQuery(this);
    }
}