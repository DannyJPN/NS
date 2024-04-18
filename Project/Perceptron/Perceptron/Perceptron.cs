using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Perceptron
{

    public class Perceptron
    {
        public delegate void StatusUpdate(object sender, EventArgs e);
        public event StatusUpdate OnTrainStep;
        private double[] weights;
        private double[] inputs;
        private InputDescription[] inputDescriptions;
        private double output;
        private double learningRate;
        private double error;
        private int timeoutstep = 0;
        private bool step = false;
        //public DataGridView results { get; set; }
        public Perceptron() : this(2, 0.3)
        {
        }

        public Perceptron(int numberOfInputs, double learningRate)
        {
            this.learningRate = learningRate;
            weights = new double[numberOfInputs + 1];
            inputs = new double[numberOfInputs + 1];
            inputDescriptions = new InputDescription[numberOfInputs];
            for (int i = 0; i < inputDescriptions.Length; i++)
            {
                inputDescriptions[i] = new InputDescription("Input " + i, 0, 1);
            }
        }

        public String Name { get; set; }

        public int TimeOutStep { get { return timeoutstep; } set { timeoutstep = value; } }
        public bool Step { get { return step; } set { step = value; } }
        // public void Train(List<double[]> pointlist, List<double> expected, int cycles)
        public void Train(List<TrainSetElement> inputlist, int cycles)
        {
            //List<double[]> points = AddBiasInput(pointlist);
            ;


            for (int c = 0; c < cycles; c++)
            {
                CurrentIter = c;
                double totalerror = 0;
                for (int idx = 0; idx < inputlist.Count; idx++)
                {
                    CurrentInputIndex = idx;
                    this.inputs[0] = 1;
                    Array.Copy(inputlist[idx].Inputs, 0, this.inputs, 1, inputlist[idx].Inputs.Length);
                    this.output = this.EvalFunc();
                    this.error = inputlist[idx].Output - this.output;
                    totalerror += Math.Abs(this.error);
                    Console.WriteLine(String.Format("Epoch {0},pointIDX {1},output {2},error {3},point [{4}],expected {5} ", c, idx, this.output, this.error, String.Join(";", this.inputs), inputlist[idx].Output));

                    this.RecalcWeights();



                    this.SaveWeights(String.Format("Weights_{0}.txt", (c)));
                    OnTrainStep(this, null);
                    if (timeoutstep > 0)
                    {
                        System.Threading.Thread.Sleep(timeoutstep);
                    }
                    else
                    {
                        //   while (!step) { System.Threading.Thread.Sleep(200); }
                    }
                }

                Console.WriteLine(String.Format("Epoch {0} finished with weights [{1}]", c, string.Join(";", this.weights)));
                if (totalerror == 0)
                {
                    Console.WriteLine("LEARNED!!");
                    break;

                }
            }
        }

        public List<double> Test(List<TestSetElement> inputlist)
        {
            //List<double[]> points = AddBiasInput(pointlist);

            List<double> outputs = new List<double>();

            for (int idx = 0; idx < inputlist.Count; idx++)
            {
                this.inputs[0] = 1;
                Array.Copy(inputlist[idx].Inputs, 0, this.inputs, 1, inputlist[idx].Inputs.Length);

                Console.WriteLine(String.Format("Classifying pointIDX {0},point [{1}],output {2} ",  idx,  String.Join(";", this.inputs), this.output));

                outputs.Add(this.EvalFunc()); 
            }
            return outputs;


        }

        private int EvalFunc()
        {
            //Vector<double> vectinputs = new Vector<double>(this.inputs);
            //Vector<double> vectweights = new Vector<double>(this.weights);
            //double suma = Vector.Dot<double>(vectinputs, vectweights);
            double realsuma = DotProduct(inputs, weights);
            //Console.WriteLine("Vector {0}({1}) weights {2}({3}) has dot {4}({5})", vectinputs, String.Join(";", inputs), vectweights, String.Join(";", weights), suma, realsuma);
            return realsuma > 0 ? 1 : 0;
        }

        private double DotProduct(double[] inputs, double[] weights)
        {
            double sum = 0;
            int len = inputs.Length > weights.Length ? weights.Length : inputs.Length;
            for (int i = 0; i < len; i++)
            {
                sum += inputs[i] * weights[i];
            }
            return sum;
        }

        private void RecalcWeights()
        {
            for (int idx = 0; idx < this.weights.Length; idx++)
            {
                // Console.WriteLine(String.Format("{0} = {1} + {2}*{3}*{4}", this.weights[idx] + this.learningRate * this.error * this.output, this.weights[idx], this.learningRate, this.error, this.output));
                this.weights[idx] += this.learningRate * this.error * this.inputs[idx];
            }
        }

        /* private List<double[]> AddBiasInput(List<double[]> pointlist)
         {
             List<double[]> points = new List<double[]>();
             foreach (double[] point in points)
             {
                 double[] newpoint = new double[point.Length + 1];
                 newpoint[0] = 1;
                 Array.Copy(point, 0,newpoint, 1, point.Length);
                 points.Add(newpoint);
             }
             return points;
         }*/

        private void SaveWeights(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine(String.Join(";", this.weights));
            }
        }
        [XmlArray("weights")]
        [XmlArrayItem("weight")]

        public double[] Weights { get { return weights; } set { weights = value; } }
        public double[] Inputs { get { return inputs; } set { inputs = value; } }




        public double Output { get { return output; } }



        [XmlElement("inputDescriptions")]
        public InputDescription[] InputDescriptions
        {
            get { return inputDescriptions; }
            set
            {
                inputDescriptions = value;
                if (inputDescriptions.Length != inputs.Length + 1)
                {
                    inputs = new double[inputDescriptions.Length + 1];
                }
            }
        }

        [XmlElement("learningrate")]
        public double LearningRate
        {
            get { return learningRate; }
            set { learningRate = value; }
        }

        public int CurrentInputIndex { get; set; }

        public int CurrentIter { get; set; }

        public override String ToString()
        {
            return "Perceptron [weights=" + String.Join(",", weights) + ", inputs="
                    + String.Join(",", inputs) + ", inputDescriptions="
                    + String.Join(",", (object[])inputDescriptions) + ", output=" + output
                    + ", learningRate=" + learningRate + ", name=" + Name + "]";
        }


    }
}
