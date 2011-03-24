#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2011 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.Windows.Forms;
using HeuristicLab.Common.Resources;
using HeuristicLab.MainForm;
using HeuristicLab.Optimization.Views;

namespace HeuristicLab.Problems.DataAnalysis.Views {
  [View("Data analysis solution view")]
  [Content(typeof(DataAnalysisSolution), true)]
  public partial class DataAnalysisSolutionView : ResultCollectionView {
    public DataAnalysisSolutionView() {
      InitializeComponent();
    }

    public new DataAnalysisSolution Content {
      get { return (DataAnalysisSolution)base.Content; }
      set { base.Content = value; }
    }

    protected override void OnContentChanged() {
      string selectedName = null;
      if ((itemsListView.SelectedItems.Count == 1) && (itemsListView.SelectedItems[0].Tag != null && itemsListView.SelectedItems[0].Tag is Type))
        selectedName = itemsListView.SelectedItems[0].Text;

      //cache old viewTypes;
      var viewTypes = new List<Type>();
      foreach (ListViewItem item in ItemsListView.Items) {
        var viewType = item.Tag as Type;
        if (viewType != null) viewTypes.Add(viewType);
      }

      base.OnContentChanged();

      //readd viewTypes
      foreach (Type viewType in viewTypes)
        AddViewListViewItem(viewType);

      //recover selection
      if (selectedName != null) {
        foreach (ListViewItem item in itemsListView.Items) {
          if (item.Tag != null && item.Tag is Type && item.Text == selectedName)
            item.Selected = true;
        }
      }
    }

    protected override void itemsListView_DoubleClick(object sender, EventArgs e) {
      if (itemsListView.SelectedItems.Count == 1 && itemsListView.SelectedItems[0].Tag is Type) {
        Type viewType = (Type)itemsListView.SelectedItems[0].Tag;
        MainFormManager.MainForm.ShowContent(Content, viewType);
      } else
        base.itemsListView_DoubleClick(sender, e);
    }

    protected override void itemsListView_SelectedIndexChanged(object sender, EventArgs e) {
      if (itemsListView.SelectedItems.Count == 1 && itemsListView.SelectedItems[0].Tag is Type) {
        detailsGroupBox.Enabled = true;
        Type viewType = (Type)itemsListView.SelectedItems[0].Tag;
        viewHost.ViewType = viewType;
        viewHost.Content = Content;
      } else
        base.itemsListView_SelectedIndexChanged(sender, e);
    }

    protected void AddViewListViewItem(Type viewType) {
      if (!typeof(IDataAnalysisSolutionEvaluationView).IsAssignableFrom(viewType))
        throw new ArgumentException("Given type " + viewType + " is not a IDataAnalysisSolutionEvaluationView.");

      ListViewItem listViewItem = new ListViewItem();
      listViewItem.Text = ViewAttribute.GetViewName(viewType);
      itemsListView.SmallImageList.Images.Add(VSImageLibrary.Graph);
      listViewItem.ImageIndex = itemsListView.SmallImageList.Images.Count - 1;
      listViewItem.Tag = viewType;
      itemsListView.Items.Add(listViewItem);

      AdjustListViewColumnSizes();
    }

    protected void RemoveViewListViewItem(Type viewType) {
      List<ListViewItem> itemsToRemove = new List<ListViewItem>(); ;
      foreach (ListViewItem item in itemsListView.Items)
        if (item.Tag as Type == typeof(ClassificationSolutionEstimatedClassValuesView))
          itemsToRemove.Add(item);

      foreach (ListViewItem item in itemsToRemove)
        itemsListView.Items.Remove(item);
    }
  }
}
