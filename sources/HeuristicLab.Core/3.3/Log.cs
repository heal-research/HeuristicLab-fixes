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
using System.Drawing;
using HeuristicLab.Common;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.PluginInfrastructure;

namespace HeuristicLab.Core {
  [Item("Log", "A log for logging string messages.")]
  [StorableClass]
  public class Log : Item, ILog, IStorableContent {
    public string Filename { get; set; }

    public static new Image StaticItemImage {
      get { return HeuristicLab.Common.Resources.VSImageLibrary.File; }
    }

    [Storable]
    protected IList<string> messages;
    public virtual IEnumerable<string> Messages {
      get { return messages; }
    }

    [Storable]
    protected long maxMessageCount;
    public virtual long MaxMessageCount {
      get { return maxMessageCount; }
    }

    [StorableConstructor]
    protected Log(bool deserializing) : base(deserializing) { }
    protected Log(Log original, Cloner cloner)
      : base(original, cloner) {
      this.messages = new List<string>(original.messages);
      this.maxMessageCount = original.maxMessageCount;
    }
    public Log(long maxMessageCount = -1)
      : base() {
      this.messages = new List<string>();
      this.maxMessageCount = maxMessageCount;
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new Log(this, cloner);
    }

    public virtual void Clear() {
      messages.Clear();
      OnCleared();
    }
    public virtual void LogMessage(string message) {
      string s = FormatLogMessage(message);
      messages.Add(s);
      CapMessages();
      OnMessageAdded(s);
    }
    public virtual void LogException(Exception ex) {
      string s = FormatException(ex);
      messages.Add(s);
      CapMessages();
      OnMessageAdded(s);
    }
    protected virtual void CapMessages() {
      while (maxMessageCount >= 0 && messages.Count > maxMessageCount) {
        messages.RemoveAt(0);
      }
    }
    protected virtual string FormatLogMessage(string message) {
      return DateTime.Now.ToString() + "\t" + message;
    }
    protected virtual string FormatException(Exception ex) {
      return DateTime.Now.ToString() + "\t" + "Exception occurred:" + Environment.NewLine + ErrorHandling.BuildErrorMessage(ex);
    }

    public event EventHandler<EventArgs<string>> MessageAdded;
    protected virtual void OnMessageAdded(string message) {
      EventHandler<EventArgs<string>> handler = MessageAdded;
      if (handler != null) handler(this, new EventArgs<string>(message));
    }
    public event EventHandler Cleared;
    protected virtual void OnCleared() {
      EventHandler handler = Cleared;
      if (handler != null) handler(this, EventArgs.Empty);
    }
  }
}
