using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Inspection_App
{
    // Chunk class saves chunk info.
    class Chunk : INotifyPropertyChanged
    {
        //////// fields
        private string name;
        private ObservableCollection<double> values;
        private bool isFloat;
        private double currValue;
        private string corrChunk = "none";
        private double correlation = 0;
        // x axe is chunk, y axe is correlative chunk
        public Line lin_reg { get; set; }
        public double CurrValue {
            get
            {
                return currValue;                                
            }
            set
            {
                currValue = value;
                NotifyPropertyChanged("CurrValue");
            }
        }
        public string CorrChunk
        {
            get { return corrChunk; }
            set 
            {
                corrChunk = value;
                NotifyPropertyChanged("CorrChunk");     
            }      
        }
        public double Correlation
        {
            get { return correlation; }
            set
            {
                correlation = value;
                NotifyPropertyChanged("Correlation");   
            }
        }

        public string Name { get { return name; } set { NotifyPropertyChanged("Name"); name = value; } } 
        public bool IsFloat { get { return isFloat; } set { NotifyPropertyChanged("IsFloat"); isFloat = value; } }
        public ObservableCollection<double> Values {
            get { return values; } 
            set 
            {
                NotifyPropertyChanged("Values");
                values = value; 
            } 
        }

        public Chunk(string _name, string type)
        {
            name = _name;
            values = new AsyncObservableCollection<double>();       // changed from observerableCollection
            if (type == "double")
                isFloat = false;
            else           
                isFloat = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
