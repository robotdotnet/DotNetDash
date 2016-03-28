using NetworkTables.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace DotNetDash.SpeedController
{
    public class ControllerModel : NetworkTableContext
    {
        private static List<ControlMode> controlModes;
        private object syncRoot = new object();
        public IEnumerable<ControlMode> ControlModes => controlModes;

        public ControllerModel(string tableName, ITable table) : base(tableName, table)
        {
            if(controlModes == null)
            {
                FillControlModeOptions();
            }
            table.AddTableListenerOnSynchronizationContext(SynchronizationContext.Current, (modifiedTable, key, value, _) =>
            {
                switch (key)
                {
                    case nameof(Mode):
                        Mode = (ControlMode)(int)(double)value;
                        break;
                    case "Value":
                        ShowValue((double)value);
                        break;
                    default:
                        break;
                }
            }, NetworkTables.NotifyFlags.NotifyImmediate | NetworkTables.NotifyFlags.NotifyUpdate | NetworkTables.NotifyFlags.NotifyNew);
            ZeroOutput = new Command(() => Numbers["Value"] = 0.0);
            ClearGraph = new Command(() =>
                {
                    lock (syncRoot)
                    {
                        OutputPoints.Clear();
                        SetpointLine.Clear(); 
                    }
                });
        }

        private static void FillControlModeOptions()
        {
            var modes = (ControlMode[])Enum.GetValues(typeof(ControlMode));
            controlModes = modes.ToList();
        }

        private void ShowValue(double value)
        {
            // All missing modes do not require any special code.  They can just directly bind to [Value]
            switch (Mode)
            {
                case ControlMode.Position:
                case ControlMode.Speed:
                case ControlMode.Current:
                    lock (syncRoot)
                    {
                        OutputPoints.Add(new OxyPlot.DataPoint(OutputPoints.Count, value));
                        SetpointLine.Add(new OxyPlot.DataPoint(SetpointLine.Count, Setpoint)); 
                    }
                    break;
            }
        }

        public ObservableCollection<OxyPlot.DataPoint> OutputPoints { get; } = new ObservableCollection<OxyPlot.DataPoint>();
        public ObservableCollection<OxyPlot.DataPoint> SetpointLine { get; } = new ObservableCollection<OxyPlot.DataPoint>();

        private ControlMode mode;

        public ControlMode Mode
        {
            get { return mode; }
            set
            {
                if (mode != value)
                {
                    mode = value;
                    Numbers[nameof(Mode)] = (int)value;
                    NotifyPropertyChanged();
                    OutputPoints.Clear();
                    SetpointLine.Clear();
                } 

            }
        }

        private double setpoint;

        public double Setpoint
        {
            get { return setpoint; }
            set
            {
                setpoint = value;
                Numbers["Value"] = value; //Propagate value back to the speed controller
            }
        }

        public Command ZeroOutput { get; }
        public Command ClearGraph { get; }
    }
}
