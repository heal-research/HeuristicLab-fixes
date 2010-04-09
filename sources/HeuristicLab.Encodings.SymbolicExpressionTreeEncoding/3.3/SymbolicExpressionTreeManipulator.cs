﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2010 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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

using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Operators;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Encodings.SymbolicExpressionTreeEncoding {
  /// <summary>
  /// A base class for operators that manipulate real-valued vectors.
  /// </summary>
  [Item("SymbolicExpressionTreeManipulator", "A base class for operators that manipulate symbolic expression trees.")]
  [StorableClass]
  public abstract class SymbolicExpressionTreeManipulator : SymbolicExpressionTreeOperator, IManipulator {
    private const string FailedManipulationEventsParameterName = "FailedManipulationEvents";

    #region Parameter Properties
    public IValueParameter<IntValue> FailedManipulationEventsParameter {
      get { return (IValueParameter<IntValue>)Parameters[FailedManipulationEventsParameterName]; }
    }
    #endregion

    #region Properties
    public IntValue FailedManipulationEvents {
      get { return FailedManipulationEventsParameter.Value; }
    }
    #endregion

    public SymbolicExpressionTreeManipulator()
      : base() {
      Parameters.Add(new ValueParameter<IntValue>(FailedManipulationEventsParameterName, "The number of failed manipulation events.", new IntValue()));
    }

    public sealed override IOperation Apply() {
      SymbolicExpressionTree tree = SymbolicExpressionTreeParameter.ActualValue;
      bool success;
      Manipulate(RandomParameter.ActualValue, tree, SymbolicExpressionGrammarParameter.ActualValue,
        MaxTreeSizeParameter.ActualValue, MaxTreeHeightParameter.ActualValue, out success);

      if (!success) FailedManipulationEvents.Value++;
      return base.Apply();
    }

    protected abstract void Manipulate(IRandom random, SymbolicExpressionTree symbolicExpressionTree, ISymbolicExpressionGrammar grammar,
      IntValue maxTreeSize, IntValue maxTreeHeight, out bool success);
  }
}