using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using unide;
using UnityEngine.TestTools;

namespace Samples.Sample_uGUI.Tests
{
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
            await _sceneObject.TopPage
                .ShouldBe(Condition.Active);
            await _sceneObject.SubPageA
                .ShouldBe(Condition.Inactive);
            await _sceneObject.SubPageB
                .ShouldBe(Condition.Inactive);

            await _sceneObject.SubPageAButton
                .Click();
            await _sceneObject.TopPage
                .ShouldBe(Condition.Inactive);
            await _sceneObject.SubPageA
                .ShouldBe(Condition.Active);
            await _sceneObject.SubPageB
                .ShouldBe(Condition.Inactive);

            await _sceneObject.SubPageABackButton
                .Click();
            await _sceneObject.TopPage
                .ShouldBe(Condition.Active);
            await _sceneObject.SubPageA
                .ShouldBe(Condition.Inactive);
            await _sceneObject.SubPageB
                .ShouldBe(Condition.Inactive);
        });

        [UnityTest]
        public IEnumerator ページBに画面遷移で往復できる() => UniTask.ToCoroutine(async () =>
        {
            await _sceneObject.TopPage
                .ShouldBe(Condition.Active);
            await _sceneObject.SubPageA
                .ShouldBe(Condition.Inactive);
            await _sceneObject.SubPageB
                .ShouldBe(Condition.Inactive);

            await _sceneObject.SubPageBButton
                .Click();
            await _sceneObject.TopPage
                .ShouldBe(Condition.Inactive);
            await _sceneObject.SubPageA
                .ShouldBe(Condition.Inactive);
            await _sceneObject.SubPageB
                .ShouldBe(Condition.Active);

            await _sceneObject.SubPageBBackButton
                .Click();
            await _sceneObject.TopPage
                .ShouldBe(Condition.Active);
            await _sceneObject.SubPageA
                .ShouldBe(Condition.Inactive);
            await _sceneObject.SubPageB
                .ShouldBe(Condition.Inactive);
        });
    }
}

/// <summary>
/// シーン内のGameObjectを列挙したSceneObject
/// </summary>
public sealed class SampleuGUISceneObject : SceneObjectBase
{
    public override string SceneName => "Sample-uGUI";

    public SampleuGUISceneObject(IUnideDriver d) : base(d)
    { }

    public UniTask<UnideQuery> TopPage => Q.ByName("TopPage");
    public UniTask<UnideQuery> SubPageA => Q.ByName("SubPageA");
    public UniTask<UnideQuery> SubPageB => Q.ByName("SubPageB");
    public UniTask<UnideQuery> SubPageAButton => TopPage.ByName("SubPageAButton");
    public UniTask<UnideQuery> SubPageBButton => TopPage.ByName("SubPageBButton");
    public UniTask<UnideQuery> SubPageABackButton => SubPageA.ByName("BackButton");
    public UniTask<UnideQuery> SubPageBBackButton => SubPageB.ByName("BackButton");
}
