using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;

using D3Sharp.Force;

namespace D3Sharp.Demo.Model
{
    public enum Relationship
    {
        Realization,
        Generalization,
        Association,
        Aggregation,
        Composition,
        Dependency
    }

    public class DemoLink : Link, INotifyPropertyChanged
    {
        public Relationship Relationship { get; set; }

        public double MiddleX
        {
            get
            {
                var x1 = ((DemoNode)Source).X;
                var x2 = ((DemoNode)Target).X;
                return (x2 - x1) / 2 + x1;
            }
        }

        public double MiddleY
        {
            get
            {
                var y1 = ((DemoNode)Source).Y;
                var y2 = ((DemoNode)Target).Y;
                return (y2 - y1) / 2 + y1;
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public void Update()
        {
            OnPropertyChanged(nameof(MiddleX));
            OnPropertyChanged(nameof(MiddleY));
        }
    }
}
