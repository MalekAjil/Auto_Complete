using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoComplete
{
   public class Node
    {
        private string name;
        private int freq = 0;
        private List<Node> prev;
        private List<Node> next; 
        public Node(string nam)
        {
            this.name = nam;            
            prev = new List<Node>();
            next = new List<Node>();          
        }

        public Node()
        {
            // TODO: Complete member initialization            
            prev = new List<Node>();
            next = new List<Node>(); 
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public List<Node> Prev
        {
            get { return prev; }
            set { prev = value; }
        }
        public List<Node> Next
        {
            get { return next; }
            set { next = value; }
        }
        public int Freq
        {
            get { return freq; }
            set { freq = value; }
        }
    }
}
