using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Probelms.Kattis.Com.Core {
  public static class TypeExtensions {
    public static string GetPrettyName(this Type x) {
      return x.GetPrettyName(false);
    }
    public static string GetPrettyName(this Type x, bool includeNamespace) {
      StringBuilder sb = new StringBuilder();
      if ((includeNamespace) && !string.IsNullOrEmpty(x.Namespace))
        sb.Append(x.Namespace).Append(".");
      sb.Append(x.Name);
      if (x.IsGenericType) {
        Type[] genericParams = x.GetGenericArguments();
        sb.Remove(sb.ToString().LastIndexOf('`'), genericParams.Length.ToString().Length + 1);
        sb.Append("<");
        for (int i = 0; i < genericParams.Length; i++) {
          if (i > 0) sb.Append(", ");
          sb.Append(genericParams[i].GetPrettyName(includeNamespace));
        }
        sb.Append(">");
      }
      return sb.ToString();
    }

    public static IEnumerable<FieldInfo> GetAllFields(this Type type) {
      while (type != null) {
        foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic))
          yield return field;
        type = type.BaseType;
      }
    }

    // http://stackoverflow.com/questions/457676/c-reflection-check-if-a-class-is-derived-from-a-generic-class
    public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic) {
      while (toCheck != typeof(object)) {
        var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
        if (generic == cur) {
          return true;
        }
        toCheck = toCheck.BaseType; // baseType is null when toCheck is an Interface
        if (toCheck == null)
          return false;
      }
      return false;
    }
  }
}
