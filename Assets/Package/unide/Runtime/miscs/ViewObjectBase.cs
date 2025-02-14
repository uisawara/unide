using Cysharp.Threading.Tasks;

namespace unide
{
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
}
