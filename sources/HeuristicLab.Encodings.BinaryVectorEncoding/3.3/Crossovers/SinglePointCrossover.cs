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
using HeuristicLab.Data;
using HeuristicLab.Parameters;

namespace HeuristicLab.Encodings.BinaryVectorEncoding {
  /// <summary>
  /// Single point crossover for binary vectors.
  /// </summary>
  /// <remarks>
  /// It is implemented based on the NPointCrossover
  /// </remarks>
  [Item("SinglePointCrossover", "Single point crossover for binary vectors. It is implemented based on the NPointCrossover.")]
  [StorableClass]
  public class SinglePointCrossover: BinaryVectorCrossover {
    /// <summary>
    /// Performs a single point crossover at a randomly chosen position of two 
    /// given parent binary vectors.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if there are not exactly two parents.</exception>
    /// <param name="random">A random number generator.</param>
    /// <param name="parents">An array containing the two binary vectors that should be crossed.</param>
    /// <returns>The newly created binary vector, resulting from the single point crossover.</returns>
    protected override BinaryVector Cross(IRandom random, ItemArray<BinaryVector> parents) {
      if (parents.Length != 2) throw new ArgumentException("ERROR in NPointCrossover: The number of parents is not equal to 2");

      return NPointCrossover.Apply(random, parents[0], parents[1], new IntValue(1));
    }
  }
}
