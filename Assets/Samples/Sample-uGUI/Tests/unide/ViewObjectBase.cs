using Cysharp.Threading.Tasks;

public abstract class SceneObjectBase
{
    public abstract string SceneName { get; }

    private UnideQuerySource QuerySource { get; }
    
    public IUnideDriver D { get; }
    public UniTask<UnideQuery> Q => QuerySource.CreateQueryContext();
    
    protected SceneObjectBase(IUnideDriver d)
    {
        D = d;
        QuerySource = new UnideQuerySource(D);
    }

    public void Open()
    {
        D.Open(SceneName);
    }
}

public abstract class ViewObjectBase
{
    private SceneObjectBase _sceneObject;

    protected IUnideDriver D => _sceneObject.D;
    protected UniTask<UnideQuery> Q => _sceneObject.Q;

    public ViewObjectBase(SceneObjectBase sceneObject)
    {
        _sceneObject = sceneObject;
    }
}
