using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete
{
    public class AutoComplete
    {
        List<Node> nodes;

        public List<Node> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }
        List<Line> lines;
        public List<List<string>> words;// = new List<List<string>>();
        public AutoComplete()
        {
            words = new List<List<string>>();

        }
        //int wordsCount;
        
        public void cutWords(string txt)
        {
            string str;            
            int wordPos = 0,endPos=0;
            List<string> phras = new List<string>();
            for (int i = 0; i < txt.Length; i++)
            {
                if (txt[i] == ' ')
                {                   
                    str = txt[wordPos].ToString();
                    for (int j = wordPos + 1; j < i; j++)
                    {
                        str += txt[j];
                    }
                   // str = txt.Substring(wordPos, i - 1);
                    phras.Add(str);
                    if(i+1<txt.Length && txt[i+1]==' ')
                        i++;
                    wordPos = i + 1;
                }
                else if (txt[i] == '.'|| txt[i] == '\n' || txt[i] == '،' || txt[i] == '؟' || txt[i] == '!'  || txt[i] == '\t')
                {
                    endPos = i;
                    if(i+1<txt.Length)
                    {
                        if ((txt[i] == '.' || txt[i] == '\n' || txt[i] == '،' || txt[i] == '؟' || txt[i] == '!' || txt[i] == '\t') && (txt[i + 1] == '.' || txt[i + 1] == '\n' || txt[i + 1] == '،' || txt[i + 1] == '؟' || txt[i + 1] == '!' || txt[i + 1] == '\t'))
                        {                            
                            i++;                            
                        }
                    }
                    str = txt[wordPos].ToString();
                    for (int j = wordPos + 1; j < endPos; j++)
                    {
                        str += txt[j];
                    }
                    //str = txt.Substring(wordPos, endPos - 1);
                    phras.Add(str);                    
                    wordPos = i + 1;
                    words.Add(phras);
                    phras = new List<string>();
                }
            }           
        }

        public void train()
        {
            string str;           
            Node tempNode1, tempNode2, root;
            Line tempLine;
            root = new Node("");
            nodes = new List<Node>();
            lines = new List<Line>();
            nodes.Add(root);
            for (int i = 0; i < words.Count; i++)
            {
                for (int j = 0; j < words[i].Count; j++)
                {           
                    str = words[i][j];                    
                    tempNode1 = BFSsearch(str);
                    if (tempNode1 == null)
                    {
                        tempNode1 = new Node(str);
                        if (j == 0)
                        {
                            tempNode1.Prev.Add(root);
                            nodes[0].Next.Add(tempNode1);
                            tempNode1.Freq += 1;
                            tempLine = new Line();
                            tempLine.Prev = root;
                            tempLine.Next = tempNode1;
                            tempLine.Freq = 1;
                            lines.Add(tempLine);
                        }
                        else
                        {
                            tempNode2 = BFSsearch(words[i][j - 1]);
                            if (tempNode2 != null)
                            {
                                tempNode1.Prev.Add(tempNode2);
                                tempNode1.Freq += 1;
                                for (int n = 0; n < nodes.Count; n++)
                                {
                                    if (nodes[n].Name.CompareTo(words[i][j - 1])==0)
                                    {
                                        bool found = false;
                                        foreach (Node nd in nodes[n].Next)
                                        {
                                            if (nd.Name.CompareTo(tempNode1.Name) == 0)
                                                found = true;
                                        }
                                        if (!found)
                                            nodes[n].Next.Add(tempNode1); break;
                                    }
                                }

                                tempLine = findLine(tempNode2, tempNode1);
                                if (tempLine == null)
                                {
                                    tempLine = new Line();
                                    tempLine.Prev = tempNode2;
                                    tempLine.Next = tempNode1;
                                    tempLine.Freq = 1;
                                    lines.Add(tempLine);
                                }
                                else
                                {
                                    for (int l = 0; l < lines.Count; l++)
                                    {
                                        if (lines[l].Prev.Name.CompareTo(tempNode2.Name)==0 && lines[l].Next.Name.CompareTo(tempNode1.Name)==0)
                                        {
                                            lines[l].Freq++;
                                        }
                                    }
                                }
                            }
                        }
                        nodes.Add(tempNode1);
                    }
                    else
                    {
                        for (int n = 0; n < nodes.Count; n++)
                        {
                            if (nodes[n].Name.CompareTo(str)==0)
                            {
                                nodes[n].Freq += 1;
                                if (j != 0)
                                {
                                    tempNode2 = BFSsearch(words[i][j - 1]);
                                    if (tempNode2 != null)
                                    {
                                        nodes[n].Prev.Add(tempNode2);
                                        for (int m = 0; m < nodes.Count; m++)
                                        {
                                            if (nodes[m].Name.CompareTo(words[i][j - 1])==0)
                                            {
                                                bool found = false;
                                                foreach (Node nd in nodes[m].Next)
                                                {
                                                    if (nd.Name.CompareTo(nodes[n].Name) == 0)
                                                        found = true;
                                                }
                                                if(!found)
                                                    nodes[m].Next.Add(nodes[n]); break;
                                            }
                                        }
                                        tempLine = findLine(tempNode2, nodes[n]);
                                        if (tempLine == null)
                                        {
                                            tempLine = new Line();
                                            tempLine.Prev = tempNode2;
                                            tempLine.Next = nodes[n];
                                            tempLine.Freq = 1;
                                            lines.Add(tempLine);
                                        }
                                        else
                                        {
                                            for (int l = 0; l < lines.Count; l++)
                                            {
                                                if (lines[l].Prev.Name.CompareTo(tempNode2.Name)==0 && lines[l].Next.Name.CompareTo(nodes[n].Name)==0)
                                                {
                                                    lines[l].Freq++;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int l = 0; l < lines.Count; l++)
                                    {
                                        if (lines[l].Prev.Name.CompareTo(root.Name)==0 && lines[l].Next.Name.CompareTo(nodes[n].Name)==0)
                                        {
                                            lines[l].Freq++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public Node findNode(string nam)
        {
            foreach (Node n in nodes)
            {                
                if (n.Name.CompareTo(nam)==0)
                    return n;
            }
            return null;
        }
        public Line findLine(Node n1,Node n2)
        {
            //bool found = false;
            foreach (Line l in lines)
            {
                if (l.Prev.Name.CompareTo(n1.Name)==0 && l.Next.Name.CompareTo(n2.Name)==0)
                {
                    return l;
                }
            }
            return null;
        }
        public Node BFSsearch(string str)
        {
            if (nodes.Count > 0)
            {
                Node root = nodes[0];
                LinkedList<Node> Closed = new LinkedList<Node>();
                LinkedList<Node> Open = new LinkedList<Node>();
                Open.AddLast(root);
                while (Open.Count != 0)
                {
                    Node x = Open.ElementAt(0);
                    Open.RemoveFirst();
                    Closed.AddLast(x);
                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (x.Name.CompareTo( lines.ElementAt(i).Prev.Name)==0)
                        {
                            Node y = lines.ElementAt(i).Next;
                            if (!(Open.Contains(y)) && !(Closed.Contains(y)))
                            {
                                Open.AddLast(y);                                
                                if (y.Name.CompareTo(str)==0)
                                    return y;
                            }
                        }

                    }
                }
            }           
            return null;
        }

    }
}
