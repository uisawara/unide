using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class UnideQuery
{
    public UnideQuerySource QuerySource { get; }
    public IUnideDriver TestDriver => QuerySource.TestDriver;
    
    public GameObject Target { get; set; }
    public int Timeout { get; set; }
    public int Delay { get; set; }

    public UnideQuery(UnideQuerySource querySource)
    {
        QuerySource = querySource;
        Timeout = QuerySource.Timeout;
        Delay = QuerySource.Delay;
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
        if (context.Target == null)
        {
            await UniTask.WaitWhile(() => context.TestDriver.FindObjectByName(name) == null)
                .WithTimeout(context.Timeout);
            var gameObject = context.TestDriver.FindObjectByName(name);
            context.Target = gameObject;
        }
        else
        {
            await UniTask.WaitWhile(() => context.TestDriver.FindChildByNameDepth(context.Target, name) == null)
                .WithTimeout(context.Timeout); 
            var gameObject = context.TestDriver.FindChildByNameDepth(context.Target, name);
            context.Target = gameObject;
        }
        return context;
    }

    public static async UniTask<UnideQuery> ByTag(this UniTask<UnideQuery> self, string tag)
    {
        var context = await self;
        if (context.Target == null)
        {
            await UniTask.WaitWhile(() => context.TestDriver.FindObjectByTag(tag) == null)
                .WithTimeout(context.Timeout);
            var gameObject = context.TestDriver.FindObjectByTag(tag);
            context.Target = gameObject;
        }
        else
        {
            await UniTask.WaitWhile(() => context.TestDriver.FindChildByTagDepth(context.Target, tag) == null)
                .WithTimeout(context.Timeout);
            var gameObject = context.TestDriver.FindChildByTagDepth(context.Target, tag);
            context.Target = gameObject;
        }
        return context;
    }

    public static async UniTask<UnideQuery> ByComponent<TComponent>(this UniTask<UnideQuery> self) where TComponent : Component
    {
        var context = await self;
        if (context.Target == null)
        {
            await UniTask.WaitWhile(() => context.TestDriver.FindObjectByComponent<TComponent>() == null)
                .WithTimeout(context.Timeout);
            var gameObject = context.TestDriver.FindObjectByComponent<TComponent>();
            context.Target = gameObject;
        }
        else
        {
            await UniTask.WaitWhile(() => context.TestDriver.FindChildByComponentDepth<TComponent>(context.Target) == null)
                .WithTimeout(context.Timeout);
            var gameObject = context.TestDriver.FindObjectByComponent<TComponent>();
            context.Target = gameObject;
        }
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
                text.Equals(new TextElement(context.Target).GetText()))
            .WithTimeout(context.Timeout);
    }
}

public sealed class TextElement
{
    public GameObject Target { get; }

    private TextMesh _textMesh;
    private TextMeshPro _textPro;
    private TextMeshProUGUI _textUGUI;
    private InputField _inputField;
    private TMP_InputField _tmpInputField;
    
    private enum Types
    {
        TextMesh,
        TextMeshPro,
        TextMeshProUGUI,
        InputField,
        TMP_InputField,
    }

    private Types _type;
    
    public TextElement(GameObject target)
    {
        Target = target;
        _textMesh = Target.GetComponent<TextMesh>();
        if (_textMesh == null)
        {
            // Target.GetComponent<TextMesh>() を呼び出した結果が null だった場合、nullではあるがExceptionメッセージを含むオブジェクトが返ってくる。
            // これがTextMeshPro,InputField等と挙動が違っているのを解消するため、明示的にnull代入をやり直すことで対策にしている。
            _textMesh = null;
        }
        _textPro = Target.GetComponent<TextMeshPro>();
        _textUGUI = Target.GetComponent<TextMeshProUGUI>();
        _inputField = Target.GetComponent<InputField>();
        _tmpInputField = Target.GetComponent<TMP_InputField>();
        object[] objs = { _textMesh, _textPro, _textUGUI, _inputField, _tmpInputField };
        var count = objs.Where(o => o != null)
            .Count();
        if (count != 1)
        {
            throw new ArgumentException($"GameObject have too many text components: count={count}");
        }

        _type = (Types)objs.Select((o, i) => new { o, i })
            .First(x => x.o != null)
            .i;
    }

    public string GetText()
    {
        switch (_type)
        {
            case Types.TextMesh:
                return _textMesh.text;
            case Types.TextMeshPro:
                return _textPro.text;
            case Types.TextMeshProUGUI:
                return _textUGUI.text;
            case Types.InputField:
                return _inputField.text;
            case Types.TMP_InputField:
                return _tmpInputField.text;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetText(string text)
    {
        switch (_type)
        {
            case Types.TextMesh:
                _textMesh.text = text;
                break;
            case Types.TextMeshPro:
                _textPro.text = text;
                break;
            case Types.TextMeshProUGUI:
                _textUGUI.text = text;
                break;
            case Types.InputField:
                _inputField.text = text;
                break;
            case Types.TMP_InputField:
                _tmpInputField.text = text;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

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
    
    public static async UniTask SetValue(this UniTask<UnideQuery> self, string text)
    {
        var context = await self;
        await UniTask.Delay(context.Delay);
        var component = new TextElement(context.Target);
        component.SetText(text);
    }
    
    public static async UniTask<string> GetValue(this UniTask<UnideQuery> self)
    {
        var context = await self;
        await UniTask.Delay(context.Delay);
        var component = new TextElement(context.Target);
        return component.GetText();
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
