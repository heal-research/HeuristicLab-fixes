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

using System;
using System.Collections.Generic;
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Algorithms.DataAnalysis {
  [StorableClass]
  [Item(Name = "CovarianceMaternIso",
    Description = "Matern covariance function for Gaussian processes.")]
  public sealed class CovarianceMaternIso : ParameterizedNamedItem, ICovarianceFunction {
    [Storable]
    private double inverseLength;
    [Storable]
    private readonly HyperParameter<DoubleValue> inverseLengthParameter;
    public IValueParameter<DoubleValue> InverseLengthParameter {
      get { return inverseLengthParameter; }
    }

    [Storable]
    private double sf2;
    [Storable]
    private readonly HyperParameter<DoubleValue> scaleParameter;
    public IValueParameter<DoubleValue> ScaleParameter {
      get { return scaleParameter; }
    }

    [Storable]
    private int d;
    [Storable]
    private readonly ConstrainedValueParameter<IntValue> dParameter;
    public IConstrainedValueParameter<IntValue> DParameter {
      get { return dParameter; }
    }


    [StorableConstructor]
    private CovarianceMaternIso(bool deserializing)
      : base(deserializing) {
    }

    private CovarianceMaternIso(CovarianceMaternIso original, Cloner cloner)
      : base(original, cloner) {
      this.scaleParameter = cloner.Clone(original.scaleParameter);
      this.sf2 = original.sf2;
      this.inverseLengthParameter = cloner.Clone(original.inverseLengthParameter);
      this.inverseLength = original.inverseLength;
      this.dParameter = cloner.Clone(original.dParameter);
      this.d = original.d;
      RegisterEvents();
    }

    public CovarianceMaternIso()
      : base() {
      Name = ItemName;
      Description = ItemDescription;

      inverseLengthParameter = new HyperParameter<DoubleValue>("InverseLength", "The inverse length parameter of the isometric Matern covariance function.");
      scaleParameter = new HyperParameter<DoubleValue>("Scale", "The scale parameter of the isometric Matern covariance function.");
      var validDValues = new ItemSet<IntValue>();
      validDValues.Add((IntValue)new IntValue(1).AsReadOnly());
      validDValues.Add((IntValue)new IntValue(3).AsReadOnly());
      validDValues.Add((IntValue)new IntValue(5).AsReadOnly());
      dParameter = new ConstrainedValueParameter<IntValue>("D", "The d parameter (allowed values: 1, 3, or 5) of the isometric Matern covariance function.", validDValues, validDValues.First());
      d = dParameter.Value.Value;

      Parameters.Add(inverseLengthParameter);
      Parameters.Add(scaleParameter);
      Parameters.Add(dParameter);

      RegisterEvents();
    }

    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      RegisterEvents();
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new CovarianceMaternIso(this, cloner);
    }

    // caching
    private void RegisterEvents() {
      Util.AttachValueChangeHandler<DoubleValue, double>(inverseLengthParameter, () => { inverseLength = inverseLengthParameter.Value.Value; });
      Util.AttachValueChangeHandler<DoubleValue, double>(scaleParameter, () => { sf2 = scaleParameter.Value.Value; });
      Util.AttachValueChangeHandler<IntValue, int>(dParameter, () => { d = dParameter.Value.Value; });
    }

    public int GetNumberOfParameters(int numberOfVariables) {
      return
        (inverseLengthParameter.Fixed ? 0 : 1) +
        (scaleParameter.Fixed ? 0 : 1);
    }

    public void SetParameter(double[] hyp) {
      int i = 0;
      if (!inverseLengthParameter.Fixed) {
        inverseLengthParameter.SetValue(new DoubleValue(1.0 / Math.Exp(hyp[i])));
        i++;
      }
      if (!scaleParameter.Fixed) {
        scaleParameter.SetValue(new DoubleValue(Math.Exp(2 * hyp[i])));
        i++;
      }
      if (hyp.Length != i) throw new ArgumentException("The length of the parameter vector does not match the number of free parameters for CovarianceMaternIso", "hyp");
    }


    private double m(double t) {
      double f;
      switch (d) {
        case 1: { f = 1; break; }
        case 3: { f = 1 + t; break; }
        case 5: { f = 1 + t * (1 + t / 3.0); break; }
        default: throw new InvalidOperationException();
      }
      return f * Math.Exp(-t);
    }

    private double dm(double t) {
      double df;
      switch (d) {
        case 1: { df = 1; break; }
        case 3: { df = t; break; }
        case 5: { df = t * (1 + t) / 3.0; break; }
        default: throw new InvalidOperationException();
      }
      return df * t * Math.Exp(-t);
    }

    public double GetCovariance(double[,] x, int i, int j, IEnumerable<int> columnIndices) {
      double dist = i == j
                   ? 0.0
                   : Math.Sqrt(Util.SqrDist(x, i, j, Math.Sqrt(d) * inverseLength, columnIndices));
      return sf2 * m(dist);
    }

    public IEnumerable<double> GetGradient(double[,] x, int i, int j, IEnumerable<int> columnIndices) {
      double dist = i == j
                   ? 0.0
                   : Math.Sqrt(Util.SqrDist(x, i, j, Math.Sqrt(d) * inverseLength, columnIndices));

      yield return sf2 * dm(dist);
      yield return 2 * sf2 * m(dist);
    }

    public double GetCrossCovariance(double[,] x, double[,] xt, int i, int j, IEnumerable<int> columnIndices) {
      double dist = Math.Sqrt(Util.SqrDist(x, i, xt, j, Math.Sqrt(d) * inverseLength, columnIndices));
      return sf2 * m(dist);
    }
  }
}