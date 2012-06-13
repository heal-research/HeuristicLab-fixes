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
using HeuristicLab.Problems.DataAnalysis;

namespace HeuristicLab.Problems.Instances.DataAnalysis {
  public abstract class ArtificialRegressionDataDescriptor : IDataDescriptor {
    public abstract string Name { get; }
    public abstract string Description { get; }

    protected abstract string TargetVariable { get; }
    protected abstract string[] InputVariables { get; }
    protected abstract string[] AllowedInputVariables { get; }
    protected abstract int TrainingPartitionStart { get; }
    protected abstract int TrainingPartitionEnd { get; }
    protected abstract int TestPartitionStart { get; }
    protected abstract int TestPartitionEnd { get; }

    public IRegressionProblemData GenerateRegressionData() {
      Dataset dataset = new Dataset(InputVariables, this.GenerateValues());

      RegressionProblemData regData = new RegressionProblemData(dataset, AllowedInputVariables, TargetVariable);
      regData.Name = this.Name;
      regData.Description = this.Description;
      regData.TrainingPartition.Start = this.TrainingPartitionStart;
      regData.TrainingPartition.End = this.TrainingPartitionEnd;
      regData.TestPartition.Start = this.TestPartitionStart;
      regData.TestPartition.End = this.TestPartitionEnd;
      return regData;
    }

    protected abstract List<List<double>> GenerateValues();
  }
}