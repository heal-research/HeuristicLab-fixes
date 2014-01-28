﻿using System;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Threading;
using HeuristicLab.Common;

namespace HeuristicLab.HLScript {
  public abstract class UserScriptBase {
    protected dynamic vars;

    private readonly EventWriter console;
    protected EventWriter Console {
      get { return console; }
    }

    protected UserScriptBase() {
      console = new EventWriter(this);
    }

    public abstract void Main();

    private void Execute(VariableStore variableStore) {
      vars = new Variables(variableStore);
      try {
        Main();
      } catch (ThreadAbortException) {
      } catch (Exception e) {
        Console.WriteLine("---");
        Console.WriteLine(e);
      }
    }

    protected internal event EventHandler<EventArgs<string>> ConsoleOutputChanged;
    private void OnConsoleOutputChanged(string args) {
      var handler = ConsoleOutputChanged;
      if (handler != null) handler(null, new EventArgs<string>(args));
    }

    private class Variables : DynamicObject {
      private readonly VariableStore variableStore;

      public Variables(VariableStore variableStore) {
        this.variableStore = variableStore;
      }

      public override bool TryGetMember(GetMemberBinder binder, out object result) {
        return variableStore.TryGetValue(binder.Name, out result);
      }

      public override bool TrySetMember(SetMemberBinder binder, object value) {
        variableStore[binder.Name] = value;
        return true;
      }
    }

    protected class EventWriter : TextWriter {
      private readonly UserScriptBase usb;

      public EventWriter(UserScriptBase usb) {
        this.usb = usb;
      }

      public override Encoding Encoding {
        get { return Encoding.UTF8; }
      }

      #region Write/WriteLine Overrides
      #region Write
      public override void Write(bool value) { usb.OnConsoleOutputChanged(value.ToString()); }
      public override void Write(char value) { usb.OnConsoleOutputChanged(value.ToString()); }
      public override void Write(char[] buffer) { usb.OnConsoleOutputChanged(new string(buffer)); }
      public override void Write(char[] buffer, int index, int count) { usb.OnConsoleOutputChanged(new string(buffer, index, count)); }
      public override void Write(decimal value) { usb.OnConsoleOutputChanged(value.ToString()); }
      public override void Write(double value) { usb.OnConsoleOutputChanged(value.ToString()); }
      public override void Write(float value) { usb.OnConsoleOutputChanged(value.ToString()); }
      public override void Write(int value) { usb.OnConsoleOutputChanged(value.ToString()); }
      public override void Write(long value) { usb.OnConsoleOutputChanged(value.ToString()); }
      public override void Write(object value) { usb.OnConsoleOutputChanged(value.ToString()); }
      public override void Write(string value) { usb.OnConsoleOutputChanged(value); }
      public override void Write(string format, object arg0) { usb.OnConsoleOutputChanged(string.Format(format, arg0)); }
      public override void Write(string format, object arg0, object arg1) { usb.OnConsoleOutputChanged(string.Format(format, arg0, arg0)); }
      public override void Write(string format, object arg0, object arg1, object arg2) { usb.OnConsoleOutputChanged(string.Format(format, arg0, arg1, arg2)); }
      public override void Write(string format, params object[] arg) { usb.OnConsoleOutputChanged(string.Format(format, arg)); }
      public override void Write(uint value) { usb.OnConsoleOutputChanged(value.ToString()); }
      public override void Write(ulong value) { usb.OnConsoleOutputChanged(value.ToString()); }
      #endregion

      #region WriteLine
      public override void WriteLine() { usb.OnConsoleOutputChanged(Environment.NewLine); }
      public override void WriteLine(bool value) { usb.OnConsoleOutputChanged(value + Environment.NewLine); }
      public override void WriteLine(char value) { usb.OnConsoleOutputChanged(value + Environment.NewLine); }
      public override void WriteLine(char[] buffer) { usb.OnConsoleOutputChanged(new string(buffer) + Environment.NewLine); }
      public override void WriteLine(char[] buffer, int index, int count) { usb.OnConsoleOutputChanged(new string(buffer, index, count) + Environment.NewLine); }
      public override void WriteLine(decimal value) { usb.OnConsoleOutputChanged(value + Environment.NewLine); }
      public override void WriteLine(double value) { usb.OnConsoleOutputChanged(value + Environment.NewLine); }
      public override void WriteLine(float value) { usb.OnConsoleOutputChanged(value + Environment.NewLine); }
      public override void WriteLine(int value) { usb.OnConsoleOutputChanged(value + Environment.NewLine); }
      public override void WriteLine(long value) { usb.OnConsoleOutputChanged(value + Environment.NewLine); }
      public override void WriteLine(object value) { usb.OnConsoleOutputChanged(value + Environment.NewLine); }
      public override void WriteLine(string value) { usb.OnConsoleOutputChanged(value + Environment.NewLine); }
      public override void WriteLine(string format, object arg0) { usb.OnConsoleOutputChanged(string.Format(format, arg0) + Environment.NewLine); }
      public override void WriteLine(string format, object arg0, object arg1) { usb.OnConsoleOutputChanged(string.Format(format, arg0, arg1) + Environment.NewLine); }
      public override void WriteLine(string format, object arg0, object arg1, object arg2) { usb.OnConsoleOutputChanged(string.Format(format, arg0, arg1, arg2) + Environment.NewLine); }
      public override void WriteLine(string format, params object[] arg) { usb.OnConsoleOutputChanged(string.Format(format, arg) + Environment.NewLine); }
      public override void WriteLine(uint value) { usb.OnConsoleOutputChanged(value + Environment.NewLine); }
      public override void WriteLine(ulong value) { usb.OnConsoleOutputChanged(value + Environment.NewLine); }
      #endregion
      #endregion
    }
  }
}