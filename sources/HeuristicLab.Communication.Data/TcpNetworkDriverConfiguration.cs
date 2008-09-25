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
using System.Text;
using System.Xml;
using System.Net.Sockets;
using HeuristicLab.Core;
using HeuristicLab.Data;

namespace HeuristicLab.Communication.Data {
  public class TcpNetworkDriverConfiguration : ItemBase, IDriverConfiguration {
    private StringData targetIPAddress;
    public StringData TargetIPAddress {
      get { return targetIPAddress; }
      set {
        targetIPAddress = value;
        OnChanged();
      }
    }

    private IntData targetPort;
    public IntData TargetPort {
      get { return targetPort; }
      set {
        targetPort = value;
        OnChanged();
      }
    }

    private IntData sourcePort;
    public IntData SourcePort {
      get { return sourcePort; }
      set {
        sourcePort = value;
        OnChanged();
      }
    }

    public TcpNetworkDriverConfiguration() {
      targetIPAddress = new StringData("127.0.0.1");
      targetPort = new IntData(2552);
      sourcePort = new IntData(5225);
    }

    public override IView CreateView() {
      return new TcpNetworkDriverConfigurationView(this);
    }

    #region persistence & clone
    public override XmlNode GetXmlNode(string name, XmlDocument document, IDictionary<Guid, IStorable> persistedObjects) {
      XmlNode node = base.GetXmlNode(name, document, persistedObjects);
      XmlNode ipNode = PersistenceManager.Persist("TargetIP", TargetIPAddress, document, persistedObjects);
      node.AppendChild(ipNode);
      XmlNode tportNode = PersistenceManager.Persist("TargetPort", TargetPort, document, persistedObjects);
      node.AppendChild(tportNode);
      XmlNode sportNode = PersistenceManager.Persist("SourcePort", SourcePort, document, persistedObjects);
      node.AppendChild(sportNode);
      return node;
    }

    public override void Populate(XmlNode node, IDictionary<Guid, IStorable> restoredObjects) {
      base.Populate(node, restoredObjects);
      targetIPAddress = (StringData)PersistenceManager.Restore(node.SelectSingleNode("TargetIP"), restoredObjects);
      targetPort = (IntData)PersistenceManager.Restore(node.SelectSingleNode("TargetPort"), restoredObjects);
      sourcePort = (IntData)PersistenceManager.Restore(node.SelectSingleNode("SourcePort"), restoredObjects);
    }

    public override object Clone(IDictionary<Guid, object> clonedObjects) {
      TcpNetworkDriverConfiguration clone = new TcpNetworkDriverConfiguration();
      clonedObjects.Add(Guid, clone);
      clone.targetIPAddress = (StringData)Auxiliary.Clone(TargetIPAddress, clonedObjects);
      clone.targetPort = (IntData)Auxiliary.Clone(TargetPort, clonedObjects);
      clone.sourcePort = (IntData)Auxiliary.Clone(SourcePort, clonedObjects);
      return clone;
    }
    #endregion
  }
}
