using UnityEngine;

namespace unide
{
    public sealed class UnideQuery
    {
        public UnideQuerySource QuerySource { get; }
        public IUnideDriver TestDriver => QuerySource.TestDriver;
    
        public GameObject Target { get; set; }
        public int Timeout { get; set; }
        public int Delay { get; set; }

        public UnideQuery(UnideQuerySource querySource)
        {
            QuerySource = querySource;
            Timeout = QuerySource.Timeout;
            Delay = QuerySource.Delay;
        }
    }
}