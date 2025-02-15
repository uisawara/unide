using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace unide
{
    public static class UnideQueryExtensions
    {
        public static async UniTask<UnideQuery> ByName(this UniTask<UnideQuery> self, string name)
        {
            var context = await self;
            try
            {
                if (context.Target == null)
                {
                    await UniTask.WaitWhile(() => context.TestDriver.FindObjectByName(name) == null)
                        .WithTimeout(context.Timeout);
                    var gameObject = context.TestDriver.FindObjectByName(name);
                    context.Target = gameObject;
                }
                else
                {
                    await UniTask.WaitWhile(() => context.TestDriver.FindChildByNameDepth(context.Target, name) == null)
                        .WithTimeout(context.Timeout);
                    var gameObject = context.TestDriver.FindChildByNameDepth(context.Target, name);
                    context.Target = gameObject;
                }
            }
            catch (TimeoutException e)
            {
                context.Target = null;
            }

            return context;
        }

        public static async UniTask<UnideQuery> ByTag(this UniTask<UnideQuery> self, string tag)
        {
            var context = await self;
            try
            {
                if (context.Target == null)
                {
                    await UniTask.WaitWhile(() => context.TestDriver.FindObjectByTag(tag) == null)
                        .WithTimeout(context.Timeout);
                    var gameObject = context.TestDriver.FindObjectByTag(tag);
                    context.Target = gameObject;
                }
                else
                {
                    await UniTask.WaitWhile(() => context.TestDriver.FindChildByTagDepth(context.Target, tag) == null)
                        .WithTimeout(context.Timeout);
                    var gameObject = context.TestDriver.FindChildByTagDepth(context.Target, tag);
                    context.Target = gameObject;
                }
            }
            catch (TimeoutException e)
            {
                context.Target = null;
            }

            return context;
        }

        public static async UniTask<UnideQuery> ByComponent<TComponent>(this UniTask<UnideQuery> self)
            where TComponent : Component
        {
            var context = await self;
            try
            {
                if (context.Target == null)
                {
                    await UniTask.WaitWhile(() => context.TestDriver.FindObjectByComponent<TComponent>() == null)
                        .WithTimeout(context.Timeout);
                    var gameObject = context.TestDriver.FindObjectByComponent<TComponent>();
                    context.Target = gameObject;
                }
                else
                {
                    await UniTask.WaitWhile(() =>
                            context.TestDriver.FindChildByComponentDepth<TComponent>(context.Target) == null)
                        .WithTimeout(context.Timeout);
                    var gameObject = context.TestDriver.FindObjectByComponent<TComponent>();
                    context.Target = gameObject;
                }
            }
            catch (TimeoutException e)
            {
                context.Target = null;
            }

            return context;
        }

        public static async UniTask<UnideQuery> At(this UniTask<UnideQuery> self, int childIndex)
        {
            var context = await self;
            try
            {
                await UniTask.WaitWhile(() => context.Target.transform.GetChild(childIndex)
                        .gameObject == null)
                    .WithTimeout(context.Timeout);
                context.Target = context.Target.transform.GetChild(childIndex)
                    .gameObject;
            }
            catch (TimeoutException e)
            {
                context.Target = null;
            }

            return context;
        }
    }
}
