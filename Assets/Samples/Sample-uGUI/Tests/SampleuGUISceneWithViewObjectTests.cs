using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using unide;
using UnityEngine.TestTools;

namespace Samples.Sample_uGUI.Tests
{
    public sealed class SampleuGUISceneWithViewObjectTests
    {
        private readonly SampleuGUISceneAndViewObject _sceneObject;

        public SampleuGUISceneWithViewObjectTests()
        {
            _sceneObject = new SampleuGUISceneAndViewObject(new UnideDriver());
        }
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _sceneObject.Open();
        }

        [UnityTest]
        public IEnumerator ページAに画面遷移で往復できる() => UniTask.ToCoroutine(async () =>
        {
            await _sceneObject.TopPage.Page
                .ShouldBe(Condition.Active);
            await _sceneObject.SubPageA.Page
                .ShouldBe(Condition.Inactive);
            await _sceneObject.SubPageB.Page
                .ShouldBe(Condition.Inactive);

            await _sceneObject.TopPage.SubPageAButton
                .Click();
            await _sceneObject.TopPage.Page
                .ShouldBe(Condition.Inactive);
            await _sceneObject.SubPageA.Page
                .ShouldBe(Condition.Active);
            await _sceneObject.SubPageB.Page
                .ShouldBe(Condition.Inactive);

            await _sceneObject.SubPageA.BackButton
                .Click();
            await _sceneObject.TopPage.Page
                .ShouldBe(Condition.Active);
            await _sceneObject.SubPageA.Page
                .ShouldBe(Condition.Inactive);
            await _sceneObject.SubPageB.Page
                .ShouldBe(Condition.Inactive);
        });

        [UnityTest]
        public IEnumerator ページBに画面遷移で往復できる() => UniTask.ToCoroutine(async () =>
        {
            await _sceneObject.TopPage.Page
                .ShouldBe(Condition.Active);
            await _sceneObject.SubPageA.Page
                .ShouldBe(Condition.Inactive);
            await _sceneObject.SubPageB.Page
                .ShouldBe(Condition.Inactive);

            await _sceneObject.TopPage.SubPageBButton
                .Click();
            await _sceneObject.TopPage.Page
                .ShouldBe(Condition.Inactive);
            await _sceneObject.SubPageA.Page
                .ShouldBe(Condition.Inactive);
            await _sceneObject.SubPageB.Page
                .ShouldBe(Condition.Active);

            await _sceneObject.SubPageB.BackButton
                .Click();
            await _sceneObject.TopPage.Page
                .ShouldBe(Condition.Active);
            await _sceneObject.SubPageA.Page
                .ShouldBe(Condition.Inactive);
            await _sceneObject.SubPageB.Page
                .ShouldBe(Condition.Inactive);
        });
    }
}

/// <summary>
/// Scene内にあるGameObjectをView単位にInnerClass化したViewObject
/// </summary>
public sealed class SampleuGUISceneAndViewObject : SceneObjectBase
{
    public override string SceneName => "Sample-uGUI";

    public TopPageObject TopPage { get; }
    public SubPageAObject SubPageA { get; }
    public SubPageBObject SubPageB { get; }

    public SampleuGUISceneAndViewObject(IUnideDriver d) : base(d)
    {
        TopPage = new TopPageObject(this);
        SubPageA = new SubPageAObject(this);
        SubPageB = new SubPageBObject(this);
    }
    
    public sealed class TopPageObject : ViewObjectBase
    {
        public TopPageObject(SceneObjectBase sceneObject) : base(sceneObject)
        { }
        public UniTask<UnideQuery> Page => Q.ByName("TopPage");
        public UniTask<UnideQuery> SubPageAButton => Page.ByName("SubPageAButton");
        public UniTask<UnideQuery> SubPageBButton => Page.ByName("SubPageBButton");
    }
    
    public sealed class SubPageAObject : ViewObjectBase
    {
        public SubPageAObject(SceneObjectBase sceneObject) : base(sceneObject)
        { }
        public UniTask<UnideQuery> Page => Q.ByName("SubPageA");
        public UniTask<UnideQuery> BackButton => Page.ByName("BackButton");
    }
    
    public sealed class SubPageBObject : ViewObjectBase
    {
        public SubPageBObject(SceneObjectBase sceneObject) : base(sceneObject)
        { }
        public UniTask<UnideQuery> Page => Q.ByName("SubPageB");
        public UniTask<UnideQuery> BackButton => Page.ByName("BackButton");
    }
}
