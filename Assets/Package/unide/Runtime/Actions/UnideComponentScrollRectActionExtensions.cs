using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace unide
{
    public static class UnideComponentScrollRectActionExtensions
    {
        public static async UniTask SetScrollPositionVertical(this UniTask<UnideQuery> self, float normalizedPosition)
        {
            var context = await self;
            Assert.IsNotNull(context.Target);

            var component = context.Target.GetComponent<ScrollRect>();

            await UniTask.Delay(context.Delay);

            component.verticalNormalizedPosition = Mathf.Clamp01(normalizedPosition);
        }

        public static async UniTask CaptureScreenshotVerticalAll(this UniTask<UnideQuery> self)
        {
            var context = await self;
            Assert.IsNotNull(context.Target);

            var component = context.Target.GetComponent<ScrollRect>();

            await UniTask.Delay(context.Delay);

            await ScrollFromTopToBottomAsync(component, 0.7f, 0.25f,
                v => { return context.TestDriver.CaptureScreenshot(context.QuerySource.TakeScreenshotFilePath()); });
        }

        /// <summary>
        /// ScrollRect を上から下まで段階的にスクロールし、各段階で Action を実行。
        /// </summary>
        /// <param name="scrollRect">対象の ScrollRect</param>
        /// <param name="stepRatio">表示領域に対する1ステップの割合（0.7 = 70%）</param>
        /// <param name="waitSeconds">各ステップ間の待機秒数</param>
        /// <param name="onStep">ステップごとに実行されるコールバック（stepIndex を渡す）</param>
        private static async UniTask ScrollFromTopToBottomAsync(
            ScrollRect scrollRect,
            float stepRatio = 0.7f,
            float waitSeconds = 1f,
            Func<int, UniTask> onStep = null
        )
        {
            RectTransform viewport = scrollRect.viewport;
            RectTransform content = scrollRect.content;

            float viewHeight = viewport.rect.height;
            float contentHeight = content.rect.height;
            float scrollableHeight = contentHeight - viewHeight;

            if (scrollableHeight <= 0f)
            {
                Debug.Log("スクロール不要（コンテンツが小さい）");
                return;
            }

            float delta = (viewHeight * stepRatio) / scrollableHeight;
            float pos = 1.0f;

            int step = 0;

            while (pos > 0f)
            {
                scrollRect.verticalNormalizedPosition = Mathf.Clamp01(pos);
                await onStep.Invoke(step);
                await UniTask.Delay(TimeSpan.FromSeconds(waitSeconds));
                pos -= delta;
                step++;
            }

            scrollRect.verticalNormalizedPosition = 0f;
            onStep?.Invoke(step);
        }
    }
}