﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2011 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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

namespace HeuristicLab.Problems.DataAnalysis.Symbolic.Regression {
  [Item("Mean squared error & Tree size Evaluator", "Calculates the mean squared error and the tree size of a symbolic regression solution.")]
  [StorableClass]
  public class SymbolicRegressionMultiObjectiveMeanSquaredErrorSolutionSizeEvaluator : SymbolicRegressionMultiObjectiveEvaluator {
    [StorableConstructor]
    protected SymbolicRegressionMultiObjectiveMeanSquaredErrorSolutionSizeEvaluator(bool deserializing) : base(deserializing) { }
    protected SymbolicRegressionMultiObjectiveMeanSquaredErrorSolutionSizeEvaluator(SymbolicRegressionMultiObjectiveMeanSquaredErrorSolutionSizeEvaluator original, Cloner cloner)
      : base(original, cloner) {
    }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new SymbolicRegressionMultiObjectiveMeanSquaredErrorSolutionSizeEvaluator(this, cloner);
    }

    public SymbolicRegressionMultiObjectiveMeanSquaredErrorSolutionSizeEvaluator() : base() { }

    public override IEnumerable<bool> Maximization { get { return new bool[2] { false, false }; } }

    public override Core.IOperation Apply() {
      IEnumerable<int> rows = GenerateRowsToEvaluate();
      var solution = SymbolicExpressionTreeParameter.ActualValue;
      double[] qualities = Calculate(SymbolicDataAnalysisTreeInterpreterParameter.ActualValue, solution, EstimationLimitsParameter.ActualValue.Lower, EstimationLimitsParameter.ActualValue.Upper, ProblemDataParameter.ActualValue, rows);
      QualitiesParameter.ActualValue = new DoubleArray(qualities);
      return base.Apply();
    }

    public static double[] Calculate(ISymbolicDataAnalysisExpressionTreeInterpreter interpreter, ISymbolicExpressionTree solution, double lowerEstimationLimit, double upperEstimationLimit, IRegressionProblemData problemData, IEnumerable<int> rows) {
      IEnumerable<double> estimatedValues = interpreter.GetSymbolicExpressionTreeValues(solution, problemData.Dataset, rows);
      IEnumerable<double> originalValues = problemData.Dataset.GetEnumeratedVariableValues(problemData.TargetVariable, rows);
      IEnumerable<double> boundedEstimationValues = estimatedValues.LimitToRange(lowerEstimationLimit, upperEstimationLimit);
      OnlineEvaluatorError errorState;
      double mse = OnlineMeanSquaredErrorEvaluator.Calculate(originalValues, boundedEstimationValues, out errorState);
      if (errorState != OnlineEvaluatorError.None) mse = double.NaN;
      return new double[2] { mse, solution.Length };
    }

    public override double[] Evaluate(IExecutionContext context, ISymbolicExpressionTree tree, IRegressionProblemData problemData, IEnumerable<int> rows) {
      SymbolicDataAnalysisTreeInterpreterParameter.ExecutionContext = context;
      EstimationLimitsParameter.ExecutionContext = context;

      double[] quality = Calculate(SymbolicDataAnalysisTreeInterpreterParameter.ActualValue, tree, EstimationLimitsParameter.ActualValue.Lower, EstimationLimitsParameter.ActualValue.Upper, problemData, rows);

      SymbolicDataAnalysisTreeInterpreterParameter.ExecutionContext = null;
      EstimationLimitsParameter.ExecutionContext = null;

      return quality;
    }
  }
}
