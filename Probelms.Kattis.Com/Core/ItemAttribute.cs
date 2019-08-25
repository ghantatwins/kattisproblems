using System;
using System.Linq;
using System.Reflection;

namespace Probelms.Kattis.Com.Core {
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public sealed class ItemAttribute : Attribute {
    private string name;
    public string Name {
      get { return name; }
      set { name = value == null ? string.Empty : value; }
    }
    private string description;
    public string Description {
      get { return description; }
      set { description = value == null ? string.Empty : value; }
    }
    public bool ExcludeGenericTypeInfo { get; set; }

    public ItemAttribute() : this(string.Empty, string.Empty, false) { }
    public ItemAttribute(string name, string description) : this(name, description, false) { }
    public ItemAttribute(string name, string description, bool excludeGenericTypeInfo) {
      Name = name;
      Description = description;
      ExcludeGenericTypeInfo = excludeGenericTypeInfo;
    }

    public static string GetName(Type type) {
      object[] attribs = type.GetCustomAttributes(typeof(ItemAttribute), false);
      if (attribs.Length > 0) {
        var attribute = (ItemAttribute)attribs[0];
        string name = attribute.Name;
        if (!attribute.ExcludeGenericTypeInfo && type.IsGenericType) {
          name += "<";
          Type[] typeParams = type.GetGenericArguments();
          if (typeParams.Length > 0) {
            name += GetName(typeParams[0]);
            for (int i = 1; i < typeParams.Length; i++)
              name += ", " + GetName(typeParams[i]);
          }
          name += ">";
        }
        return name;
      } else {
        return type.GetPrettyName();
      }
    }
    public static string GetDescription(Type type) {
      object[] attribs = type.GetCustomAttributes(typeof(ItemAttribute), false);
      if (attribs.Length > 0) return ((ItemAttribute)attribs[0]).Description;
      else return string.Empty;
    }
    public static Version GetVersion(Type type) {
      Assembly assembly = Assembly.GetAssembly(type);
      AssemblyFileVersionAttribute version = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true).
                                             Cast<AssemblyFileVersionAttribute>().FirstOrDefault();
      if (version != null) {
        return new Version(version.Version);
      } else {
        return assembly.GetName().Version;
      }
    }
    
  }
}
