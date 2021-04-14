using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Inspection_App
{
    class Chunk : INotifyPropertyChanged
    {
        private string name;
        //private List<T> values;        // problem!
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
                return currValue;                                // todo:may be problem for some chunks!
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
                NotifyPropertyChanged("CorrChunk");     // added. not sure needed
            }      
        }
        public double Correlation
        {
            get { return correlation; }
            set
            {
                correlation = value;
                NotifyPropertyChanged("Correlation");   // added. not sure needed
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

        /* private string name;
         private string type;
         private double dValue = 0;
         private float fValue = 0;
         private float Value
         {
             get
             {
                 if (dValue == 0)
                     return fValue;
                 else
                     return dValue;
             }
             set
             {

             }
         }

         public Chunk(string name, string type)
         {
             this.name = name;
             if (type == "double")
             {
                 T = Type.Double;
             }
         }

         public string Name {
             get { return name; } 
             set
             {
                 if (value != name)
                     name = value;
             }
         }
         public string Type
         {
             get { return type; }
             set
             {
                 if (value != type)
                     type = value;
             }
         }
         public float Value
         {
             get { return cValue; }
             set
             {
                 if (value != cValue)
                     cValue = value;
             }
         }*/

        /*public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }*/
    }
}
