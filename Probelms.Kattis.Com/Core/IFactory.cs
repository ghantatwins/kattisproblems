using System.Collections.Generic;

namespace Probelms.Kattis.Com.Core
{
    public interface IFactory<T>
        where T : IItem
    {
        int TotalItems { get; }
        T Create();
        IEqualityComparer<T> EqualityComparer { get; }
        IComparer<T> Comparer { get; }
        void Reset();
    }
}