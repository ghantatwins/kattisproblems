
using System;

namespace Probelms.Kattis.Com.Core {
   /// <summary>
  /// Interface to represent (almost) every HeuristicLab object (an object, an operator,...).
  /// </summary>
  public interface IItem : IContent, IDeepCloneable {
    string ItemName { get; }
    string ItemDescription { get; }
    Version ItemVersion { get; }
    
    event EventHandler ToStringChanged;
       bool Constraint(IItem nestedItem);
       void Print(IPrinter printer);
   }
}
