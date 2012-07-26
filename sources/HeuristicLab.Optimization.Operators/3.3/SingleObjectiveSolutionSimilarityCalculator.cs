﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2012 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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

using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Optimization.Operators {
  /// <summary>
  /// A base class for items that perform similarity calculation between two single objective solutions.
  /// </summary>
  [Item("SimilarityCalculator", "A base class for items that perform similarity calculation between two solutions.")]
  [StorableClass]
  public abstract class SingleObjectiveSolutionSimilarityCalculator : SolutionSimilarityCalculator, ISingleObjectiveSolutionSimilarityCalculator {
    #region Properties
    [Storable]
    public string SolutionVariableName { get; set; }
    [Storable]
    public string QualityVariableName { get; set; }
    #endregion

    [StorableConstructor]
    protected SingleObjectiveSolutionSimilarityCalculator(bool deserializing) : base(deserializing) { }
    protected SingleObjectiveSolutionSimilarityCalculator(SingleObjectiveSolutionSimilarityCalculator original, Cloner cloner)
      : base(original, cloner) {
      this.SolutionVariableName = original.SolutionVariableName;
      this.QualityVariableName = original.QualityVariableName;
    }
    protected SingleObjectiveSolutionSimilarityCalculator() : base() { }

    public override int GetHashCode(IScope scope) {
      return ((DoubleValue)scope.Variables[QualityVariableName].Value).Value.GetHashCode();
    }
  }
}
