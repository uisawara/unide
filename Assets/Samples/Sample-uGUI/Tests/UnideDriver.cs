using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class UnideDriver
{
    public static void Open(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

public static class Q
{
    private static GameObject FindObjectByName(string name)
    {
        GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject obj in allObjects)
        {
            GameObject foundObject = SearchInChildren(obj.transform, name);
            if (foundObject != null)
            {
                return foundObject;
            }
        }
        return null;
    }

    private static GameObject SearchInChildren(Transform parent, string name)
    {
        if (parent.gameObject.name == name)
        {
            return parent.gameObject;
        }

        foreach (Transform child in parent)
        {
            GameObject found = SearchInChildren(child, name);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }
    
    private static GameObject FindObjectByTag(string tag)
    {
        GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject obj in allObjects)
        {
            GameObject foundObject = SearchByTagInChildren(obj.transform, tag);
            if (foundObject != null)
            {
                return foundObject;
            }
        }
        return null;
    }

    private static GameObject SearchByTagInChildren(Transform parent, string tag)
    {
        if (parent.gameObject.tag == tag)
        {
            return parent.gameObject;
        }

        foreach (Transform child in parent)
        {
            GameObject found = SearchByTagInChildren(child, tag);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }
    
    public static UniTask<TestContext> ByName(string name)
    {
        var gameObject = FindObjectByName(name);
        return UniTask.FromResult(new TestContext(gameObject.transform));
    }

    public static UniTask<TestContext> ByTag(string tag)
    {
        var gameObject = FindObjectByTag(tag);
        return UniTask.FromResult(new TestContext(gameObject.transform));
    }
}

public class TestContext
{
    public Transform Target { get; }

    public TestContext(Transform target)
    {
        Target = target;
    }
}

public enum Condition
{
    Active,
    Inactive,
    Interactive,
    NonInteractive,
}

public static class TestContextValidationExtensions
{
    public static async UniTask ShouldBe(this UniTask<TestContext> self, Condition condition)
    {
        var context = await self;
        
        switch (condition)
        {
            case Condition.Active:
                await UniTask.WaitUntil(() => context.Target.gameObject.activeSelf);
                break;
            case Condition.Inactive:
                await UniTask.WaitWhile(() => context.Target.gameObject.activeSelf);
                break;
            case Condition.Interactive:
                await UniTask.WaitUntil(() => context.Target.gameObject.GetComponent<Selectable>().IsInteractable());
                break;
            case Condition.NonInteractive:
                await UniTask.WaitWhile(() => context.Target.gameObject.activeInHierarchy && context.Target.gameObject.GetComponent<Selectable>().IsInteractable());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(condition), condition, null);
        }
    }
}

public static class TestContextActionExtensions
{
    public static async UniTask Click(this UniTask<TestContext> self)
    {
        await UniTask.Delay(500);
        var context = await self;
        var button = context.Target.GetComponent<Button>();
        button.onClick.Invoke();
    }
}
