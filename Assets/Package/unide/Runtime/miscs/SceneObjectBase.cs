using Cysharp.Threading.Tasks;

namespace unide
{
    public abstract class SceneObjectBase
    {
        public abstract string SceneName { get; }

        public UnideQuerySource QuerySource { get; }

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
}