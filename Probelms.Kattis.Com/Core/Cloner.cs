
using System.Collections.Generic;

namespace Probelms.Kattis.Com.Core {
  /// <summary>
  /// A helper class which is used to create deep clones of object graphs.
  /// </summary>
  public sealed class Cloner {
    private Dictionary<IDeepCloneable, IDeepCloneable> mapping;

    /// <summary>
    /// Creates a new Cloner instance.
    /// </summary>
    public Cloner() {
      mapping = new Dictionary<IDeepCloneable, IDeepCloneable>(new ReferenceEqualityComparer());
    }

    /// <summary>
    /// Creates a deep clone of a given deeply cloneable object.
    /// </summary>
    /// <param name="item">The object which should be cloned.</param>
    /// <returns>A clone of the given object.</returns>
    public T Clone<T>(T obj) where T : class, IDeepCloneable {
      if (obj == null) return null;
      IDeepCloneable clone;
      if (mapping.TryGetValue(obj, out clone))
        return (T)clone;
      else
        return (T)obj.Clone(this);
    }
    /// <summary>
    /// Registers a new clone for a given deeply cloneable object.
    /// </summary>
    /// <param name="item">The original object.</param>
    /// <param name="clone">The clone of the original object.</param>
    public void RegisterClonedObject(IDeepCloneable item, IDeepCloneable clone) {
      mapping.Add(item, clone);
    }

    /// <summary>
    /// Checks if a clone is already registered for a given deeply cloneable item.
    /// </summary>
    /// <param name="item">The original object.</param>
    /// <returns>True if a clone is already registered for the given item; false otherwise</returns>
    public bool ClonedObjectRegistered(IDeepCloneable item) {
      return mapping.ContainsKey(item);
    }

    /// <summary>
    /// Returns the clone of an deeply cloned item, if it was already cloned.
    /// </summary>
    /// <param name="original">The original object.</param>
    /// <returns>The clone of the given object, if it was already cloned; null otherwise</returns>
    public IDeepCloneable GetClone(IDeepCloneable original) {
      IDeepCloneable clone = null;
      mapping.TryGetValue(original, out clone);
      return clone;
    }

  }
}
