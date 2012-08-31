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

using System.Collections.Generic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.DataAnalysis.Symbolic.Classification {
  [Item("Penalty Score Evaluator", "Calculates the penalty score of a symbolic classification solution.")]
  [StorableClass]
  public class SymbolicClassificationSingleObjectivePenaltyScoreEvaluator : SymbolicClassificationSingleObjectiveEvaluator {
    public override bool Maximization { get { return false; } }

    [StorableConstructor]
    protected SymbolicClassificationSingleObjectivePenaltyScoreEvaluator(bool deserializing) : base(deserializing) { }
    protected SymbolicClassificationSingleObjectivePenaltyScoreEvaluator(SymbolicClassificationSingleObjectivePenaltyScoreEvaluator original, Cloner cloner) : base(original, cloner) { }
    public SymbolicClassificationSingleObjectivePenaltyScoreEvaluator() : base() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new SymbolicClassificationSingleObjectivePenaltyScoreEvaluator(this, cloner);
    }

    public override IOperation Apply() {
      double quality = Evaluate(ExecutionContext, SymbolicExpressionTreeParameter.ActualValue, ProblemDataParameter.ActualValue, GenerateRowsToEvaluate());
      QualityParameter.ActualValue = new DoubleValue(quality);
      return base.Apply();
    }

    public static double Calculate(IClassificationModel model, IClassificationProblemData problemData, IEnumerable<int> rows) {
      var estimations = model.GetEstimatedClassValues(problemData.Dataset, rows).GetEnumerator();
      if (!estimations.MoveNext()) return double.NaN;

      double penalty = 0.0;
      foreach (var r in rows) {
        var actualClass = problemData.Dataset.GetDoubleValue(problemData.TargetVariable, r);
        penalty += problemData.GetClassificationPenalty(actualClass, estimations.Current);
        estimations.MoveNext();
      }
      return penalty;
    }

    public override double Evaluate(IExecutionContext context, ISymbolicExpressionTree tree, IClassificationProblemData problemData, IEnumerable<int> rows) {
      SymbolicDataAnalysisTreeInterpreterParameter.ExecutionContext = context;
      EstimationLimitsParameter.ExecutionContext = context;

      var model = new SymbolicDiscriminantFunctionClassificationModel(tree, SymbolicDataAnalysisTreeInterpreterParameter.ActualValue, EstimationLimitsParameter.ActualValue.Lower, EstimationLimitsParameter.ActualValue.Upper);
      SymbolicDiscriminantFunctionClassificationModel.SetAccuracyMaximizingThresholds(model, problemData);
      double penalty = Calculate(model, problemData, rows);

      SymbolicDataAnalysisTreeInterpreterParameter.ExecutionContext = null;
      EstimationLimitsParameter.ExecutionContext = null;

      return penalty;
    }
  }
}