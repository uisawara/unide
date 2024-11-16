using System.Collections;
using System.IO;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Samples.Sample_uGUI.Tests
{
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
            _querySource.BaseScreenshotPath = Path.Combine(Application.dataPath, "../TestResults/IntegrationTests/Screenshots/");
            _querySource.EnableCaptureScreenshotBeforeClick = true;
        }
    
        [UnityTest]
        public IEnumerator ページAに画面遷移で往復できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByName("TopPage")
                .ShouldBe(Condition.Active);
            await Q.ByName("SubPageA")
                .ShouldBe(Condition.Inactive);
            await Q.ByName("SubPageB")
                .ShouldBe(Condition.Inactive);

            await Q.ByName("SubPageAButton")
                .Click();
            await Q.ByName("TopPage")
                .ShouldBe(Condition.Inactive);
            await Q.ByName("SubPageA")
                .ShouldBe(Condition.Active);
            await Q.ByName("SubPageB")
                .ShouldBe(Condition.Inactive);

            await Q.ByName("SubPageA")
                .ByName("BackButton")
                .Click();
            await Q.ByName("TopPage")
                .ShouldBe(Condition.Active);
            await Q.ByName("SubPageA")
                .ShouldBe(Condition.Inactive);
            await Q.ByName("SubPageB")
                .ShouldBe(Condition.Inactive);
        });
    
        [UnityTest]
        public IEnumerator ページBに画面遷移で往復できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByName("TopPage")
                .ShouldBe(Condition.Active);
            await Q.ByName("SubPageA")
                .ShouldBe(Condition.Inactive);
            await Q.ByName("SubPageB")
                .ShouldBe(Condition.Inactive);

            await Q.ByName("SubPageBButton")
                .Click();
            await Q.ByName("TopPage")
                .ShouldBe(Condition.Inactive);
            await Q.ByName("SubPageA")
                .ShouldBe(Condition.Inactive);
            await Q.ByName("SubPageB")
                .ShouldBe(Condition.Active);

            await Q.ByName("SubPageB")
                .ByName("BackButton")
                .Click();
            await Q.ByName("TopPage")
                .ShouldBe(Condition.Active);
            await Q.ByName("SubPageA")
                .ShouldBe(Condition.Inactive);
            await Q.ByName("SubPageB")
                .ShouldBe(Condition.Inactive);
        });
    }
}
