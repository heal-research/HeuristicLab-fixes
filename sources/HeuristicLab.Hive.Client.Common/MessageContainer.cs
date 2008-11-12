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

namespace HeuristicLab.Hive.Client.Common {
  /// <summary>
  /// The MessageContainer is a container class for Messages. Its two parts are:
  /// the actual message itself and the JobId, refered by the message
  /// </summary>
  public class MessageContainer {

    public enum MessageType { FetchJob, AbortJob, RequestSnapshot, FinishedJob };

    public MessageType Message { get; set; }
    public int JobId { get; set; }

    public MessageContainer(MessageType message) {
      Message = message;
      JobId = 0;
    }
    public MessageContainer(MessageType message, int jobId) {
      Message = message;
      JobId = jobId;
    }

  }
}
