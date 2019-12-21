using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace KineticEnergy
{
    class ViewModel : INotifyPropertyChanged
    {
        private static ViewModel instance = null;
        public static ViewModel Instance
        { get { return instance ?? (instance = new ViewModel()); } }

        private double massValue;
        public double MassValue
        {
            get { return massValue; }
            set
            {   //Update energy value when MassValue is changed
                massValue = value;
                EnergyValue = CalcKineticEnergy(massValue, velocityValue);
                NotifyChange();
            }
        }

        private double velocityValue;
        public double VelocityValue
        {
            get
            { return velocityValue; }
            set
            {   //Update energy value when VelocityValue is changed
                velocityValue = value;
                EnergyValue = CalcKineticEnergy(massValue, velocityValue);
                NotifyChange();
            }
        }

        private double energyValue;
        public double EnergyValue
        { get { return energyValue; } set { energyValue = value; NotifyChange(); } }

        public double CalcKineticEnergy(double mass, double velocity)
        {
            return Math.Round(mass * velocity * velocity / 2, 2);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyChange([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Add balls to canvas
        private ObservableCollection<Ellipse> balls;
        public ObservableCollection<Ellipse> Balls
        {
            get { return balls ?? (balls = new ObservableCollection<Ellipse>()); }
            set { balls = value; }
        }

        //Add labels to listbox
        private ObservableCollection<ListBoxItem> labels;
        public ObservableCollection<ListBoxItem> Labels
        {
            get { return labels ?? (labels = new ObservableCollection<ListBoxItem>()); }
            set { labels = value; }
        }
    }
}
