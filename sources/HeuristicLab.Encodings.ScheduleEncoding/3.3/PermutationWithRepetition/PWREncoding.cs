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

using System.Text;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Encodings.IntegerVectorEncoding;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Encodings.ScheduleEncoding.PermutationWithRepetition {
  [Item("PermutationWithRepetitionEncoding", "Represents a encoding for a standard JobShop Scheduling Problem.")]
  [StorableClass]
  public class PWREncoding : Item, IScheduleEncoding {
    [Storable]
    public IntegerVector PermutationWithRepetition { get; set; }


    [StorableConstructor]
    protected PWREncoding(bool deserializing) : base(deserializing) { }
    protected PWREncoding(PWREncoding original, Cloner cloner)
      : base(original, cloner) {
      this.PermutationWithRepetition = cloner.Clone(original.PermutationWithRepetition);
    }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new PWREncoding(this, cloner);
    }
    public PWREncoding()
      : base() {
      PermutationWithRepetition = new IntegerVector();
    }

    public PWREncoding(int nrOfJobs, int nrOfResources, IRandom random)
      : base() {
      PermutationWithRepetition = new IntegerVector(nrOfJobs * nrOfResources);
      int[] lookUpTable = new int[nrOfJobs];

      for (int i = 0; i < PermutationWithRepetition.Length; i++) {
        int newValue = random.Next(nrOfJobs);
        while (lookUpTable[newValue] >= nrOfResources)
          newValue = random.Next(nrOfJobs);

        PermutationWithRepetition[i] = newValue;

        lookUpTable[newValue]++;
      }
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder();
      sb.Append("[ ");
      foreach (int i in PermutationWithRepetition) {
        sb.Append(i + " ");
      }
      sb.Append("]");

      return sb.ToString();
    }

    public override bool Equals(object obj) {
      if (obj.GetType() == typeof(PWREncoding))
        return AreEqual(this, obj as PWREncoding);
      else
        return base.Equals(obj);
    }
    public override int GetHashCode() {
      if (PermutationWithRepetition.Length == 1)
        return PermutationWithRepetition[0].GetHashCode();
      if (PermutationWithRepetition.Length == 2)
        return PermutationWithRepetition[0].GetHashCode() ^ PermutationWithRepetition[1].GetHashCode();
      return 0;
    }
    private bool AreEqual(PWREncoding pWREncoding1, PWREncoding pWREncoding2) {
      if (pWREncoding1.PermutationWithRepetition.Length != pWREncoding2.PermutationWithRepetition.Length)
        return false;
      for (int i = 0; i < pWREncoding1.PermutationWithRepetition.Length; i++) {
        if (pWREncoding1.PermutationWithRepetition[i] != pWREncoding2.PermutationWithRepetition[i])
          return false;
      }
      return true;
    }


  }
}