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
using System.Timers;
using HeuristicLab.Hive.Client.Common;

namespace HeuristicLab.Hive.Client.Core {
  /// <summary>
  /// Heartbeat class. It sends every x ms a heartbeat to the server and receives a Message
  /// </summary>
  public class Heartbeat {
    public double Interval { get; set; }    
    private Timer heartbeatTimer = null;
        
    public Heartbeat() {
      Interval = 100;
    }

    public Heartbeat(double interval) {
      Interval = interval;      
    }

    /// <summary>
    /// Starts the Heartbeat signal.
    /// </summary>
    public void StartHeartbeat() {
      heartbeatTimer = new System.Timers.Timer();
      heartbeatTimer.Interval = this.Interval;
      heartbeatTimer.AutoReset = true;
      heartbeatTimer.Elapsed += new ElapsedEventHandler(heartbeatTimer_Elapsed);
      heartbeatTimer.Start();               
    }

    /// <summary>
    /// This Method is called every time the timer ticks
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void heartbeatTimer_Elapsed(object sender, ElapsedEventArgs e) {
      Console.WriteLine("tick");
      MessageQueue.GetInstance().AddMessage(MessageContainer.MessageType.FetchJob);
    }

  }
}
