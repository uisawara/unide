using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Samples.Sample_uGUI.Tests
{
    public sealed class UnideTests
    {
        private IUnideDriver _driver = new UnideDriver();
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _driver.Open("Sample-uGUI");
        }
    
        [UnityTest]
        public IEnumerator Nameで検索できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByName("TopPage");
        });

        [UnityTest]
        public IEnumerator Tagで検索できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByTag("MainCamera");
        });

        [UnityTest]
        public IEnumerator Nameで検索して該当なしで例外がでる() => UniTask.ToCoroutine(async () =>
        {
            throw new NotImplementedException();
        });

        [UnityTest]
        public IEnumerator Tagで検索して該当なしで例外がでる() => UniTask.ToCoroutine(async () =>
        {
            throw new NotImplementedException();
        });

        [UnityTest]
        public IEnumerator ShouldBeでActiveを確認できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByName("TopPage")
                .ShouldBe(Condition.Active);
        });

        [UnityTest]
        public IEnumerator ShouldBeでInactiveを確認できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByName("SubPageA")
                .ShouldBe(Condition.Inactive);
        });

        [UnityTest]
        public IEnumerator ShouldBeでInteractiveを確認できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByName("SubPageAButton")
                .ShouldBe(Condition.Interactive);
        });

        [UnityTest]
        public IEnumerator ShouldBeでNonInteractiveを確認できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByName("BackButton")
                .ShouldBe(Condition.NonInteractive);
        });
    }
}
