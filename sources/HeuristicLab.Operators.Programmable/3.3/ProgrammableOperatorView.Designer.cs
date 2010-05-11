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

namespace HeuristicLab.Operators.Programmable {
  partial class ProgrammableOperatorView {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      System.Windows.Forms.TabPage tabPage2;
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.splitContainer2 = new System.Windows.Forms.SplitContainer();
      this.assembliesTreeView = new System.Windows.Forms.TreeView();
      this.namespacesTreeView = new System.Windows.Forms.TreeView();
      this.showCodeButton = new System.Windows.Forms.Button();
      this.compileButton = new System.Windows.Forms.Button();
      this.codeEditor = new HeuristicLab.CodeEditor.CodeEditor();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.parameterCollectionView = new HeuristicLab.Core.Views.ParameterCollectionView();
      tabPage2 = new System.Windows.Forms.TabPage();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
      tabPage2.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.SuspendLayout();
      // 
      // nameTextBox
      // 
      this.errorProvider.SetIconAlignment(this.nameTextBox, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
      this.errorProvider.SetIconPadding(this.nameTextBox, 2);
      this.nameTextBox.Size = new System.Drawing.Size(913, 20);
      // 
      // descriptionTextBox
      // 
      this.descriptionTextBox.Size = new System.Drawing.Size(913, 20);
      // 
      // tabPage2
      // 
      tabPage2.Controls.Add(this.splitContainer1);
      tabPage2.Location = new System.Drawing.Point(4, 22);
      tabPage2.Name = "tabPage2";
      tabPage2.Padding = new System.Windows.Forms.Padding(3);
      tabPage2.Size = new System.Drawing.Size(977, 619);
      tabPage2.TabIndex = 1;
      tabPage2.Text = "Code";
      tabPage2.UseVisualStyleBackColor = true;
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(3, 3);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
      this.splitContainer1.Panel1.Controls.Add(this.showCodeButton);
      this.splitContainer1.Panel1.Controls.Add(this.compileButton);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.codeEditor);
      this.splitContainer1.Size = new System.Drawing.Size(971, 613);
      this.splitContainer1.SplitterDistance = 254;
      this.splitContainer1.TabIndex = 0;
      // 
      // splitContainer2
      // 
      this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.splitContainer2.Location = new System.Drawing.Point(3, 3);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer2.Panel1
      // 
      this.splitContainer2.Panel1.Controls.Add(this.assembliesTreeView);
      // 
      // splitContainer2.Panel2
      // 
      this.splitContainer2.Panel2.Controls.Add(this.namespacesTreeView);
      this.splitContainer2.Size = new System.Drawing.Size(248, 578);
      this.splitContainer2.SplitterDistance = 289;
      this.splitContainer2.TabIndex = 2;
      // 
      // assembliesTreeView
      // 
      this.assembliesTreeView.CheckBoxes = true;
      this.assembliesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.assembliesTreeView.Location = new System.Drawing.Point(0, 0);
      this.assembliesTreeView.Name = "assembliesTreeView";
      this.assembliesTreeView.Size = new System.Drawing.Size(248, 289);
      this.assembliesTreeView.TabIndex = 0;
      this.assembliesTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.assembliesTreeView_AfterCheck);
      // 
      // namespacesTreeView
      // 
      this.namespacesTreeView.CheckBoxes = true;
      this.namespacesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.namespacesTreeView.Location = new System.Drawing.Point(0, 0);
      this.namespacesTreeView.Name = "namespacesTreeView";
      this.namespacesTreeView.PathSeparator = ".";
      this.namespacesTreeView.Size = new System.Drawing.Size(248, 285);
      this.namespacesTreeView.TabIndex = 0;
      this.namespacesTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.namespacesTreeView_AfterCheck);
      // 
      // showCodeButton
      // 
      this.showCodeButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.showCodeButton.Location = new System.Drawing.Point(65, 587);
      this.showCodeButton.Name = "showCodeButton";
      this.showCodeButton.Size = new System.Drawing.Size(186, 23);
      this.showCodeButton.TabIndex = 0;
      this.showCodeButton.Text = "&Show Generated Code ...";
      this.showCodeButton.UseVisualStyleBackColor = false;
      this.showCodeButton.Click += new System.EventHandler(this.showCodeButton_Click);
      // 
      // compileButton
      // 
      this.compileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.compileButton.Location = new System.Drawing.Point(3, 587);
      this.compileButton.Name = "compileButton";
      this.compileButton.Size = new System.Drawing.Size(56, 23);
      this.compileButton.TabIndex = 1;
      this.compileButton.Text = "&Compile";
      this.compileButton.UseVisualStyleBackColor = true;
      this.compileButton.Click += new System.EventHandler(this.compileButton_Click);
      // 
      // codeEditor
      // 
      this.codeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.codeEditor.Location = new System.Drawing.Point(0, 0);
      this.codeEditor.Name = "codeEditor";
      this.codeEditor.Prefix = "";
      this.codeEditor.Size = new System.Drawing.Size(713, 613);
      this.codeEditor.Suffix = "";
      this.codeEditor.TabIndex = 0;
      this.codeEditor.UserCode = "";
      this.codeEditor.Validated += new System.EventHandler(this.codeEditor_Validated);
      // 
      // tabControl1
      // 
      this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(tabPage2);
      this.tabControl1.Location = new System.Drawing.Point(0, 52);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(985, 645);
      this.tabControl1.TabIndex = 7;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.parameterCollectionView);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(977, 619);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Parameters";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // parameterCollectionView1
      // 
      this.parameterCollectionView.Content = null;
      this.parameterCollectionView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.parameterCollectionView.Location = new System.Drawing.Point(3, 3);
      this.parameterCollectionView.Name = "parameterCollectionView1";
      this.parameterCollectionView.Size = new System.Drawing.Size(971, 613);
      this.parameterCollectionView.TabIndex = 0;
      // 
      // ProgrammableOperatorView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabControl1);
      this.Name = "ProgrammableOperatorView";
      this.Size = new System.Drawing.Size(985, 697);
      this.Controls.SetChildIndex(this.tabControl1, 0);
      this.Controls.SetChildIndex(this.nameTextBox, 0);
      this.Controls.SetChildIndex(this.nameLabel, 0);
      this.Controls.SetChildIndex(this.descriptionLabel, 0);
      this.Controls.SetChildIndex(this.descriptionTextBox, 0);
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
      tabPage2.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private HeuristicLab.Core.Views.ParameterCollectionView parameterCollectionView;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private HeuristicLab.CodeEditor.CodeEditor codeEditor;
    private System.Windows.Forms.Button compileButton;
    private System.Windows.Forms.Button showCodeButton;
    private System.Windows.Forms.SplitContainer splitContainer2;
    private System.Windows.Forms.TreeView assembliesTreeView;
    private System.Windows.Forms.TreeView namespacesTreeView;    
  }
}
