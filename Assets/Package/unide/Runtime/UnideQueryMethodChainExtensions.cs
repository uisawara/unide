using Cysharp.Threading.Tasks;

namespace unide
{
    public static class UnideQueryMethodChainExtensions
    {
        public static async UniTask<UnideQuery> SetTimeout(this UniTask<UnideQuery> self, int timeout)
        {
            var context = await self;
            context.Timeout = timeout;
            return context;
        }

        public static async UniTask<UnideQuery> SetDelay(this UniTask<UnideQuery> self, int delay)
        {
            var context = await self;
            context.Delay = delay;
            return context;
        }
    }
}