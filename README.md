**Machine translated**

# Unide

![IMAGE](https://github.com/uisawara/unide/blob/main/CodeCoverage/Report/badge_combined.svg)

A Unity Package for UI automated testing in Unity development.

A package designed to make UI testing for Unity3D uGUI easier to write.
We aim to achieve the following:

- Easy to introduce and get started
- Concise syntax
- Aim for maintainable automated test code
- IDE code completion friendly
- UTF: Unity Test Framework friendly
- Easy to combine with other frameworks
  - No special base class required for test classes

## Prerequisites

### Primary Target

- Make scenario testing easier for developers using Unity Editor/Test Runner during development.

### Not Primary Targets

- Production-level quality
  - Mainly for debugging scenario behavior during development and regression testing
- Testing built binaries
- Device fragmentation testing

## Guidelines and Decision Criteria

- Priority order for decision making:
  - Ease of use and error prevention > Package readability > Performance
- We consider "extreme" abbreviated expressions for frequently used descriptions to be acceptable
  - (Such as 'Q' for starting GameObject search queries)
- When naming, we prioritize user perspective over engineer perspective
  - (Prioritize abstraction like TextElement(gameObject).GetText() over gameObject.GetComponent<TextMesh>.GetValue())

## Features

- UI element querying
- UI element manipulation
- UI element validation
- Delay and Timeout settings
- Screenshots
  - Automatic screenshot capture before clicks
- Supported UI Frameworks
  - uGUI support

# Installation

## Install via UPM (using Git URL)

- First install UniTask by referring to [this link](https://github.com/Cysharp/UniTask)
- Then install unide using:

```
https://github.com/uisawara/unide.git?path=Assets/Package/unide
```

# Usage

## Basic Usage

```C#
public sealed class SampleuGUISceneTests
{
    private readonly IUnideDriver D;
    private readonly UnideQuerySource _querySource;

    private UniTask<UnideQuery> Q => _querySource.CreateQueryContext();

    public SampleuGUISceneTests()
    {
        D = new UnideDriver();
        _querySource = new UnideQuerySource(D);
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        D.Open("Sample-uGUI");
    }

    [UnityTest]
    public IEnumerator CanNavigateToPageAAndBack() => UniTask.ToCoroutine(async () =>
    {
        await Q.ByName("SubPageAButton")
            .Click();
        await Q.ByName("TopPage")
            .ShouldBe(Condition.Inactive);
        await Q.ByName("BackButton")
            .Click();
        await Q.ByName("TopPage")
            .ShouldBe(Condition.Active);
    }
}
```

## PageObject Pattern like SceneObject Pattern

```c#
public sealed class SampleuGUISceneObject : SceneObjectBase
{
    public override string SceneName => "Sample-uGUI";

    public SampleuGUISceneObject(IUnideDriver d) : base(d)
    { }

    public UniTask<UnideQuery> TopPage => Q.ByName("TopPage");
    public UniTask<UnideQuery> SubPageA => Q.ByName("SubPageA");
    public UniTask<UnideQuery> SubPageB => Q.ByName("SubPageB");
    public UniTask<UnideQuery> SubPageAButton => Q.ByName("SubPageAButton");
    public UniTask<UnideQuery> SubPageBButton => Q.ByName("SubPageBButton");
    public UniTask<UnideQuery> BackButton => Q.ByName("BackButton");
}

public sealed class SampleuGUISceneWithSceneObjectTests
{
    private readonly SampleuGUISceneObject _sceneObject;

    public SampleuGUISceneWithSceneObjectTests()
    {
        _sceneObject = new SampleuGUISceneObject(new UnideDriver());

        // 操作ウェイト設定
        _sceneObject.QuerySource.Timeout = 5000;
        _sceneObject.QuerySource.Delay = 1500;

        // スクリーンショット自動取得設定
        _sceneObject.QuerySource.BaseScreenshotPath =
            Path.Combine(Application.dataPath, "../TestResults/IntegrationTests/Screenshots");
        _sceneObject.QuerySource.EnableCaptureScreenshotBeforeClick = true;
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _sceneObject.Open();
    }

    [UnityTest]
    public IEnumerator CanNavigateToPageAAndBack() => UniTask.ToCoroutine(async () =>
    {
        await _sceneObject.SubPageAButton
            .Click();
        await _sceneObject.SubPageA
            .ShouldBe(Condition.Active);
        await _sceneObject.BackButton
            .Click();
        await _sceneObject.TopPage
            .ShouldBe(Condition.Active);
    });
}
```

# Sample Snippets

### Common Operations

```c#
// Click button
await Q.ByName("SubPageAButton")
    .Click();

// Click button in hierarchy
await Q.ByName("SubPageA")
    .ByName("ScrollRect1")
    .ByName("ButtonA")
    .Click();

// Click button with timeout
await Q.ByName("SubPageAButton")
    .SetTimeout(10000)
    .Click();
```

### Common Checks

```c#
// Check if active
await Q.ByName("TopPage")
    .ShouldBe(Condition.Active);

// Check if inactive
await Q.ByName("SubPageB")
    .ShouldBe(Condition.Inactive);

// Check TextMeshProUGUI text
await Q.ByName("LabelA")
    .ShouldHave("LabelA");
```

![IMAGE](https://github.com/uisawara/unide/blob/main/Assets/icon.png)

[日本語版はこちら](README.ja.md)
