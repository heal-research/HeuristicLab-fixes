﻿using System;
using System.Collections;
using System.Reflection;
using HeuristicLab.Persistence.Core;
using HeuristicLab.Persistence.Interfaces;
using System.Collections.Generic;
using HeuristicLab.Persistence.Default.Decomposers.Storable;
using HeuristicLab.Persistence.Auxiliary;

namespace HeuristicLab.Persistence.Default.Decomposers {

  [EmptyStorableClass]
  public class EnumerableDecomposer : IDecomposer {

    public int Priority {
      get { return 100; }
    }


    public bool CanDecompose(Type type) {
      return
        ReflectionTools.HasDefaultConstructor(type) &&
        type.GetInterface(typeof(IEnumerable).FullName) != null &&
        type.GetMethod("Add") != null &&
        type.GetMethod("Add").GetParameters().Length == 1;
    }

    public IEnumerable<Tag> CreateMetaInfo(object o) {
      return new Tag[] { };
    }

    public IEnumerable<Tag> Decompose(object obj) {
      foreach (object o in (IEnumerable)obj) {
        yield return new Tag(o);
      }
    }

    public object CreateInstance(Type type, IEnumerable<Tag> metaInfo) {
      return Activator.CreateInstance(type, true);
    }

    public void Populate(object instance, IEnumerable<Tag> tags, Type type) {
      MethodInfo addMethod = type.GetMethod("Add");
      try {
        foreach (var tag in tags)
          addMethod.Invoke(instance, new[] { tag.Value });
      } catch (Exception e) {
        throw new PersistenceException("Exception caught while trying to populate enumerable.", e);
      }
    }
  }
}