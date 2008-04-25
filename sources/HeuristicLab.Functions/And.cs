#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2008 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
 *
 * This file is part of HeuristicLab.
 *
 * HeuristicLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * HeuristicLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with HeuristicLab. If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeuristicLab.Constraints;
using HeuristicLab.DataAnalysis;

namespace HeuristicLab.Functions {
  public class And : FunctionBase {
    public override string Description {
      get {
        return @"Logical AND operation. Only defined for sub-tree-results 0.0 and 1.0.
AND is a special form, sub-trees are evaluated from the first to the last. Evaluation is 
stopped as soon as one of the sub-trees evaluates to 0.0 (false).";
      }
    }

    public And()
      : base() {
      AddConstraint(new NumberOfSubOperatorsConstraint(2, 3));
    }

    public override IFunctionTree GetTreeNode() {
      return new AndFunctionTree(this);
    }

    // special form
    public override double Apply(Dataset dataset, int sampleIndex, double[] args) {
      throw new NotImplementedException();
    }

    public override void Accept(IFunctionVisitor visitor) {
      visitor.Visit(this);
    }
  }

  class AndFunctionTree : FunctionTree {
    public AndFunctionTree() : base() { }
    public AndFunctionTree(And and) : base(and) { }

    public override double Evaluate(Dataset dataset, int sampleIndex) {
      foreach(IFunctionTree subTree in SubTrees) {
        double result = Math.Round(subTree.Evaluate(dataset, sampleIndex));
        if(result == 0.0) return 0.0; // one sub-tree is 0.0 (false) => return false
        else if(result != 1.0) return double.NaN;
      }
      // all sub-trees evaluated to 1.0 (true) => return 1.0 (true)
      return 1.0;      
    }
  }
}
