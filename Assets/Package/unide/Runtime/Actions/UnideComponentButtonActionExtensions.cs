using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace unide
{
    public static class UnideComponentButtonActionExtensions
    {
        public static async UniTask Click(this UniTask<UnideQuery> self)
        {
            var context = await self;
            Assert.IsNotNull(context.Target);
            
            var component = context.Target.GetComponent<Button>();

            var target = component.gameObject;
            var canvas = target.GetComponentInParent<Canvas>(true);
            var camera = GetUICamera(canvas);
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
                position = RectTransformUtility.WorldToScreenPoint(camera, target.transform.position)
            };
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerDownHandler);
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerClickHandler);
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerUpHandler);

            //component.onClick.Invoke();
            
            await UniTask.Delay(context.Delay);
            if (context.QuerySource.EnableCaptureScreenshotBeforeClick)
            {
                await context.TestDriver.CaptureScreenshot(context.QuerySource.TakeScreenshotFilePath());
            }
        }
        
        private static Camera GetUICamera(Canvas canvas)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                return null; // Overlay はカメラ不要
            }
            else
            {
                return canvas.worldCamera ?? Camera.main;
            }
        }
    }
}