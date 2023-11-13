using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoComplete
{
    public class Line
    {   
        private Node prev;
        private Node next;
        private int freq;

        public Node Prev
        {
            get { return prev; }
            set { prev = value; }
        }        

        public Node Next
        {
            get { return next; }
            set { next = value; }
        }
        public int Freq
        {
            get { return freq; }
            set { freq = value; }
        } 
        //public double weight;
        public Line() 
        {
            freq = 1;
        }
        public Line(int f,Node p,Node t)
        {   
            this.prev = p;
            this.next = t;
            this.freq = f;
        }
    }
}
