using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public sealed class UnideContext
{
    private const int DefaultTimeout = 1000;
    private const int DefaultDelay = 500;

    public GameObject Target { get; }

    public int Timeout { get; set; } = DefaultTimeout;
    public int Delay { get; set; } = DefaultDelay;

    public UnideContext(GameObject target)
    {
        Target = target;
    }

    private IUnideDriver _testDriver;

    public UnideContext(IUnideDriver testDriver)
    {
        _testDriver = testDriver;
    }

    public async UniTask<UnideContext> ByName(string name)
    {
        await UniTask.WaitWhile(() => _testDriver.FindObjectByName(name) == null)
            .WithTimeout(Timeout);
        var gameObject = _testDriver.FindObjectByName(name);
        return new UnideContext(gameObject);
    }

    public async UniTask<UnideContext> ByTag(string tag)
    {
        await UniTask.WaitWhile(() => _testDriver.FindObjectByTag(tag) == null)
            .WithTimeout(Timeout);
        var gameObject = _testDriver.FindObjectByTag(tag);
        return new UnideContext(gameObject);
    }
}

public static class TestContextMethodChainExtensions
{
    public static async UniTask<UnideContext> SetTimeout(this UniTask<UnideContext> self, int timeout)
    {
        var context = await self;
        context.Timeout = timeout;
        return context;
    }

    public static async UniTask<UnideContext> SetDelay(this UniTask<UnideContext> self, int delay)
    {
        var context = await self;
        context.Delay = delay;
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

public static class TestContextExtensions
{
    public static async UniTask ShouldBe(this UniTask<UnideContext> self, Condition condition)
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

    public static async UniTask Click(this UniTask<UnideContext> self)
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
