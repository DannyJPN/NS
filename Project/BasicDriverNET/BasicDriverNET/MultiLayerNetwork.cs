using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BackPropag
{
    [XmlRoot("backpropagationNeuronNet")]
    public class MultiLayerNetwork
    {
        public delegate void StatusUpdate(object sender, EventArgs e);
        public event StatusUpdate OnTrainStep;
        public int TimeOutStep { get { return timeoutstep; } set { timeoutstep = value; } }
        public bool Step { get { return step; } set { step = value; } }
        private int timeoutstep = 0;
        private bool step = false;
        private double learningRate;
        public double TotalError = 0;
        private int dimension;
        public int IterStep = 15000;
        private InputDescription[] inputDescriptions;
        private List<TrainSetElement> trainSet;
        private List<TestSetElement> testSet;
        [XmlArray("trainSet")]
        [XmlArrayItem("trainSetElement")]
        public List<TrainSetElement> TrainSet
        {
            get { return trainSet; }
            set { trainSet = value; }
        }

        [XmlArray("testSet")]
        [XmlArrayItem("testSetElement")]
        public List<TestSetElement> TestSet
        {
            get { return testSet; }
            set { testSet = value; }
        }
        public int CurrentInputIndex { get; set; }

        public int CurrentIter { get; set; }
        public List<List<Neuron>> NeuralNetwork { get; set; }
        [XmlElement("lastStepInfluenceLearningRate")]
        public double lastStepInfluenceLearningRate { get; set; }
        private double[] inputs;
        private int[] layer_definition;
        public double[] Inputs { get { return inputs; } set { inputs = value; } }
        public double[] OrigInputs { get { return originputs; } set { originputs = value; } }
        [XmlArray("neuronInLayersCount")]
        [XmlArrayItem("neuronInLayerCount")]
        public int[] Layer_Definition
        {
            get { return layer_definition; }
            set
            {
                layer_definition = value;
                this.errors = new double[layer_definition[layer_definition.Length - 1]];
                this.NeuralNetwork = new List<List<Neuron>>();
                for (int l = 0; l < layer_definition.Length; l++)
                {
                    this.NeuralNetwork.Add(new List<Neuron>());
                    for (int j = 0; j < layer_definition[l]; j++)
                    {
                        if (l == 0)
                        {
                            this.NeuralNetwork[l].Add(new Neuron(this.dimension, this.learningRate));

                            this.NeuralNetwork[l][j].InputNeuron = true;
 
                            


                            Console.WriteLine("LAYDEF Adding Neuron {0} of dimension {1} in layer {2} {3}", j, this.dimension, l, this.NeuralNetwork[l][j].InputNeuron);
                            this.NeuralNetwork[l][j].InputDescriptions = this.InputDescriptions;
                        }
                        else
                        {
                            this.NeuralNetwork[l].Add(new Neuron(layer_definition[l - 1], this.learningRate));
                            this.NeuralNetwork[l][j].InputNeuron = false;
                            Console.WriteLine("LAYDEF Adding Neuron {0} of dimension {1} in layer {2} {3}", j, this.layer_definition[l - 1], l, this.NeuralNetwork[l][j].InputNeuron);
                        }
                        this.NeuralNetwork[l][j].output = 0;
                        this.NeuralNetwork[l][j].error = 0;
                    }
                }
            }
        }
        private double[] errors;
        private string[] outputDescriptions;
        private double[] originputs;

        [XmlArray("outputDescriptions")]
        [XmlArrayItem("outputDescription")]

        public string[] OutputDescriptions
        {
            get { return outputDescriptions; }
            set { outputDescriptions = value; }
        }

        [XmlElement("learningrate")]
        public double LearningRate
        {
            get { return learningRate; }
            set { learningRate = value; }
        }
        [XmlElement("inputsCount")]
        public int Dimension
        {
            get { return dimension; }
            set { dimension = value; }
        }
        public MultiLayerNetwork() : this(2, 0.1)

        {

        }

        public MultiLayerNetwork(int dimension, double learningRate)
        {
            this.learningRate = learningRate;
            this.dimension = dimension;
            this.NeuralNetwork = new List<List<Neuron>>();
            this.inputs = new double[this.dimension + 1];
            this.originputs = new double[this.dimension + 1];
            this.layer_definition = new int[] { 1 };
            this.errors = new double[layer_definition[layer_definition.Length - 1]];

            inputDescriptions = new InputDescription[this.dimension];
            for (int i = 0; i < inputDescriptions.Length; i++)
            {
                inputDescriptions[i] = new InputDescription("Input " + i, 0, 1);
                Console.WriteLine("Constructor 1 input {0}:{1}", i, inputDescriptions[i].Name);
            }
            for (int l = 0; l < layer_definition.Length; l++)
            {
                this.NeuralNetwork.Add(new List<Neuron>());
                for (int j = 0; j < layer_definition[l]; j++)
                {
                    if (l == 0)
                    {
                        this.NeuralNetwork[l].Add(new Neuron(this.dimension, this.learningRate));

                        this.NeuralNetwork[l][j].InputNeuron = true;
                        Console.WriteLine("CON1 Adding Neuron {0} of dimension {1} in layer {2} {3}", j, this.dimension, l, this.NeuralNetwork[l][j].InputNeuron);
                        this.NeuralNetwork[l][j].InputDescriptions = this.InputDescriptions;
                    }
                    else
                    {
                        this.NeuralNetwork[l].Add(new Neuron(layer_definition[l - 1], this.learningRate));
                        this.NeuralNetwork[l][j].InputNeuron = false;
                        Console.WriteLine("CON1 Adding Neuron {0} of dimension {1} in layer {2} {3}", j, this.layer_definition[l - 1], l, this.NeuralNetwork[l][j].InputNeuron);
                    }
                    this.NeuralNetwork[l][j].output = 0;
                    this.NeuralNetwork[l][j].error = 0;
                }

            }
        }
        public MultiLayerNetwork(int dimension, double learningRate, int[] layer_definition)
        {
            this.learningRate = learningRate;
            this.dimension = dimension;
            this.NeuralNetwork = new List<List<Neuron>>();
            this.inputs = new double[this.dimension + 1];
            this.originputs = new double[this.dimension + 1];
            this.layer_definition = layer_definition;
            this.errors = new double[layer_definition[layer_definition.Length - 1]];
            inputDescriptions = new InputDescription[this.dimension];
            for (int i = 0; i < inputDescriptions.Length; i++)
            {
                inputDescriptions[i] = new InputDescription("Input " + i, 0, 1);
                Console.WriteLine("Constructor 2 input {0}:{1}", i, inputDescriptions[i].Name);
            }
            for (int l = 0; l < layer_definition.Length; l++)
            {
                this.NeuralNetwork.Add(new List<Neuron>());
                for (int j = 0; j < layer_definition[l]; j++)
                {
                    if (l == 0)
                    {
                        this.NeuralNetwork[l].Add(new Neuron(this.dimension, this.learningRate));

                        this.NeuralNetwork[l][j].InputNeuron = true;
                        Console.WriteLine("CON2 Adding Neuron {0} of dimension {1} in layer {2} {3}", j, this.dimension, l, this.NeuralNetwork[l][j].InputNeuron);
                        this.NeuralNetwork[l][j].InputDescriptions = this.InputDescriptions;
                    }
                    else
                    {
                        this.NeuralNetwork[l].Add(new Neuron(layer_definition[l - 1], this.learningRate));
                        this.NeuralNetwork[l][j].InputNeuron = false;
                        Console.WriteLine("CON2 Adding Neuron {0} of dimension {1} in layer {2} {3}", j, this.layer_definition[l - 1], l, this.NeuralNetwork[l][j].InputNeuron);
                    }
                    this.NeuralNetwork[l][j].output = 0;
                    this.NeuralNetwork[l][j].error = 0;
                }
            }
        }

        public void Train(List<double[]> pointlist, List<List<double>> expected, int cycles)
        {
            this.errors = new double[layer_definition[layer_definition.Length - 1]];
            
            this.Outputs = new List<double>();
            TotalError = 0;
            for (int c = 0; c < cycles; c++)
            {
                CurrentIter = c;
                TotalError = 0;
                for (int i = 0; i < this.errors.Length; i++)
                {
                    this.errors[i] = 0;
                }
                for (int idx = 0; idx < pointlist.Count; idx++)
                {
                    CurrentInputIndex = idx;
                    OrigInputs = this.inputs = pointlist[idx];
                    // Console.WriteLine("Initial input vector {0}", String.Join(";",this.inputs));
                    //Console.WriteLine("Epoch {1} FeedForward {0} START",idx,c);

                    for (int layeridx = 0; layeridx < this.NeuralNetwork.Count; layeridx++)
                    {
                        List<double> outputs = new List<double>();
                        foreach (Neuron neuron in this.NeuralNetwork[layeridx])
                        {
                            
                            if (neuron.InputNeuron)
                            {
                                neuron.InputDescriptions = this.InputDescriptions;
                            }
                            
                            outputs.Add(this.GetOutput(neuron, this.inputs));
                            
                        }
                        if (layeridx < this.NeuralNetwork.Count - 1)
                        {
                            //Console.WriteLine("Layer {0} Inputs {1} generated outputs {2} as input for layer  {3}", layeridx, String.Join(";",this.inputs), String.Join(";", outputs), layeridx + 1);
                            this.inputs = outputs.ToArray();
                        }
                        else
                        {
                            this.errors = new double[this.NeuralNetwork[layeridx].Count];

                            for (int neuridx = 0; neuridx < this.NeuralNetwork[layeridx].Count; neuridx++)
                            {
                                errors[neuridx] = (0.5 * Math.Pow((expected[idx][neuridx] - outputs[neuridx]), 2));
                                //Console.WriteLine("Error {0} = {1} ( {4} * Pow({2} - {3}) )", neuridx, errors[neuridx], expected[idx][neuridx], outputs[neuridx],0.5);
                            }
                            //[((1 / 2) * (expected[idx][neuridx] - outputs[neuridx]) * *2) for neuridx in range(len(this.NeuralNetwork[layeridx]))]
                            if (c % IterStep == 0 || c == cycles-1)
                            {
                                //Console.WriteLine("Layer {0} is last. Reached errors {1} with outputs {2} should be {3}", layeridx, String.Join(";", this.errors), String.Join(";", outputs), String.Join(";", expected[idx]));
                            }
                            this.Outputs = outputs;
                        }
                    }
                    // Console.WriteLine("Epoch {1} FeedForward {0} FINISH", idx, c);
                    // Console.WriteLine("Epoch {1} BackPropag {0} START", idx, c);

                    for (int layeridx = this.NeuralNetwork.Count - 1; layeridx >= 0; layeridx--)
                    {
                        for (int neuridx = 0; neuridx < this.NeuralNetwork[layeridx].Count; neuridx++)
                        {
                            double neuroerror;
                            if (layeridx < this.NeuralNetwork.Count - 1)
                            {
                                List<double> errvect = new List<double>();
                                List<double> outvect = new List<double>();
                                List<double> weightvect = new List<double>();
                                foreach (Neuron x in this.NeuralNetwork[layeridx + 1])
                                {
                                    errvect.Add(x.error);
                                    outvect.Add(x.output * (1 - x.output));
                                    weightvect.Add(x.weights[neuridx + 1]);
                                }
                                //[x.error for x in this.NeuralNetwork[layeridx + 1]]


                                //outvect =[x.output * (1 - x.output) for x in this.NeuralNetwork[layeridx + 1]]

                                //weightvect =[x.weights[neuridx + 1] for x in this.NeuralNetwork[layeridx + 1]]
                                //Console.WriteLine("(((((((");
                                //for i in range(len(outvect)){
                                //Console.Write("{0} * {1} * {2} +",errvect[i],outvect[i],weightvect[i]);
                                //Console.WriteLine("=");
                                //Console.WriteLine("[{0}]",errvect[i] * outvect[i] * weightvect[i]);
                                //}
                                //neuroerror=numpy.dot(numpy.dot(errvect,outvect),weightvect);
                                neuroerror = Sum(Multiply(Multiply(errvect, outvect), weightvect));
                                //Console.WriteLine(" = {0})))))))",neuroerror))
                                // Console.WriteLine("Layer {0} Neuron {4} Error {5}. Outputs of layer {1} are {2}. Weights {3}",layeridx, layeridx + 1,String.Join(";",outvect), String.Join(";", weightvect), neuridx,neuroerror);
                            }
                            else
                            {
                                neuroerror = this.NeuralNetwork[layeridx][neuridx].output - expected[idx][neuridx];
                                //neuroerror = errors[neuridx];
                                //Console.WriteLine("Layer {0} is last. Neuron {1} has error {2} ({3} - {4})",layeridx,neuridx,neuroerror,this.NeuralNetwork[layeridx][neuridx].output,expected[idx][neuridx]);
                            }
                            this.NeuralNetwork[layeridx][neuridx].error = neuroerror;


                            for (int weightidx = 0; weightidx < this.NeuralNetwork[layeridx][neuridx].weights.Length; weightidx++)
                            {
                                //learner=1//this.NeuralNetwork[layeridx][neuridx].learningRate;
                                double outp = this.NeuralNetwork[layeridx][neuridx].output;
                                double inp = this.NeuralNetwork[layeridx][neuridx].inputs[weightidx];
                                double delta = neuroerror * outp * (1 - outp) * inp;
                                //Console.WriteLine("Layer {0} Neuron {1} delta {2} ({6}) =  {3} * {4} * (1- {4}) * {5}",layeridx,neuridx,weightidx,neuroerror,outp,inp,delta);
                                this.NeuralNetwork[layeridx][neuridx].weightchanges[weightidx] = delta;
                                // Console.WriteLine("Layer {0} Neuron {1} Weight {2} ({3}) with input ({4}) gets delta {5} -> {6}",layeridx,neuridx,weightidx,
                                // this.NeuralNetwork[layeridx][neuridx].weights[weightidx],
                                //this.NeuralNetwork[layeridx][neuridx].inputs[weightidx],delta,
                                //this.NeuralNetwork[layeridx][neuridx].weights[weightidx]-this.NeuralNetwork[layeridx][neuridx].learningRate*this.NeuralNetwork[layeridx][neuridx].weightchanges[weightidx]);
                                //Console.WriteLine("Layer {0} Neuron {1} Weight {2} will be {3}",layeridx,neuridx,weightidx,this.NeuralNetwork[layeridx][neuridx].weights[weightidx]-this.NeuralNetwork[layeridx][neuridx].learningRate*this.NeuralNetwork[layeridx][neuridx].weightchanges[weightidx]);
                            }
                        }
                    }
                    //Console.WriteLine("Epoch {1} BackPropag {0} FINISH",idx,c);
                    for (int layeridx = 0; layeridx < this.NeuralNetwork.Count; layeridx++)
                    {
                        foreach (Neuron neuron in this.NeuralNetwork[layeridx])
                        {
                            neuron.ApplyWeightChange();
                        }
                    }
                    List<double> finaloutputs = new List<double>();
                    foreach (Neuron x in this.NeuralNetwork[this.NeuralNetwork.Count - 1])
                    {
                        finaloutputs.Add(x.output);
                    }
                    //finaloutputs =[x.output for x in this.NeuralNetwork[len(this.NeuralNetwork) - 1]];

                    //Console.WriteLine("Epoch {0} Point {1} output {2} expected {3}. Totalerror {4}", c, String.Join(";",pointlist[idx]), String.Join(";",finaloutputs), String.Join(";",expected[idx]), Sum(this.errors));
                    //Console.WriteLine("Epoch {0} Point {1} Totalerror {2}", c, idx,  Sum(this.errors));
                    //Console.WriteLine("__________");


                    //OnTrainStep(this, null);
                    if (timeoutstep > 0)
                    {
                        System.Threading.Thread.Sleep(timeoutstep);
                    }
                    else
                    {
                        //   while (!step) { System.Threading.Thread.Sleep(200); }
                    }
                    TotalError += Sum(this.errors);

                }
                //Console.WriteLine("----");
                if (c % IterStep == 0 || c == cycles - 1)
                {
                    Console.WriteLine("Epoch {1} COMPLETE ERROR: {0}", TotalError, c);

                }

                if (Math.Round(TotalError, 3) == 0)
                {
                    Console.WriteLine("LEARNED!!");
                    break;

                }
            }
            Console.WriteLine("END");
        }

        private List<double> Multiply(List<double> vect1, List<double> vect2)
        {
            List<double> multiplied = new List<double>();
            if (vect1.Count() != vect2.Count())
            { return null; }
            for (int i = 0; i < vect2.Count(); i++)
            {
                multiplied.Add(vect1[i] * vect2[i]);
            }

            return multiplied;
        }

        private double Sum(IEnumerable<double> arr)
        {
            double sum = 0;
            foreach (double item in arr)
            {
                sum += item;
            }

            return sum;
        }

        public List<List<double>> Test(List<double[]> pointlist)
        {
            List<List<double>> finaloutputs = new List<List<double>>();
            List<double> outputs = null;
            for (int idx = 0; idx < pointlist.Count; idx++)
            {
                this.inputs = pointlist[idx];
                for (int layeridx = 0; layeridx < NeuralNetwork.Count; layeridx++)
                {

                    outputs = new List<double>();
                    foreach (Neuron neuron in this.NeuralNetwork[layeridx])
                    {
                        outputs.Add(this.GetOutput(neuron, this.inputs));
                    }
                    if (layeridx < this.NeuralNetwork.Count - 1)
                    {
                        this.inputs = outputs.ToArray();
                    }
                }
                finaloutputs.Add(outputs);
            }
            return finaloutputs;
        }
        public double GetOutput(Neuron neuron, double[] inpoint)
        { return neuron.GetOutput(inpoint); }
        [XmlArray("inputDescriptions")]
        [XmlArrayItem("inputDescription")]

        public InputDescription[] InputDescriptions
        {
            get { return inputDescriptions; }
            set
            {
                inputDescriptions = value;
                Console.WriteLine("InputDesc Assignment {0} pieces", inputDescriptions.Length);
                if (inputDescriptions.Length != inputs.Length + 1)
                {
                    inputs = new double[inputDescriptions.Length + 1];
                    originputs = new double[inputDescriptions.Length + 1];

                }
            }
        }

        public string Name { get; set; }
        public List<double> Outputs { get; set; }

        public override string ToString()
        {
            return String.Format("Layers:{0}\nLearnRate {1}\nTestset\n{2}\nTrainset\n{3}\nInputs\n{4}\nOutputs\n{5}\nDimension {6}", String.Join(";", Layer_Definition), LearningRate, String.Join(";", TestSet), String.Join(";", TrainSet), String.Join(";", (object[])InputDescriptions), String.Join(";", OutputDescriptions), this.Dimension);
        }
    }

    public class Neuron
    {
        public double error;
        public double[] weights;
        public double output;
        internal double[] inputs;
        internal double[] weightchanges;
        private  int dimension;
        private  double learningRate;
        public int Dimension { get { return dimension; } set { dimension = value; } }
        public double LearningRate { get { return learningRate; } set { learningRate = value; } }
        [XmlArray("inputDescriptions")]
        [XmlArrayItem("inputDescription")]
        private InputDescription[] inputDescriptions;
        public bool InputNeuron { get;  set; }
        public InputDescription[] InputDescriptions
        {
            get { return inputDescriptions; }
            set
            {
                inputDescriptions = value;
                //Console.WriteLine("NEURON InputDesc Assignment {0} pieces", inputDescriptions.Length);
                if (inputDescriptions.Length != inputs.Length + 1)
                {
                    inputs = new double[inputDescriptions.Length + 1];


                }
            }
        }
        public Neuron() : this(2, 0.25) { }
        public Neuron(int dimension, double learningRate)
        {
            this.dimension = dimension;
            this.learningRate = learningRate;
            this.InputNeuron = false;
            inputDescriptions = new InputDescription[this.dimension];
            this.inputs = new double[this.dimension + 1];
            this.weights = new double[this.dimension + 1];
            this.output = Double.NaN;

            this.weightchanges = new double[this.dimension + 1];
            this.error = 0;
            Random n = new Random();
            for (int i = 0; i < this.dimension + 1; i++)
            {
                this.weights[i] = (n.NextDouble());
                this.weightchanges[i] = 0;
            }




        }

        public double GetOutput(double[] inpoint)
        {
            double[] point = this.AddBiasInput(inpoint);
            this.inputs = point;
            this.output = this.EvalFunc();
            return this.output;
        }

        private double[] AddBiasInput(double[] inpoint)
        {
            List<double> biasinput = new List<double>(inpoint);


           // Console.WriteLine("NEURON {0} normalized to {1}/{2}", InputNeuron,inpoint.Length,inputDescriptions.Length);
            if (this.InputNeuron)
            {
                for (int i = 0; i < biasinput.Count; i++)
                {
                    biasinput[i] = Normalize(biasinput[i], inputDescriptions[i]);
                }
                //Console.WriteLine("{0} normalized to {1}", String.Join(";", inpoint), String.Join(";", biasinput));
            }
            biasinput.Insert(0, 1);

            return biasinput.ToArray();
        }
        private double Normalize(double num, InputDescription inputDescription)
        {

            double result = (num - inputDescription.Minimum) / (inputDescription.Maximum - inputDescription.Minimum);
            //Console.WriteLine("{0} = ({1} - {2})/({3} - {4})",result,num, inputDescription.Minimum, inputDescription.Maximum, inputDescription.Minimum);
            return result;
        }
        private double GetMax(List<double> biasinput)
        {
            double max = biasinput[0];
            foreach (double item in biasinput)
            {
                if (item > max)
                {
                    max = item;
                }
            }
            return max;

        }

        private double EvalFunc()
        {
            double suma = DotProduct(this.inputs, this.weights);
            // print("Point {0} weights {1} suma {2}".format(this.inputs,this.weights,suma))

            return Math.Exp(suma) / (1 + Math.Exp(suma));
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

        public void ApplyWeightChange()
        {
            for (int idx = 0; idx < this.weights.Length; idx++)
            {
                //Console.WriteLine("Adjust weights {0} = {1} - {2}*{3}", this.weights[idx] - this.learningRate * this.weightchanges[idx], this.weights[idx], this.learningRate, this.weightchanges[idx]);
                this.weights[idx] -= this.learningRate * this.weightchanges[idx];
            }

        }


    }
}
