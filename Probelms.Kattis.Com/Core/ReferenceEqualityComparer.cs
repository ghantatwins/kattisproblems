
using System.Collections.Generic;

namespace Probelms.Kattis.Com.Core {
  public class ReferenceEqualityComparer : IEqualityComparer<object> {
    bool IEqualityComparer<object>.Equals(object x, object y) {
      return object.ReferenceEquals(x, y);
    }

    int IEqualityComparer<object>.GetHashCode(object obj) {
        return obj.GetHashCode();
    }
  }
}
