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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeuristicLab.Core;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Operators;
using HeuristicLab.Optimization;
using HeuristicLab.Problems.VehicleRouting.Variants;
using HeuristicLab.Problems.VehicleRouting.Interfaces;
using HeuristicLab.Parameters;
using HeuristicLab.Data;
using HeuristicLab.Common;

namespace HeuristicLab.Problems.VehicleRouting {
  /// <summary>
  /// An operator for adaptive constraint relaxation.
  /// </summary>
  [Item("TimeWindowRelaxationVRPAnalyzer", "An operator for adaptively relaxing the time window constraints.")]
  [StorableClass]
  public class TimeWindowRelaxationVRPAnalyzer: SingleSuccessorOperator, IAnalyzer, ITimeWindowedOperator {
    public ILookupParameter<IVRPProblemInstance> ProblemInstanceParameter {
      get { return (ILookupParameter<IVRPProblemInstance>)Parameters["ProblemInstance"]; }
    }
    public ScopeTreeLookupParameter<IVRPEncoding> VRPToursParameter {
      get { return (ScopeTreeLookupParameter<IVRPEncoding>)Parameters["VRPTours"]; }
    }
    public ScopeTreeLookupParameter<DoubleValue> QualityParameter {
      get { return (ScopeTreeLookupParameter<DoubleValue>)Parameters["Quality"]; }
    }

    public ScopeTreeLookupParameter<DoubleValue> TardinessParameter {
      get { return (ScopeTreeLookupParameter<DoubleValue>)Parameters["Tardiness"]; }
    }

    public IValueParameter<DoubleValue> SigmaParameter {
      get { return (IValueParameter<DoubleValue>)Parameters["Sigma"]; }
    }
    public IValueParameter<DoubleValue> PhiParameter {
      get { return (IValueParameter<DoubleValue>)Parameters["Phi"]; }
    }
    public IValueParameter<DoubleValue> MinPenaltyFactorParameter {
      get { return (IValueParameter<DoubleValue>)Parameters["MinPenaltyFactor"]; }
    }

    public ValueLookupParameter<ResultCollection> ResultsParameter {
      get { return (ValueLookupParameter<ResultCollection>)Parameters["Results"]; }
    }

    public bool EnabledByDefault {
      get { return false; }
    }

    [StorableConstructor]
    protected TimeWindowRelaxationVRPAnalyzer(bool deserializing) : base(deserializing) { }

    public TimeWindowRelaxationVRPAnalyzer()
      : base() {
      Parameters.Add(new LookupParameter<IVRPProblemInstance>("ProblemInstance", "The problem instance."));
      Parameters.Add(new ScopeTreeLookupParameter<IVRPEncoding>("VRPTours", "The VRP tours which should be evaluated."));
      Parameters.Add(new ScopeTreeLookupParameter<DoubleValue>("Quality", "The qualities of the VRP solutions which should be analyzed."));

      Parameters.Add(new ScopeTreeLookupParameter<DoubleValue>("Tardiness", "The tardiness of the VRP solutions which should be analyzed."));

      Parameters.Add(new ValueParameter<DoubleValue>("Sigma", "The sigma applied to the penalty factor.", new DoubleValue(0.04)));
      Parameters.Add(new ValueParameter<DoubleValue>("Phi", "The phi applied to the penalty factor.", new DoubleValue(0.01)));
      Parameters.Add(new ValueParameter<DoubleValue>("MinPenaltyFactor", "The minimum penalty factor.", new DoubleValue(0.01)));

      Parameters.Add(new ValueLookupParameter<ResultCollection>("Results", "The result collection where the best VRP solution should be stored."));
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new TimeWindowRelaxationVRPAnalyzer(this, cloner);
    }

    protected TimeWindowRelaxationVRPAnalyzer(TimeWindowRelaxationVRPAnalyzer original, Cloner cloner)
      : base(original, cloner) {
    }

    public override IOperation Apply() {
      ITimeWindowedProblemInstance vrptw = ProblemInstanceParameter.ActualValue as ITimeWindowedProblemInstance;
      ResultCollection results = ResultsParameter.ActualValue;
      
      ItemArray<DoubleValue> qualities = QualityParameter.ActualValue;
      ItemArray<DoubleValue> tardiness = TardinessParameter.ActualValue;
      
      double sigma = SigmaParameter.Value.Value;
      double phi = PhiParameter.Value.Value;
      double minPenalty = MinPenaltyFactorParameter.Value.Value;

      for (int j = 0; j < qualities.Length; j++) {
        qualities[j].Value -= tardiness[j].Value * vrptw.TardinessPenalty.Value;
      }

      int validCount = 0;
      for (int j = 0; j < qualities.Length; j++) {
        if (tardiness[j].Value == 0)
          validCount++;
      }

      double factor = 1.0 - ((double)validCount / (double)qualities.Length);

      double min = vrptw.TardinessPenalty.Value / (1 + sigma);
      double max = vrptw.TardinessPenalty.Value * (1 + phi);

      vrptw.TardinessPenalty = new DoubleValue(min + (max - min) * factor);
      if (vrptw.TardinessPenalty.Value < minPenalty)
        vrptw.TardinessPenalty.Value = minPenalty;

      for (int j = 0; j < qualities.Length; j++) {
        qualities[j].Value += tardiness[j].Value * vrptw.TardinessPenalty.Value;
      }

      if (!results.ContainsKey("Current Tardiness Penalty")) {
        results.Add(new Result("Current Tardiness Penalty", new DoubleValue(vrptw.TardinessPenalty.Value)));
      } else {
        (results["Current Tardiness Penalty"].Value as DoubleValue).Value = vrptw.TardinessPenalty.Value;
      }

      return base.Apply();
    }
  }
}