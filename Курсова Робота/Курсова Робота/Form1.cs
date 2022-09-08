using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Курсова_Робота
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int[,] incidenceMatrix;
        int numberOfVertices; 
        bool[,] passedVertices;
        List<List<int>> allCycles = new List<List<int>>();


        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            numberOfVertices = int.Parse(textBox1.Text);
            incidenceMatrix = new int[numberOfVertices, numberOfVertices];
            dataGridView1.RowCount = numberOfVertices + 1;
            dataGridView1.ColumnCount = numberOfVertices + 1;
            dataGridView1.AutoResizeColumns();
            for (int i = 0; i <= numberOfVertices; i++)
            {
                dataGridView1.Rows[0].Cells[i].Value = i;
                dataGridView1.Rows[i].Cells[0].Value = i;
            }

            for (int i = 1; i <= numberOfVertices; i++)
            {
                for (int j = 1; j <= numberOfVertices; j++)
                {
                    incidenceMatrix[i - 1, j - 1] = int.Parse(Interaction.InputBox(""));
                    dataGridView1.Rows[i].Cells[j].Value = incidenceMatrix[i - 1, j - 1];
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            var sw = new Stopwatch();
            sw.Start(); 

            for (int i = 0; i < numberOfVertices; i++)
                Finding_cycles(i);

            ShowCyclec();

            sw.Stop();
            label6.Text = $"Час: {sw.ElapsedMilliseconds}";
        }


        void Finding_cycles(int vertex_number)
        {
            passedVertices = new bool[numberOfVertices, numberOfVertices];
            int adjacentVertex, vertexChainDepth = 1;
            bool isVertexFound;
            var chainOfPassedVertices = new Stack<int>();
            chainOfPassedVertices.Push(vertex_number);

            while (vertexChainDepth > 0)
            {
                adjacentVertex = 1;
                isVertexFound = false;

                while (!isVertexFound && adjacentVertex < numberOfVertices)
                {
                    if (incidenceMatrix[chainOfPassedVertices.Peek(), adjacentVertex] != 0 && !chainOfPassedVertices.Contains(adjacentVertex) && !passedVertices[vertexChainDepth, adjacentVertex])
                        isVertexFound = true;
                    else
                        adjacentVertex++;
                }

                if (isVertexFound)
                {
                    vertexChainDepth++;
                    chainOfPassedVertices.Push(adjacentVertex);
                }
                else
                {
                    if (incidenceMatrix[vertex_number, chainOfPassedVertices.Peek()] == 1 && chainOfPassedVertices.Count > 2)
                    {
                        var foundCycle = new List<int>();
                        foundCycle = chainOfPassedVertices.ToList();
                        bool isListsAreNotEquivalent = true;

                        for (int i = 0; i < allCycles.Count; i++)
                        {
                            if (ListsAreEquivalent(foundCycle, allCycles[i]))
                            {
                                isListsAreNotEquivalent = false;
                                break;
                            }
                        }
                        if (isListsAreNotEquivalent)
                            allCycles.Add(foundCycle);
                    }

                    vertexChainDepth--;
                    passedVertices[vertexChainDepth, chainOfPassedVertices.Pop()] = true;

                    if (numberOfVertices - vertexChainDepth >= 2)
                    {
                        for (int i = 0; i < numberOfVertices; i++)
                            passedVertices[vertexChainDepth + 1, i] = false;
                    }
                }
            }
        }


        bool ListsAreEquivalent(List<int> list1, List<int> list2)
        {
            var list3 = (from item in list1 select item).ToList();
            list3.Sort();
            var list4 = (from item in list2 select item).ToList();
            list4.Sort();

            return list3.SequenceEqual(list4);
        }


        void ShowCyclec()
        {
            listBox1.Items.Clear();

            if (allCycles.Count == 0)
            {
                listBox1.Items.Add(" - ");
            }
            else
            {
                for (int i = 0; i < allCycles.Count; i++)
                {
                    i++;
                    string s = i + ")";
                    i--;
                    allCycles[i].Reverse();

                    foreach (var item in allCycles[i])
                    {
                        s += " " + item;
                    }

                    listBox1.Items.Add(s);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)//створює масив у якому усі вершини інцедентні усім вершинам
        {
            dataGridView1.Rows.Clear();
            numberOfVertices = int.Parse(textBox1.Text);
            incidenceMatrix = new int[numberOfVertices, numberOfVertices];
            dataGridView1.RowCount = numberOfVertices + 1;
            dataGridView1.ColumnCount = numberOfVertices + 1;
            dataGridView1.AutoResizeColumns();
            for (int i = 0; i <= numberOfVertices; i++)
            {
                dataGridView1.Rows[0].Cells[i].Value = i;
                dataGridView1.Rows[i].Cells[0].Value = i;
            }
            for (int i = 1; i <= numberOfVertices; i++)
            {
                for (int j = 1; j <= numberOfVertices; j++)
                {
                    if (i == j)
                        incidenceMatrix[i - 1, j - 1] = 0;
                    else
                        incidenceMatrix[i - 1, j - 1] = 1;

                    dataGridView1.Rows[i].Cells[j].Value = incidenceMatrix[i - 1, j - 1];
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int x = int.Parse(textBox2.Text);
            listBox2.Items.Clear();
            int i = 1;
            string s;
            foreach (var item in allCycles)
            {
                s = i + ")";
                if (item.Contains(x))
                {
                    foreach (var item2 in item)
                    {
                        s += " " + item2;
                    }
                    listBox2.Items.Add(s);
                    i++;
                }
            }
        }
    }
}
