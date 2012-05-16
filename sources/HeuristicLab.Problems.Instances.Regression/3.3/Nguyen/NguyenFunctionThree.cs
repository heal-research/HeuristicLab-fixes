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

namespace HeuristicLab.Problems.Instances.Regression {
  public class NguyenFunctionThree : ArtificialRegressionDataDescriptor {

    public override string Name { get { return "Nguyen F3 = x^5 + x^4 + x^3 + x^2 + x"; } }
    public override string Description {
      get {
        return "Paper: Semantically-based Crossover in Genetic Programming: Application to Real-valued Symbolic Regression" + Environment.NewLine
        + "Authors: Nguyen Quang Uy · Nguyen Xuan Hoai · Michael O’Neill · R.I. McKay · Edgar Galvan-Lopez" + Environment.NewLine
        + "Function: F3 = x^5 + x^4 + x^3 + x^2 + x" + Environment.NewLine
        + "Fitcases: 20 random points ⊆ [-1, 1]" + Environment.NewLine
        + "Non-terminals: +, -, *, /, sin, cos, exp, log (protected version)" + Environment.NewLine
        + "Terminals: X, 1 for single variable problems, and X, Y for bivariable problems";
      }
    }
    protected override string TargetVariable { get { return "Y"; } }
    protected override string[] InputVariables { get { return new string[] { "X", "Y" }; } }
    protected override string[] AllowedInputVariables { get { return new string[] { "X" }; } }
    protected override int TrainingPartitionStart { get { return 0; } }
    protected override int TrainingPartitionEnd { get { return 20; } }
    protected override int TestPartitionStart { get { return 250; } }
    protected override int TestPartitionEnd { get { return 350; } }

    protected override List<List<double>> GenerateValues() {
      List<List<double>> data = new List<List<double>>();
      data.Add(ValueGenerator.GenerateUniformDistributedValues(500, -1, 1).ToList());

      double x;
      List<double> results = new List<double>();
      for (int i = 0; i < data[0].Count; i++) {
        x = data[0][i];
        results.Add(Math.Pow(x, 5) + Math.Pow(x, 4) + Math.Pow(x, 3) + Math.Pow(x, 2) + x);
      }
      data.Add(results);

      return data;
    }
  }
}