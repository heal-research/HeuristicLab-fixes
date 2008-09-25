﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2008 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.Xml;
using HeuristicLab.Core;
using HeuristicLab.Data;

namespace HeuristicLab.SimOpt {
  public class SimOptParameterPacker : OperatorBase {
    public override string Description {
      get { return @"Updates a ConstrainedItemList with the variables in the scope and removes them afterwards."; }
    }

    public SimOptParameterPacker()
      : base() {
      AddVariableInfo(new VariableInfo("Items", "The ConstrainedItemList to be updated", typeof(ConstrainedItemList), VariableKind.In | VariableKind.Out));
    }

    public override IOperation Apply(IScope scope) {
      ConstrainedItemList cil = GetVariableValue<ConstrainedItemList>("Items", scope, false);
      for (int i = 0; i < cil.Count; i++) {
        IVariable var = scope.GetVariable(((Variable)cil[i]).Name);
        if (var == null) throw new InvalidOperationException("ERROR in SimOptParameterPacker: Cannot find variable " + ((Variable)cil[i]).Name + " in scope");
        else {
          ((Variable)cil[i]).Value = (IItem)var.Value.Clone();
          scope.RemoveVariable(var.Name);
        }
      }
      return null;
    }
  }
}
