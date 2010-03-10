#region License Information
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
using HeuristicLab.Core;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Data {
  [Item("ValueTypeData<T>", "A base class for representing value types.")]
  [StorableClass(StorableClassType.MarkedOnly)]
  public class ValueTypeData<T> : Item where T : struct {
    [Storable]
    private T value;
    public T Value {
      get { return value; }
      set {
        if (!value.Equals(this.value)) {
          this.value = value;
          OnValueChanged();
        }
      }
    }

    public ValueTypeData() {
      this.value = default(T);
    }
    public ValueTypeData(T value) {
      this.value = value;
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      ValueTypeData<T> clone = (ValueTypeData<T>)base.Clone(cloner);
      clone.value = value;
      return clone;
    }

    public override string ToString() {
      return value.ToString();
    }

    public event EventHandler ValueChanged;
    protected virtual void OnValueChanged() {
      if (ValueChanged != null)
        ValueChanged(this, EventArgs.Empty);
      OnToStringChanged();
    }
  }
}
