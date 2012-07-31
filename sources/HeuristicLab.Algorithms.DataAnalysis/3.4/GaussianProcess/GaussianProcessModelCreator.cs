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
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Operators;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.DataAnalysis;

namespace HeuristicLab.Algorithms.DataAnalysis {
  [StorableClass]
  // base class for GaussianProcessModelCreators (specific for classification and regression)
  public abstract class GaussianProcessModelCreator : SingleSuccessorOperator {
    private const string HyperparameterParameterName = "Hyperparameter";
    private const string MeanFunctionParameterName = "MeanFunction";
    private const string CovarianceFunctionParameterName = "CovarianceFunction";
    private const string ModelParameterName = "Model";
    private const string NegativeLogLikelihoodParameterName = "NegativeLogLikelihood";
    private const string HyperparameterGradientsParameterName = "HyperparameterGradients";

    #region Parameter Properties
    // in
    public ILookupParameter<DoubleArray> HyperparameterParameter {
      get { return (ILookupParameter<DoubleArray>)Parameters[HyperparameterParameterName]; }
    }
    public ILookupParameter<IMeanFunction> MeanFunctionParameter {
      get { return (ILookupParameter<IMeanFunction>)Parameters[MeanFunctionParameterName]; }
    }
    public ILookupParameter<ICovarianceFunction> CovarianceFunctionParameter {
      get { return (ILookupParameter<ICovarianceFunction>)Parameters[CovarianceFunctionParameterName]; }
    }
    // out
    public ILookupParameter<IGaussianProcessModel> ModelParameter {
      get { return (ILookupParameter<IGaussianProcessModel>)Parameters[ModelParameterName]; }
    }
    public ILookupParameter<DoubleArray> HyperparameterGradientsParameter {
      get { return (ILookupParameter<DoubleArray>)Parameters[HyperparameterGradientsParameterName]; }
    }
    public ILookupParameter<DoubleValue> NegativeLogLikelihoodParameter {
      get { return (ILookupParameter<DoubleValue>)Parameters[NegativeLogLikelihoodParameterName]; }
    }

    #endregion

    #region Properties
    public DoubleArray Hyperparameter { get { return HyperparameterParameter.ActualValue; } }
    public IMeanFunction MeanFunction { get { return MeanFunctionParameter.ActualValue; } }
    public ICovarianceFunction CovarianceFunction { get { return CovarianceFunctionParameter.ActualValue; } }
    #endregion

    [StorableConstructor]
    protected GaussianProcessModelCreator(bool deserializing) : base(deserializing) { }
    protected GaussianProcessModelCreator(GaussianProcessModelCreator original, Cloner cloner) : base(original, cloner) { }
    protected GaussianProcessModelCreator()
      : base() {
      // in
      Parameters.Add(new LookupParameter<DoubleArray>(HyperparameterParameterName, "The hyperparameters for the Gaussian process model."));
      Parameters.Add(new LookupParameter<IMeanFunction>(MeanFunctionParameterName, "The mean function for the Gaussian process model."));
      Parameters.Add(new LookupParameter<ICovarianceFunction>(CovarianceFunctionParameterName, "The covariance function for the Gaussian process model."));
      // out
      Parameters.Add(new LookupParameter<IGaussianProcessModel>(ModelParameterName, "The resulting Gaussian process model"));
      Parameters.Add(new LookupParameter<DoubleArray>(HyperparameterGradientsParameterName, "The gradients of the hyperparameters for the produced Gaussian process model (necessary for hyperparameter optimization)"));
      Parameters.Add(new LookupParameter<DoubleValue>(NegativeLogLikelihoodParameterName, "The negative log-likelihood of the produced Gaussian process model given the data."));
    }
  }
}