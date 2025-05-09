﻿using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace unide.Tests
{
    public sealed class UnideDriverAndQueryTests
    {
        private readonly IUnideDriver D;
        private readonly UnideQuerySource _querySource;

        private UniTask<UnideQuery> Q => _querySource.CreateQueryContext();

        public UnideDriverAndQueryTests()
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
        public IEnumerator 全列挙できる() => UniTask.ToCoroutine(async () =>
        {
            var all = D.FindAll();
            foreach (var item in all)
            {
                Debug.Log($"name: {item.name}");
            }
        });

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
        public IEnumerator Componentで検索できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByComponent<Button>();
        });

        [UnityTest]
        public IEnumerator Atで子要素取得できる() => UniTask.ToCoroutine(async () =>
        {
            var child0 = await Q.ByName("ChildSwitcher")
                .At(0);
            Assert.AreEqual(child0.Target.gameObject.name, "TopPage");
            
            var child1 = await Q.ByName("ChildSwitcher")
                .At(1);
            Assert.AreEqual(child1.Target.gameObject.name, "SubPageA");
        });
        
        [UnityTest]
        public IEnumerator Nameで検索して該当なしでnullが返る() => UniTask.ToCoroutine(async () =>
        {
            var results = await Q.ByName("unknown");
            Assert.IsNull(results.Target);
        });

        [UnityTest]
        public IEnumerator Tagで検索して該当なしでnullが返る() => UniTask.ToCoroutine(async () =>
        {
            var results = await Q.ByTag("unknown");
            Assert.IsNull(results.Target);
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
        public IEnumerator ShouldHaveでtextを確認できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByName("LabelA")
                .ShouldHave("LabelA");
        });

        [UnityTest]
        public IEnumerator ShouldHaveでInputFieldを確認できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByName("InputFieldA")
                .SetText("input value");
            await Q.ByName("InputFieldA")
                .ShouldHave("input value");
        });

        [UnityTest]
        public IEnumerator InputFieldに入力テキストを設定できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByName("InputFieldA")
                .SetText("input value 1");
            var value = await Q.ByName("InputFieldA")
                .GetText();
            Assert.AreEqual("input value 1", value);
        });

        [UnityTest]
        public IEnumerator Sliderに値を設定できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByName("SliderA")
                .SetValue(0.1f);
            var value01 = await Q.ByName("SliderA")
                .GetFloat();
            Assert.AreEqual(value01, 0.1f);

            await Q.ByName("SliderA")
                .SetValue(0.5f);
            var value05 = await Q.ByName("SliderA")
                .GetFloat();
            Assert.AreEqual(value05, 0.5f);
        });

        [UnityTest]
        public IEnumerator ScrollRectを指定箇所にスクロールできる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByName("Scroll View")
                .SetScrollPositionVertical(0.25f);
            var scrollView = await Q.ByName("Scroll View");
            Assert.AreEqual(scrollView.Target.GetComponent<ScrollRect>().normalizedPosition.y, 0.25f);
        });

        [UnityTest]
        public IEnumerator ShouldHaveでtextが成立せず例外がでる() => UniTask.ToCoroutine(async () =>
        {
            var throwsException = false;
            try
            {
                await Q.ByName("LabelA")
                    .ShouldHave("not LabelA");
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throwsException = true;
            }
            Assert.IsTrue(throwsException);
        });

        [UnityTest]
        public IEnumerator ShouldBeでNonInteractiveを確認できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByName("BackButton")
                .ShouldBe(Condition.NonInteractive);
        });

        [UnityTest]
        public IEnumerator ShouldBeでDisappearを確認できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByName("<<element not exist>>")
                .ShouldBe(Condition.Disappear);
        });
        
        [UnityTest]
        public IEnumerator Timeout指定できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByName("SubPageAButton")
                .SetTimeout(10000)
                .Click();
            await Q.ByName("BackButton")
                .Click();
        });
        
        [UnityTest]
        public IEnumerator Delay指定できる() => UniTask.ToCoroutine(async () =>
        {
            await Q.ByName("BackButton")
                .SetDelay(3000)
                .ShouldBe(Condition.NonInteractive);
        });
    }
}
