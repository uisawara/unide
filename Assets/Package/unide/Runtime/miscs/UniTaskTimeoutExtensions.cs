using System;
using Cysharp.Threading.Tasks;

namespace unide
{
    public static class UniTaskTimeoutExtensions
    {
        public static async UniTask WithTimeout(this UniTask task, int timeout)
        {
            var timeoutTask = UniTask.Delay(timeout);
            if (await UniTask.WhenAny(timeoutTask, task) == 0)
            {
                throw new TimeoutException("The task has timed out.");
            }
        }
    }
}