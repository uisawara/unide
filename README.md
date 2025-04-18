# Unide

![IMAGE](https://github.com/uisawara/unide/blob/main/CodeCoverage/Report/badge_combined.svg)

Unity開発におけるUI自動テストを試行錯誤するUnity Package

selenideに影響を受け Unity3D向けのUIテストを気軽に書けるようにしたいと実験的に作ったPackageです。
以下のようにしていきたいです。

- 手間少なく導入して使い始められる。
- 短く書ける。
- 扱いやすくメンテナンス”されやすい”自動テストコードを目指す。
- IDEのコード補完フレンド
- UTF:Unity Test Frameworkフレンドリ
- 他フレームワークと組み合わせやすい。
  - テストクラスに特別な基底クラスを必要としない。


## 前提

### 主に対象とするもの

* 開発時に実装する人のUnity Editor/Test Runnerでのシナリオテストをやりやすくする。

### 主な対象としないもの

* Production向けのクオリティ
  * 主に開発中のシナリオ動作のデバッグとレグレッションテスト向け。
* ビルド済バイナリに対してのテスト
* デバイスフラグメンテーションのテスト

## 指針・判断基準

* 判断時の優先度は以下を想定しています。
  * 利用側の書きやすさ・間違いにくさ＞Package内の可読性＞パフォーマンス
* 多用する記述に対して”極端な”簡略表現をすることを良しとします。
  * (GameObject検索クエリ開始の表現である 'Q' など)
* 命名時はユーザー目線＞エンジニア目線の順で設計判断する。
  * (gameObject.GetComponent<TextMesh>.GetValue() より TextElement(gameObject).GetText()のように抽象化して扱えるのを優先する)

## 機能

- UI要素のクエリ
- UI要素の操作
- UI要素のバリデーション
- Delay,Timeout設定
- スクリーンショット
  - Click前の自動スクリーンショット取得
- 対応するUI Framework
  - uGUI向け

# Installation

## Install via UPM (using Git URL)

* UniTaskが必要なので[こちら](https://github.com/Cysharp/UniTask)を参照しつつインストール
* 続けてunideをインストール
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

// 階層の中にあるボタンクリック
await Q.ByName("SubPageA")
    .ByName("ScrollRect1") // あいだの階層が複数あっても対応してます。
    .ByName("ButtonA")
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
