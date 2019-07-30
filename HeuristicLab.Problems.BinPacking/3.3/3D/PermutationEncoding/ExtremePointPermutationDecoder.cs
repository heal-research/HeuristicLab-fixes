﻿#region License Information
/* HeuristicLab
 * Copyright (C) Joseph Helm and Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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

using HeuristicLab.Core;
using HEAL.Attic;
using HeuristicLab.Common;
using System.Collections.Generic;
using HeuristicLab.Encodings.PermutationEncoding;


namespace HeuristicLab.Problems.BinPacking3D {
  [Item("Extreme-point Permutation Decoder (3d)", "Decodes the permutation and creates a packing solution candidate")]
  [StorableType("1232374B-6C9C-46A2-B7C2-97F484597D44")]
  public class ExtremePointPermutationDecoder : ExtremePointPermutationDecoderBase {

    [StorableConstructor]
    protected ExtremePointPermutationDecoder(StorableConstructorFlag _) : base(_) { }
    protected ExtremePointPermutationDecoder(ExtremePointPermutationDecoder original, Cloner cloner)
      : base(original, cloner) {
    }
    public ExtremePointPermutationDecoder() : base() { }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new ExtremePointPermutationDecoder(this, cloner);
    }

    public override Solution Decode(Permutation permutation, PackingShape binShape, IList<PackingItem> items, bool useStackingConstraints) {
      return Apply(permutation, binShape, items, useStackingConstraints);
    }

    public static Solution Apply(Permutation permutation, PackingShape binShape, IList<PackingItem> items, bool useStackingConstraints) {
      Solution result = new Solution(binShape, useExtremePoints: true, stackingConstraints: useStackingConstraints);
      IList<int> remainingIDs = new List<int>(permutation);
      while (remainingIDs.Count > 0) {
        var bp = new BinPacking3D(binShape);
        bp.ExtremePointBasedPacking(ref remainingIDs, items, stackingConstraints: useStackingConstraints);
        result.Bins.Add(bp);
      }
      result.UpdateBinPackings();
      return result;
    }
  }
}
