using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NSwag.Collections;

namespace Flight_Inspection_App
{
    class testModel : INotifyPropertyChanged
    {
        private int x = 5;
        private int y = 5;
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        private ObservableDictionary<int, int> d;
        public ObservableDictionary<int, int> D { get { return d; } set { this.d = value; } }
        private List<int> l;
        public List<int> L
        {
            get { return this.l; }
            set { this.l = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void notifyPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public void print()
        {
            Console.WriteLine("{0} , {1}", x, y);
        }

        public testModel(int a, int b)
        {
            this.d = new ObservableDictionary<int, int>();
            this.l = new List<int>();
            this.l.Add(1);
            this.d.Add(2, 2);
            this.d.Add(a, b);
        }
    }
}
