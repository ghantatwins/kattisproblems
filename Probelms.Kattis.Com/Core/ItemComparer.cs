using System.Collections.Generic;

namespace Probelms.Kattis.Com.Core
{
    public abstract class ItemComparer:IComparer<IItem>
    {
        public int Compare(IItem x, IItem y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return 1;
            return ActualCompare(x,y);
        }

        protected abstract int ActualCompare(IItem item1,IItem item2);
    }
}