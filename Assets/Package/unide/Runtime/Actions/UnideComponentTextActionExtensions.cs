using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;

namespace unide
{
    public static class UnideComponentTextActionExtensions
    {
        public static async UniTask SetText(this UniTask<UnideQuery> self, string text)
        {
            var context = await self;
            Assert.IsNotNull(context.Target);

            await UniTask.Delay(context.Delay);
            var component = new StringElement(context.Target);
            component.SetText(text);
        }
    
        public static async UniTask<string> GetText(this UniTask<UnideQuery> self)
        {
            var context = await self;
            Assert.IsNotNull(context.Target);

            await UniTask.Delay(context.Delay);
            var component = new StringElement(context.Target);
            return component.GetText();
        }
    }
}