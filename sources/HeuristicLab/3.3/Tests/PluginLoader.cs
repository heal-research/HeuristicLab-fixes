﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HeuristicLab.PluginInfrastructure;

namespace HeuristicLab_33.Tests {
  internal static class PluginLoader {
    public const string AssemblyExtension = ".dll";
    public const string TestAccessorAssemblyExtension = "_Accessor.dll";
    public const string TestAssemblyExtension = ".test.dll";
    public static List<Assembly> pluginAssemblies;

    static PluginLoader() {
      foreach (string path in Directory.EnumerateFiles(Environment.CurrentDirectory)
        .Where(s => s.EndsWith(AssemblyExtension) && !s.EndsWith(TestAccessorAssemblyExtension) && !s.EndsWith(TestAssemblyExtension)))
        Assembly.LoadFrom(path);

      pluginAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(IsPluginAssembly).ToList();
    }

    private static bool IsPluginAssembly(Assembly assembly) {
      return assembly.GetExportedTypes()
        .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface).Any();
    }
  }
}
