using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class UnideQuery
{
    public IUnideDriver TestDriver { get; }

    public GameObject Target { get; set; }
    public int Timeout { get; set; }
    public int Delay { get; set; }


    public UnideQuery(UnideQuerySource querySource)
    {
        TestDriver = querySource.TestDriver;
        Timeout = querySource.Timeout;
        Delay = querySource.Delay;
    }
}

public static class UnideQueryMethodChainExtensions
{
    public static async UniTask<UnideQuery> SetTimeout(this UniTask<UnideQuery> self, int timeout)
    {
        var context = await self;
        context.Timeout = timeout;
        return context;
    }

    public static async UniTask<UnideQuery> SetDelay(this UniTask<UnideQuery> self, int delay)
    {
        var context = await self;
        context.Delay = delay;
        return context;
    }
}

public static class UnideQueryExtensions
{
    public static async UniTask<UnideQuery> ByName(this UniTask<UnideQuery> self, string name)
    {
        var context = await self;
        await UniTask.WaitWhile(() => context.TestDriver.FindObjectByName(name) == null)
            .WithTimeout(context.Timeout);
        var gameObject = context.TestDriver.FindObjectByName(name);
        context.Target = gameObject;
        return context;
    }

    public static async UniTask<UnideQuery> ByTag(this UniTask<UnideQuery> self, string tag)
    {
        var context = await self;
        await UniTask.WaitWhile(() => context.TestDriver.FindObjectByTag(tag) == null)
            .WithTimeout(context.Timeout);
        var gameObject = context.TestDriver.FindObjectByTag(tag);
        context.Target = gameObject;
        return context;
    }

    public static async UniTask<UnideQuery> ByComponent<TComponent>(this UniTask<UnideQuery> self) where TComponent : Component
    {
        var context = await self;
        await UniTask.WaitWhile(() => context.TestDriver.FindObjectByComponent<TComponent>() == null)
            .WithTimeout(context.Timeout);
        var gameObject = context.TestDriver.FindObjectByComponent<TComponent>();
        context.Target = gameObject;
        return context;
    }

    public static async UniTask<UnideQuery> At(this UniTask<UnideQuery> self, int childIndex)
    {
        var context = await self;
        await UniTask.WaitWhile(() => context.Target.transform.GetChild(childIndex).gameObject == null)
            .WithTimeout(context.Timeout);
        context.Target = context.Target.transform.GetChild(childIndex).gameObject;
        return context;
    }
}

public enum Condition
{
    Active,
    Inactive,
    Interactive,
    NonInteractive,
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
            default:
                throw new ArgumentOutOfRangeException(nameof(condition), condition, null);
        }
    }

    public static async UniTask ShouldHave(this UniTask<UnideQuery> self, string text)
    {
        var context = await self;
        await UniTask.WaitUntil(() => 
                text.Equals(context.Target.GetComponent<TextMeshProUGUI>()?.text))
            .WithTimeout(context.Timeout);
    }
}

public static class UnideComponentActionExtensions
{
    public static async UniTask Click(this UniTask<UnideQuery> self)
    {
        var context = await self;
        await UniTask.Delay(context.Delay);
        var button = context.Target.GetComponent<Button>();
        button.onClick.Invoke();
    }
}

public static class UniTaskTimeoutExtensions
{
    public static async UniTask WithTimeout(this UniTask task, int timeout)
    {
        var timeoutTask = UniTask.Delay(timeout);
        if (await UniTask.WhenAny(timeoutTask, task) == 0)
        {
            throw new TimeoutException("The task has timed out.");
        }
    }
}
