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

using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Operators;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Data;
using HeuristicLab.Random;
using System;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Encodings.RealVectorEncoding {
  /// <summary>
  /// Mutates the endogenous strategy parameters.
  /// </summary>
  [Item("StdDevStrategyVectorManipulator", "Mutates the endogenous strategy parameters.")]
  [StorableClass]
  public class StdDevStrategyVectorManipulator : SingleSuccessorOperator, IStochasticOperator, IRealVectorStdDevStrategyParameterManipulator {
    public override bool CanChangeName {
      get { return false; }
    }
    public ILookupParameter<IRandom> RandomParameter {
      get { return (ILookupParameter<IRandom>)Parameters["Random"]; }
    }
    public ILookupParameter<RealVector> StrategyParameterParameter {
      get { return (ILookupParameter<RealVector>)Parameters["StrategyParameter"]; }
    }
    public IValueLookupParameter<DoubleValue> GeneralLearningRateParameter {
      get { return (IValueLookupParameter<DoubleValue>)Parameters["GeneralLearningRate"]; }
    }
    public IValueLookupParameter<DoubleValue> LearningRateParameter {
      get { return (IValueLookupParameter<DoubleValue>)Parameters["LearningRate"]; }
    }
    public IValueLookupParameter<DoubleMatrix> BoundsParameter {
      get { return (IValueLookupParameter<DoubleMatrix>)Parameters["Bounds"]; }
    }
    /// <summary>
    /// Initializes a new instance of <see cref="StrategyVectorManipulator"/> with four 
    /// parameters (<c>Random</c>, <c>StrategyVector</c>, <c>GeneralLearningRate</c> and
    /// <c>LearningRate</c>).
    /// </summary>
    public StdDevStrategyVectorManipulator()
      : base() {
      Parameters.Add(new LookupParameter<IRandom>("Random", "The random number generator to use."));
      Parameters.Add(new LookupParameter<RealVector>("StrategyParameter", "The strategy parameter to manipulate."));
      Parameters.Add(new ValueLookupParameter<DoubleValue>("GeneralLearningRate", "The general learning rate (tau0)."));
      Parameters.Add(new ValueLookupParameter<DoubleValue>("LearningRate", "The learning rate (tau)."));
      Parameters.Add(new ValueLookupParameter<DoubleMatrix>("Bounds", "A 2 column matrix specifying the lower and upper bound for each dimension. If there are less rows than dimension the bounds vector is cycled.", new DoubleMatrix(new double[,] { { 0, 5 } })));
    }

    /// <summary>
    /// Mutates the endogenous strategy parameters.
    /// </summary>
    /// <param name="random">The random number generator to use.</param>
    /// <param name="vector">The strategy vector to manipulate.</param>
    /// <param name="generalLearningRate">The general learning rate dampens the mutation over all dimensions.</param>
    /// <param name="learningRate">The learning rate dampens the mutation in each dimension.</param>
    public static void Apply(IRandom random, RealVector vector, double generalLearningRate, double learningRate, DoubleMatrix bounds) {
      NormalDistributedRandom N = new NormalDistributedRandom(random, 0.0, 1.0);
      double generalMultiplier = Math.Exp(generalLearningRate * N.NextDouble());
      for (int i = 0; i < vector.Length; i++) {
        vector[i] *= generalMultiplier * Math.Exp(learningRate * N.NextDouble());
        if (bounds != null) {
          double min = bounds[i % bounds.Rows, 0], max = bounds[i % bounds.Rows, 1];
          if (vector[i] < min) vector[i] = min;
          if (vector[i] > max) vector[i] = max;
        }
      }
    }
    /// <summary>
    /// Mutates the endogenous strategy parameters.
    /// </summary>
    /// <remarks>Calls <see cref="OperatorBase.Apply"/> of base class <see cref="OperatorBase"/>.</remarks>
    /// <inheritdoc select="returns"/>
    public override IOperation Apply() {
      RealVector strategyParams = StrategyParameterParameter.ActualValue;
      if (strategyParams != null) { // only apply if there is a strategy vector
        IRandom random = RandomParameter.ActualValue;
        double tau0 = GeneralLearningRateParameter.ActualValue.Value;
        double tau = LearningRateParameter.ActualValue.Value;
        Apply(random, strategyParams, tau0, tau, BoundsParameter.ActualValue);
      }
      return base.Apply();
    }
  }
}
