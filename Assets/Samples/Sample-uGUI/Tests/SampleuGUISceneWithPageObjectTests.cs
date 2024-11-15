using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Samples.Sample_uGUI.Tests
{
    public sealed class SampleuGUISceneWithPageObjectTests
    {
        private readonly IUnideDriver D;
        private readonly SampleuGUISceneObject _sceneObject;

        public SampleuGUISceneWithPageObjectTests()
        {
            D = new UnideDriver();
            _sceneObject = new SampleuGUISceneObject(D);
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

            await _sceneObject.BackButton
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

            await _sceneObject.BackButton
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
