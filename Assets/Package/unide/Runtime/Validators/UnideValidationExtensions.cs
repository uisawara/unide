using System;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace unide
{
    public enum Condition
    {
        Active,
        Inactive,
        Interactive,
        NonInteractive,
        Disappear
    }
    
    public static class UnideValidationExtensions
    {
        public static async UniTask ShouldBe(this UniTask<UnideQuery> self, Condition condition)
        {
            var context = await self;
        
            switch (condition)
            {
                case Condition.Active:
                    await UniTask.WaitUntil(() => context.Target.activeSelf).WithTimeout(context.Timeout);
                    break;
                case Condition.Inactive:
                    await UniTask.WaitWhile(() => context.Target.activeSelf).WithTimeout(context.Timeout);
                    break;
                case Condition.Interactive:
                    await UniTask.WaitUntil(() => context.Target.GetComponent<Selectable>().IsInteractable()).WithTimeout(context.Timeout);
                    break;
                case Condition.NonInteractive:
                    await UniTask.WaitWhile(() => context.Target.activeInHierarchy && context.Target.GetComponent<Selectable>().IsInteractable()).WithTimeout(context.Timeout);
                    break;
                case Condition.Disappear:
                    await UniTask.WaitWhile(() => context.Target !=null).WithTimeout(context.Timeout);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(condition), condition, null);
            }
        }

        public static async UniTask ShouldHave(this UniTask<UnideQuery> self, string text)
        {
            var context = await self;
            await UniTask.WaitUntil(() => 
                    text.Equals(new TextElement(context.Target).GetText()))
                .WithTimeout(context.Timeout);
        }
    }
}