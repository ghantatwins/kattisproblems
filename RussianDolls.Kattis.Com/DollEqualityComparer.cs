using System.Collections.Generic;
using Probelms.Kattis.Com;
using Probelms.Kattis.Com.Core;

namespace RussianDolls.Kattis.Com
{
    public sealed class DollEqualityComparer: ReferenceEqualityComparer,IEqualityComparer<Doll>
    {
        public bool Equals(Doll x, Doll y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Diameter == y.Diameter && x.Height == y.Height && x.Width == y.Width;
        }

        public int GetHashCode(Doll obj)
        {
            unchecked
            {
                var hashCode = obj.Diameter;
                hashCode = (hashCode * 397) ^ obj.Height;
                hashCode = (hashCode * 397) ^ obj.Width;
                return hashCode;
            }
        }
    }
}