
#region License Information
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
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Operators;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.TestFunctions;
using HeuristicLab.Random;

namespace HeuristicLab.Algorithms.GradientDescent {
  /// <summary>
  /// Limited-Memory BFGS optimization algorithm.
  /// </summary>
  [Item("LM-BFGS", "The limited-memory BFGS (Broyden�Fletcher�Goldfarb�Shanno) optimization algorithm.")]
  [Creatable("Algorithms")]
  [StorableClass]
  public sealed class LbfgsAlgorithm : HeuristicOptimizationEngineAlgorithm, IStorableContent {
    public override Type ProblemType {
      get { return typeof(SingleObjectiveTestFunctionProblem); }
    }

    public new SingleObjectiveTestFunctionProblem Problem {
      get { return (SingleObjectiveTestFunctionProblem)base.Problem; }
      set { base.Problem = value; }
    }

    public string Filename { get; set; }

    private const string MaxIterationsParameterName = "MaxIterations";
    private const string ApproximateGradientsParameterName = "ApproximateGradients";
    private const string SeedParameterName = "Seed";
    private const string SetSeedRandomlyParameterName = "SetSeedRandomly";

    #region parameter properties
    public IValueParameter<IntValue> MaxIterationsParameter {
      get { return (IValueParameter<IntValue>)Parameters[MaxIterationsParameterName]; }
    }
    public IValueParameter<IntValue> SeedParameter {
      get { return (IValueParameter<IntValue>)Parameters[SeedParameterName]; }
    }
    public IValueParameter<BoolValue> SetSeedRandomlyParameter {
      get { return (IValueParameter<BoolValue>)Parameters[SetSeedRandomlyParameterName]; }
    }
    #endregion
    #region properties
    public int MaxIterations {
      set { MaxIterationsParameter.Value.Value = value; }
      get { return MaxIterationsParameter.Value.Value; }
    }
    public int Seed { get { return SeedParameter.Value.Value; } set { SeedParameter.Value.Value = value; } }
    public bool SetSeedRandomly { get { return SetSeedRandomlyParameter.Value.Value; } set { SetSeedRandomlyParameter.Value.Value = value; } }
    #endregion

    [StorableConstructor]
    private LbfgsAlgorithm(bool deserializing) : base(deserializing) { }
    private LbfgsAlgorithm(LbfgsAlgorithm original, Cloner cloner)
      : base(original, cloner) {
    }
    public LbfgsAlgorithm()
      : base() {
      this.name = ItemName;
      this.description = ItemDescription;

      Problem = new SingleObjectiveTestFunctionProblem();

      Parameters.Add(new ValueParameter<IntValue>(MaxIterationsParameterName, "The maximal number of iterations for.", new IntValue(20)));
      Parameters.Add(new ValueParameter<IntValue>(SeedParameterName, "The random seed used to initialize the new pseudo random number generator.", new IntValue(0)));
      Parameters.Add(new ValueParameter<BoolValue>(SetSeedRandomlyParameterName, "True if the random seed should be set to a random value, otherwise false.", new BoolValue(true)));
      Parameters.Add(new ValueParameter<BoolValue>(ApproximateGradientsParameterName, "Indicates that gradients should be approximated.", new BoolValue(true)));
      Parameters[ApproximateGradientsParameterName].Hidden = true; // should not be changed

      var randomCreator = new RandomCreator();
      var solutionCreator = new Placeholder();
      var bfgsInitializer = new LbfgsInitializer();
      var makeStep = new LbfgsMakeStep();
      var branch = new ConditionalBranch();
      var evaluator = new Placeholder();
      var updateResults = new LbfgsUpdateResults();
      var analyzer = new LbfgsAnalyzer();
      var finalAnalyzer = new LbfgsAnalyzer();

      OperatorGraph.InitialOperator = randomCreator;

      randomCreator.SeedParameter.ActualName = SeedParameterName;
      randomCreator.SeedParameter.Value = null;
      randomCreator.SetSeedRandomlyParameter.ActualName = SetSeedRandomlyParameterName;
      randomCreator.SetSeedRandomlyParameter.Value = null;
      randomCreator.Successor = solutionCreator;

      solutionCreator.OperatorParameter.ActualName = Problem.SolutionCreatorParameter.Name;
      solutionCreator.Successor = bfgsInitializer;

      bfgsInitializer.IterationsParameter.ActualName = MaxIterationsParameterName;
      bfgsInitializer.PointParameter.ActualName = Problem.SolutionCreator.RealVectorParameter.ActualName;
      bfgsInitializer.ApproximateGradientsParameter.ActualName = ApproximateGradientsParameterName;
      bfgsInitializer.Successor = makeStep;

      makeStep.StateParameter.ActualName = bfgsInitializer.StateParameter.Name;
      makeStep.PointParameter.ActualName = bfgsInitializer.PointParameter.ActualName;
      makeStep.Successor = branch;

      branch.ConditionParameter.ActualName = makeStep.TerminationCriterionParameter.Name;
      branch.FalseBranch = evaluator;
      branch.TrueBranch = finalAnalyzer;

      evaluator.OperatorParameter.ActualName = Problem.EvaluatorParameter.Name;
      evaluator.Successor = updateResults;

      updateResults.StateParameter.ActualName = bfgsInitializer.StateParameter.Name;
      updateResults.QualityParameter.ActualName = Problem.Evaluator.QualityParameter.Name;
      updateResults.ApproximateGradientsParameter.ActualName = ApproximateGradientsParameterName;
      updateResults.Successor = analyzer;

      analyzer.QualityParameter.ActualName = updateResults.QualityParameter.ActualName;
      analyzer.PointParameter.ActualName = makeStep.PointParameter.ActualName;
      analyzer.StateParameter.ActualName = bfgsInitializer.StateParameter.Name;
      analyzer.Successor = makeStep;

      finalAnalyzer.QualityParameter.ActualName = Problem.Evaluator.QualityParameter.Name;
      finalAnalyzer.PointParameter.ActualName = makeStep.PointParameter.ActualName;
      finalAnalyzer.PointsTableParameter.ActualName = analyzer.PointsTableParameter.ActualName;
      finalAnalyzer.QualityGradientsTableParameter.ActualName = analyzer.QualityGradientsTableParameter.ActualName;
      finalAnalyzer.QualitiesTableParameter.ActualName = analyzer.QualitiesTableParameter.ActualName;
    }

    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() { }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new LbfgsAlgorithm(this, cloner);
    }
  }
}