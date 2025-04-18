using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;

namespace unide
{
    public static class UnideComponentIntActionExtensions
    {
        public static async UniTask SetValue(this UniTask<UnideQuery> self, int value)
        {
            var context = await self;
            Assert.IsNotNull(context.Target);

            await UniTask.Delay(context.Delay);
            var component = new IntElement(context.Target);
            component.SetValue(value);
        }
    
        public static async UniTask<float> GetInt(this UniTask<UnideQuery> self)
        {
            var context = await self;
            Assert.IsNotNull(context.Target);

            await UniTask.Delay(context.Delay);
            var component = new IntElement(context.Target);
            return component.GetValue();
        }
    }
}