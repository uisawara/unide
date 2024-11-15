using Cysharp.Threading.Tasks;

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

    public UniTask<UnideQuery> CreateQueryContext()
    {
        return UniTask.FromResult(new UnideQuery(this));
    }
}