using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace unide
{
    public static class UnideComponentActionExtensions
    {
        public static async UniTask Click(this UniTask<UnideQuery> self)
        {
            var context = await self;
            if (context.QuerySource.EnableCaptureScreenshotBeforeClick)
            {
                await context.TestDriver.CaptureScreenshot(context.QuerySource.TakeScreenshotFilePath());
            }
            await UniTask.Delay(context.Delay);
            var component = context.Target.GetComponent<Button>();
            component.onClick.Invoke();
        }
    
        public static async UniTask SetText(this UniTask<UnideQuery> self, string text)
        {
            var context = await self;
            await UniTask.Delay(context.Delay);
            var component = new TextElement(context.Target);
            component.SetText(text);
        }
    
        public static async UniTask<string> GetText(this UniTask<UnideQuery> self)
        {
            var context = await self;
            await UniTask.Delay(context.Delay);
            var component = new TextElement(context.Target);
            return component.GetText();
        }
    
        public static async UniTask SetValue(this UniTask<UnideQuery> self, float value)
        {
            var context = await self;
            await UniTask.Delay(context.Delay);
            var component = new FloatElement(context.Target);
            component.SetValue(value);
        }
    
        public static async UniTask<float> GetFloat(this UniTask<UnideQuery> self)
        {
            var context = await self;
            await UniTask.Delay(context.Delay);
            var component = new FloatElement(context.Target);
            return component.GetValue();
        }
    }
}