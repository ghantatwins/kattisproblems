
using System.Collections.Generic;

namespace Probelms.Kattis.Com.Core {
  public class TypeEqualityComparer<T> : IEqualityComparer<T> {

    bool IEqualityComparer<T>.Equals(T x, T y) {
      return x.GetType().Equals(y.GetType());
    }

    int IEqualityComparer<T>.GetHashCode(T obj) {
      if (obj == null) return 0;
      return obj.GetType().GetHashCode();
    }
  }
}
