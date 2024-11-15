﻿using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Samples.Sample_uGUI.Tests
{
    public sealed class SampleuGUISceneTests
    {
        private IUnideDriver D { get; }
        private UnideContext Q { get; }

        public SampleuGUISceneTests()
        {
            D = new UnideDriver();
            Q = new UnideContext(D);
        }
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            D.Open("Sample-uGUI");
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

            await Q.ByName("BackButton")
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

            await Q.ByName("BackButton")
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
