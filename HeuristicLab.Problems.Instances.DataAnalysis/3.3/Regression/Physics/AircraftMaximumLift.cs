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
  class AircraftMaximumLift : ArtificialRegressionDataDescriptor {
    public override string Name { get { return "Aircraft Maximum Lift Coefficient"; } }

    public override string Description {
      get {
        return "A full description of this problem instance is given in the paper: A multilevel block building algorithm for fast modeling generalized separable systems. " + Environment.NewLine +
               "Authors: Chen Chen, Changtong Luo, Zonglin Jiang" + Environment.NewLine +
               "Function: f(X) = x1 - 0.25 x4 x5 x6 (4 + 0.1 x2/x3 - x2²/x3²) + x13 x14/x15 x18 x7 - x13 x14/x15 x8 + x13 x14/x15 x9 + x16 x17/x15 x18 x10 - x16 x17/x15 x11 + x16 x17/x15 x12" + Environment.NewLine +
               "with x1 in [0.4, 0.8], " +
               "x2 in [3, 4], " +
               "x3 in [20, 30], " +
               "x4, x13, x16 in [2, 5]," +
               "x14, x17 in [1, 1.5], " +
               "x15 in [5, 7]," +
               "x18 in [10, 20]," +
               "x8, x11 in [1, 1.5]," +
               "x9, x12 in [1, 2]," +
               "x7, x10 in [0.5, 1.5]";
      }
    }

    protected override string TargetVariable { get { return "f(X)"; } }
    protected override string[] VariableNames { get { return new string[] { "x1", "x2", "x3", "x4", "x5", "x6", "x7", "x8", "x9", "x10", "x11", "x12", "x13", "x14", "x15", "x16", "x17", "x18", "f(X)" }; } }
    protected override string[] AllowedInputVariables { get { return VariableNames.Except(new string[] { TargetVariable }).ToArray(); } }
    protected override int TrainingPartitionStart { get { return 0; } }
    protected override int TrainingPartitionEnd { get { return 100; } }
    protected override int TestPartitionStart { get { return 100; } }
    protected override int TestPartitionEnd { get { return 200; } }

    public int Seed { get; private set; }

    public AircraftMaximumLift() : this((int)System.DateTime.Now.Ticks) { }

    public AircraftMaximumLift(int seed) {
      Seed = seed;
    }

    protected override List<List<double>> GenerateValues() {
      var rand = new MersenneTwister((uint)Seed);

      List<List<double>> data = new List<List<double>>();
      var x1 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 0.4, 0.8).ToList();

      var x2 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 3.0, 4.0).ToList();

      var x3 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 20.0, 30.0).ToList();

      var x4 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 2.0, 5.0).ToList();
      var x13 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 2.0, 5.0).ToList();
      var x16 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 2.0, 5.0).ToList();


      var x5 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 1.0, 1.5).ToList(); // TODO: range for X5 is not specified in the paper
      var x6 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 5.0, 7.0).ToList(); // TODO: range for X6 is not specified in the paper

      var x7 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 0.5, 1.5).ToList();
      var x10 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 0.5, 1.5).ToList();

      var x8 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 1.0, 1.5).ToList();
      var x11 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 1.0, 1.5).ToList();

      var x9 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 1.0, 2.0).ToList();
      var x12 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 1.0, 2.0).ToList();

      var x14 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 1.0, 1.5).ToList();
      var x17 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 1.0, 1.5).ToList();

      var x15 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 5.0, 7.0).ToList();

      var x18 = ValueGenerator.GenerateUniformDistributedValues(rand.Next(), TestPartitionEnd, 10.0, 20.0).ToList();

      List<double> fx = new List<double>();
      data.Add(x1);
      data.Add(x2);
      data.Add(x3);
      data.Add(x4);
      data.Add(x5);
      data.Add(x6);
      data.Add(x7);
      data.Add(x8);
      data.Add(x9);
      data.Add(x10);
      data.Add(x11);
      data.Add(x12);
      data.Add(x13);
      data.Add(x14);
      data.Add(x15);
      data.Add(x16);
      data.Add(x17);
      data.Add(x18);
      data.Add(fx);

      for (int i = 0; i < x1.Count; i++) {
        double fxi = x1[i];
        fxi = fxi - 0.25 * x4[i] * x5[i] * x6[i] * (4 + 0.1 * (x2[i] / x3[i]) - (x2[i] / x3[i]) * (x2[i] / x3[i]));
        fxi = fxi + x13[i] * (x14[i] / x15[i]) * x18[i] * x7[i];
        fxi = fxi - x13[i] * (x14[i] / x15[i]) * x8[i];
        fxi = fxi + x13[i] * (x14[i] / x15[i]) * x9[i];
        fxi = fxi + x16[i] * (x17[i] / x15[i]) * x18[i] * x10[i];
        fxi = fxi - x16[i] * (x17[i] / x15[i]) * x11[i];
        fxi = fxi + x16[i] * (x17[i] / x15[i]) * x12[i];

        fx.Add(fxi);
      }

      return data;
    }
  }
}