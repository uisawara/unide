![IMAGE](https://github.com/uisawara/unide/blob/main/CodeCoverage/Report/badge_combined.svg)

# Introduction

selenideに影響を受け Unity3D向けのUIテストを気軽に書けるようにしたいと考え、実験的に作ったPackageです。
以下のようにしていきたいです。

- シンプルに使い始められる。
- 短く書ける。
- 扱いやすくメンテナンス”されやすい”自動テストコードを目指す。
- IDEのコード補完フレンド
- UTF:Unity Test Frameworkフレンドリ

## 前提

### 主に対象とするもの

* Productionコード向けでは”決して”なく、開発過程・維持を支援する。
* 開発時に実装する人がテスト実装を合わせてやりやすくする。
* 開発中にUnity EditorからUIを通したIntegrationTestsをやりやすくするもの。

### 主な対象としないもの

* ビルド済バイナリに対してのテスト
* デバイスフラグメンテーションのテスト

## 指針・判断基準

* 判断時の優先度は以下を想定しています。
  * 利用側の書きやすさ・間違いにくさ＞Package内の可読性＞パフォーマンス
* 多用する記述に対して”極端な”簡略表現をすることを良しとします。(GameObject検索クエリ開始の表現である 'Q' など)

## 現状の制約

* uGUI向け

# Installation

## Install via UPM (using Git URL)

```
https://github.com/uisawara/unide.git?path=Assets/Package/unide
```

# Usages

## Basic usage

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
    public IEnumerator ページAに画面遷移で往復できる() => UniTask.ToCoroutine(async () =>
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
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _sceneObject.Open();
    }

    [UnityTest]
    public IEnumerator ページAに画面遷移で往復できる() => UniTask.ToCoroutine(async () =>
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

# Sample snippets

### 定番の操作

```c#
// ボタンクリック
await Q.ByName("SubPageAButton")
    .Click();

// タイムアウト時間つきボタンクリック
await Q.ByName("SubPageAButton")
    .SetTimeout(10000)
    .Click();
```

### 頻出チェック

```c#
// アクティブかチェック
await Q.ByName("TopPage")
    .ShouldBe(Condition.Active);

// 非アクティブかチェック
await Q.ByName("SubPageB")
    .ShouldBe(Condition.Inactive);

// TestMeshProUGUIのテキストチェック
await Q.ByName("LabelA")
    .ShouldHave("LabelA");
```

![IMAGE](https://github.com/uisawara/unide/blob/main/Assets/icon.png)
