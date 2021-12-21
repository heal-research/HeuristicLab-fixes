﻿using System;
using System.Collections.Generic;
using System.Linq;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;

namespace HeuristicLab.Problems.DataAnalysis.Symbolic {
  [StorableType("E3C038DB-C6AA-457D-9F65-AF16C44CCE22")]
  [Item("StructureTemplate", "Structure Template")]
  public class StructureTemplate : Item {

    #region Properties
    [Storable]
    private string template;
    public string Template {
      get => template;
      set {
        template = value;
        Tree = Parser.Parse(template);
        OnChanged();
      }
    }

    [Storable]
    private ISymbolicExpressionTree treeWithoutLinearScaling;
    [Storable]
    private ISymbolicExpressionTree treeWithLinearScaling;
    public ISymbolicExpressionTree Tree {
      get => ApplyLinearScaling ? treeWithLinearScaling : treeWithoutLinearScaling;
      private set {
        treeWithLinearScaling = AddLinearScalingTerms(value);
        treeWithoutLinearScaling = value;

        var newFunctions = GetSubFunctions();
        var oldFunctions = subFunctions?.Intersect(newFunctions) 
                           ?? Enumerable.Empty<SubFunction>();
        // adds new functions and keeps the old ones (if they match)
        subFunctions = newFunctions.Except(oldFunctions).Concat(oldFunctions).ToList();
      }
    }

    [Storable]
    private IList<SubFunction> subFunctions = new List<SubFunction>();
    public IEnumerable<SubFunction> SubFunctions => subFunctions;

    [Storable]
    private bool applyLinearScaling;
    public bool ApplyLinearScaling {
      get => applyLinearScaling;
      set {
        applyLinearScaling = value;
        OnChanged();
      }
    }

    protected InfixExpressionParser Parser { get; set; } = new InfixExpressionParser();

    #endregion

    #region Events
    public event EventHandler Changed;

    private void OnChanged() => Changed?.Invoke(this, EventArgs.Empty);
    #endregion

    #region Constructors
    public StructureTemplate() {
      Reset();
    }

    [StorableConstructor]
    protected StructureTemplate(StorableConstructorFlag _) : base(_) { }

    protected StructureTemplate(StructureTemplate original, Cloner cloner) : base(original, cloner) {
      this.Tree = cloner.Clone(original.Tree);
      this.Template = original.Template;
      this.ApplyLinearScaling = original.ApplyLinearScaling;
      this.subFunctions = original.subFunctions.Select(cloner.Clone).ToList();
      RegisterEventHandlers();
    }


    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      RegisterEventHandlers();
    }
    #endregion

    #region Cloning
    public override IDeepCloneable Clone(Cloner cloner) =>
      new StructureTemplate(this, cloner);
    #endregion

    public void Reset() {
      subFunctions = new List<SubFunction>();
      treeWithoutLinearScaling = null;
      treeWithLinearScaling = null;
      applyLinearScaling = false;
      Template = "f(_)";
      OnChanged();
    }

    private IList<SubFunction> GetSubFunctions() {
      var subFunctions = new List<SubFunction>();
      foreach (var node in Tree.IterateNodesPrefix())
        if (node is SubFunctionTreeNode subFunctionTreeNode) {
          if (!subFunctionTreeNode.Arguments.Any())
            throw new ArgumentException($"The sub-function '{subFunctionTreeNode}' requires inputs (e.g. {subFunctionTreeNode.Name}(var1, var2)).");

          var existingSubFunction = subFunctions.Where(x => x.Name == subFunctionTreeNode.Name).FirstOrDefault();
          if (existingSubFunction != null) {
            // an existing subFunction needs the same signature
            if(!existingSubFunction.Arguments.SequenceEqual(subFunctionTreeNode.Arguments))
              throw new ArgumentException(
                $"The sub-function '{existingSubFunction.Name}' has (at least two) different signatures " +
                $"({existingSubFunction.Name}({string.Join(",", existingSubFunction.Arguments)}) <> " +
                $"{subFunctionTreeNode.Name}({string.Join(",", subFunctionTreeNode.Arguments)})).");
          } else {
            var subFunction = new SubFunction() {
              Name = subFunctionTreeNode.Name,
              Arguments = subFunctionTreeNode.Arguments
            };
            subFunction.Changed += OnSubFunctionChanged;
            subFunctions.Add(subFunction);
          }
        }
      return subFunctions;
    }

    private void RegisterEventHandlers() {
      foreach(var sf in SubFunctions) {
        sf.Changed += OnSubFunctionChanged;
      }
    }

    private ISymbolicExpressionTree AddLinearScalingTerms(ISymbolicExpressionTree tree) {
      var clonedTree = (ISymbolicExpressionTree)tree.Clone();
      var startNode = clonedTree.Root.Subtrees.First();
      var template = startNode.Subtrees.First();

      var add = new Addition();
      var addNode = add.CreateTreeNode();

      var mul = new Multiplication();
      var mulNode = mul.CreateTreeNode();

      var offset = new Number();
      var offsetNode = (NumberTreeNode)offset.CreateTreeNode();
      offsetNode.Value = 0.0;
      var scale = new Number();
      var scaleNode = (NumberTreeNode)scale.CreateTreeNode();
      scaleNode.Value = 1.0;

      addNode.AddSubtree(offsetNode);
      addNode.AddSubtree(mulNode);
      mulNode.AddSubtree(scaleNode);

      startNode.RemoveSubtree(0);
      startNode.AddSubtree(addNode);
      mulNode.AddSubtree(template);
      return clonedTree;
    }

    private void OnSubFunctionChanged(object sender, EventArgs e) => OnChanged();
  }
}
