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
using System.Windows.Forms;
using HeuristicLab.Core.Views;
using HeuristicLab.MainForm;

namespace HeuristicLab.Data.Views {
  [View("BoolValue View")]
  [Content(typeof(BoolValue), true)]
  public partial class BoolValueView : ItemView {
    public new BoolValue Content {
      get { return (BoolValue)base.Content; }
      set { base.Content = value; }
    }

    public override bool ReadOnly {
      get {
        if ((Content != null) && Content.ReadOnly) return true;
        return base.ReadOnly;
      }
      set { base.ReadOnly = value; }
    }

    public BoolValueView() {
      InitializeComponent();
      Caption = "BoolValue View";
    }
    public BoolValueView(BoolValue content)
      : this() {
      Content = content;
    }

    protected override void DeregisterContentEvents() {
      Content.ValueChanged -= new EventHandler(Content_ValueChanged);
      base.DeregisterContentEvents();
    }

    protected override void RegisterContentEvents() {
      base.RegisterContentEvents();
      Content.ValueChanged += new EventHandler(Content_ValueChanged);
    }

    protected override void OnContentChanged() {
      base.OnContentChanged();
      if (Content == null) {
        Caption = "BoolValue View";
        valueCheckBox.Checked = false;
      } else {
        Caption = Content.ToString() + " (" + Content.GetType().Name + ")";
        valueCheckBox.Checked = Content.Value;
      }
      SetEnabledStateOfControls();
    }

    protected override void OnReadOnlyChanged() {
      base.OnReadOnlyChanged();
      SetEnabledStateOfControls();
    }
    private void SetEnabledStateOfControls() {
      if (Content == null) valueCheckBox.Enabled = false;
      else valueCheckBox.Enabled = !ReadOnly;
    }

    private void Content_ValueChanged(object sender, EventArgs e) {
      if (InvokeRequired)
        Invoke(new EventHandler(Content_ValueChanged), sender, e);
      else
        valueCheckBox.Checked = Content.Value;
    }

    private void valueCheckBox_CheckedChanged(object sender, EventArgs e) {
      if (!Content.ReadOnly) Content.Value = valueCheckBox.Checked;
    }
  }
}
