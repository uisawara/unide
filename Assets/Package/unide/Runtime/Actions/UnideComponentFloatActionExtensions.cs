using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;

namespace unide
{
    public static class UnideComponentFloatActionExtensions
    {
        public static async UniTask SetValue(this UniTask<UnideQuery> self, float value)
        {
            var context = await self;
            Assert.IsNotNull(context.Target);

            await UniTask.Delay(context.Delay);
            var component = new FloatElement(context.Target);
            component.SetValue(value);
        }
    
        public static async UniTask<float> GetFloat(this UniTask<UnideQuery> self)
        {
            var context = await self;
            Assert.IsNotNull(context.Target);

            await UniTask.Delay(context.Delay);
            var component = new FloatElement(context.Target);
            return component.GetValue();
        }
    }
}