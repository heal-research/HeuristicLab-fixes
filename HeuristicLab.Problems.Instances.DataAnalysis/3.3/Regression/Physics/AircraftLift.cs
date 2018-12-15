﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2018 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using HeuristicLab.Random;

namespace HeuristicLab.Problems.Instances.DataAnalysis {
  class AircraftLift : ArtificialRegressionDataDescriptor {
    public override string Name { get { return "Aircraft Lift Coefficient C_L = x1*(x2 - 2) + x3*x4*x5/x6"; } }

    public override string Description {
      get {
        return "A full description of this problem instance is given in the paper: A multilevel block building algorithm for fast modeling generalized separable systems. " + Environment.NewLine +
               "Authors: Chen Chen, Changtong Luo, Zonglin Jiang" + Environment.NewLine +
               "Function: f(X) = x1*(x2 - 2) + x3*x4*x5/x6" + Environment.NewLine +
               "with x1 in [0.4, 0.8], x2 in [5, 10], x3 in [0.4, 0.8], x4 in [5, 10], x5 in [1, 1.5], x6 in [5, 7]";
      }
    }

    protected override string TargetVariable { get { return "f(X)"; } }
    protected override string[] VariableNames { get { return new string[] { "x1", "x2", "x3", "x4", "x5", "x6", "f(X)" }; } }
    protected override string[] AllowedInputVariables { get { return new string[] { "x1", "x2", "x3", "x4", "x5", "x6" }; } }
    protected override int TrainingPartitionStart { get { return 0; } }
    protected override int TrainingPartitionEnd { get { return 100; } }
    protected override int TestPartitionStart { get { return 100; } }
    protected override int TestPartitionEnd { get { return 200; } }

    public int Seed { get; private set; }

    public AircraftLift() : this((int)System.DateTime.Now.Ticks) { }

    public AircraftLift(int seed) {
      Seed = seed;
    }

    protected override List<List<double>> GenerateValues() {
      var rand = new MersenneTwister((uint)Seed);

      List<List<double>> data = new List<List<double>>();
      var x1 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 0.4, 0.8).ToList();
      var x2 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 5.0, 10.0).ToList();
      var x3 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 0.4, 0.8).ToList();
      var x4 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 5.0, 10.0).ToList();
      var x5 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 1.0, 1.5).ToList();
      var x6 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 5.0, 7.0).ToList();

      List<double> fx = new List<double>();
      data.Add(x1);
      data.Add(x2);
      data.Add(x3);
      data.Add(x4);
      data.Add(x5);
      data.Add(x6);
      data.Add(fx);

      for (int i = 0; i < x1.Count; i++) {
        double fxi = x1[i] * (x2[i] - 2.0) + x3[i] * x4[i] * x5[i] / x6[i];
        fx.Add(fxi);
      }

      return data;
    }
  }
}
