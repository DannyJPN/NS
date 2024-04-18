using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Perceptron
{




    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BStartLearn.Enabled = false;
            BStartTest.Enabled = false;

        }
        private PerceptronTask pt;
        private AreaSeries area;
        private LineSeries trainingpoints0;
        private LineSeries trainingpoints1;
        private LineSeries testpoints0;

        //  private LineSeries testpoints;
        private PlotModel oxymodel;
        private LinearAxis xaxis;
        private LinearAxis yaxis;
        private FunctionSeries line;
        private PerceptronTask pt2;
        private LineSeries testpoints1;
        private List<double> outputs;
        private void ChooseSet(object sender, EventArgs e)
        {
            if (OFDOpener.ShowDialog() == DialogResult.OK)
            {
                pt = PerceptronTask.LoadFromXML(OFDOpener.FileName);
                pt.Perceptron.OnTrainStep += Perceptron_OnTrainStep;


                Console.WriteLine(pt);

                Console.WriteLine(pt.Perceptron);





                DGVValues.Columns.Clear();
                DGVValues.Columns.Add("Iteration", "I");
                DGVValues.Columns.Add("CurrentInput", "Cur");

                DGVValues.Columns.Add("Output", "Y");
                //                foreach (InputDescription input in pt.Perceptron.InputDescriptions)
                int incount;
                //foreach (InputDescription input in pt.Perceptron.InputDescriptions)
                for (incount = 0; incount < pt.Perceptron.InputDescriptions.Length; incount++)

                {
                    DGVValues.Columns.Add(pt.Perceptron.InputDescriptions[incount].Name, pt.Perceptron.InputDescriptions[incount].Name);
                }
                int wcount = 0;
                foreach (double weight in pt.Perceptron.Weights)
                {
                    DGVValues.Columns.Add(String.Format("Weight{0}", wcount), String.Format("W{0}", wcount));
                    wcount++;
                }
                DGVValues.AutoResizeColumns();

                if (pt.Perceptron.Weights.Length - 1 == 2)
                {

                    area = new AreaSeries()
                    {
                        Title = "grid",
                        Color = OxyPlot.OxyColors.Violet,
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerType = OxyPlot.MarkerType.Circle
                    };
                    trainingpoints0 = new OxyPlot.Series.LineSeries()
                    {
                        Title = "training points",
                        Color = OxyPlot.OxyColors.Green,
                        StrokeThickness = 0,
                        MarkerSize = 3,
                        MarkerType = OxyPlot.MarkerType.Circle
                    };

                    trainingpoints1 = new OxyPlot.Series.LineSeries()
                    {
                        Title = "training points",
                        Color = OxyPlot.OxyColors.Blue,
                        StrokeThickness = 0,
                        MarkerSize = 3,
                        MarkerType = OxyPlot.MarkerType.Circle
                    };

                    /* testpoints0 = new OxyPlot.Series.LineSeries()
                     {
                         Title = "test points",
                         Color = OxyPlot.OxyColors.Red,
                         StrokeThickness = 0,
                         MarkerSize = 3,
                         MarkerType = OxyPlot.MarkerType.Square
                     };
                     testpoints1 = new OxyPlot.Series.LineSeries()
                     {
                         Title = "test points",
                         Color = OxyPlot.OxyColors.Red,
                         StrokeThickness = 0,
                         MarkerSize = 3,
                         MarkerType = OxyPlot.MarkerType.Square
                     };*/

                    foreach (TrainSetElement ts in pt.TrainSet) { Console.WriteLine(ts); if (ts.Output > 0) { trainingpoints1.Points.Add(new DataPoint(ts.Inputs[0], ts.Inputs[1])); } else { trainingpoints0.Points.Add(new DataPoint(ts.Inputs[0], ts.Inputs[1])); } }
                    //foreach (TestSetElement ts in pt.TestSet) { Console.WriteLine(ts); testpoints.Points.Add(new DataPoint(ts.Inputs[0], ts.Inputs[1])); }
                    oxymodel = new OxyPlot.PlotModel
                    {
                        Title = "Neural Learning"
                    };
                    xaxis = new LinearAxis
                    {
                        Position = AxisPosition.Bottom,
                        Minimum = GetMin(pt.TrainSet, pt.TestSet) - 1,
                        Maximum = GetMax(pt.TrainSet, pt.TestSet) + 1,
                        MajorGridlineStyle = LineStyle.Solid
                    };

                    yaxis = new LinearAxis
                    {
                        Position = AxisPosition.Left,
                        MajorGridlineStyle = LineStyle.Solid,
                        Minimum = GetMin(pt.TrainSet, pt.TestSet) - 1,
                        Maximum = GetMax(pt.TrainSet, pt.TestSet) + 1
                    };

                    line = new FunctionSeries()
                    {
                        Title = "Decision line",
                        Color = OxyPlot.OxyColors.Black,
                        StrokeThickness = 2,
                        MarkerSize = 0,

                    };
                    double[] w = pt.Perceptron.Weights;
                    for (double x = GetMin(pt.TrainSet, pt.TestSet) - 2; x <= GetMax(pt.TrainSet, pt.TestSet) + 2; x++)
                    {

                        //adding the points based x,y
                        double y = (-(w[0] / w[2]) / (w[0] / w[1])) * x + (-w[0] / w[2]);
                        DataPoint data = new DataPoint(x, y);

                        //adding the point to the serie
                        line.Points.Add(data);

                    }

                    oxymodel.Series.Add(trainingpoints0);
                    oxymodel.Series.Add(trainingpoints1);
                    oxymodel.Series.Add(line);
                    // oxymodel.Series.Add(testpoints);
                    oxymodel.Series.Add(area);
                    oxymodel.Axes.Add(xaxis);
                    oxymodel.Axes.Add(yaxis);

                    PVGraph.Model = oxymodel;
                    PVGraph.BackColor = Color.White;

                    UpdateGraph(true);
                }

                //
                int inputcount = 2;
                pt2 = new PerceptronTask();
                pt2.Perceptron = new Perceptron(inputcount, 0.25);
                pt2.Perceptron.Name = "FirstNeuron";
                Random n = new Random();
                int trainlen = n.Next(5, 10);
                int testlen = n.Next(5, 10);

                for (int i = 0; i < inputcount; i++)
                {
                    pt2.Perceptron.Weights[i] = n.NextDouble();
                    pt2.Perceptron.InputDescriptions[i].Maximum *= 1.1;


                }
                pt2.TrainSet = new List<TrainSetElement>();
                pt2.TrainSet.Add(new TrainSetElement(new double[] { 0, 0.4 }, 1));
                pt2.TrainSet.Add(new TrainSetElement(new double[] { 15, 0.1 }, 0));
                pt2.TrainSet.Add(new TrainSetElement(new double[] { 20, 0.9 }, 1));
                pt2.TrainSet.Add(new TrainSetElement(new double[] { 30, 0.6 }, 0));
                pt2.TestSet = new List<TestSetElement>();
                pt2.TestSet.Add(new TestSetElement(new double[] { 15, 0.5 }));
                pt2.TestSet.Add(new TestSetElement(new double[] { -5, 0.1 }));
                pt2.TestSet.Add(new TestSetElement(new double[] { 35, 0.9 }));
                double min = -20, max = 20;

                for (int i = 0; i < trainlen; i++)
                {
                    pt2.TrainSet.Add(new TrainSetElement(new double[] { n.NextDouble() * (max - min) + min, n.NextDouble() * (max - min) + min }, Math.Round(n.NextDouble())));
                }
                for (int i = 0; i < testlen; i++)
                {
                    pt2.TestSet.Add(new TestSetElement(new double[] { n.NextDouble() * (max - min) + min, n.NextDouble() * (max - min) + min }));
                }




                pt2.StoreToXML("Export.xml");

                BStartLearn.Enabled = BStartTest.Enabled = true;
                pt.Perceptron.TimeOutStep = (int)NUDSpeedController.Value;
                pt.Perceptron.Step = false;
                UpdateGrid();


            }
        }

        private double GetMax(List<TrainSetElement> trainSet, List<TestSetElement> testSet)
        {
            double max = trainSet[0].Inputs[0];
            foreach (TrainSetElement ts in trainSet)
            {
                foreach (double input in ts.Inputs)
                {
                    max = max > input ? max : input;
                }

            }
            foreach (TestSetElement ts in testSet)
            {
                foreach (double input in ts.Inputs)
                {
                    max = max > input ? max : input;
                }

            }
            return max;
        }

        private double GetMin(List<TrainSetElement> trainSet, List<TestSetElement> testSet)
        {
            double min = trainSet[0].Inputs[0];
            foreach (TrainSetElement ts in trainSet)
            {
                foreach (double input in ts.Inputs)
                {
                    min = min < input ? min : input;
                }

            }
            foreach (TestSetElement ts in testSet)
            {
                foreach (double input in ts.Inputs)
                {
                    min = min < input ? min : input;
                }

            }


            return min;
        }
        /*
                private double GetMin(List<TrainSetElement> trainSet, InputDescription[] inputDescriptions)
                {
                    double min = inputDescriptions[0].Minimum;
                    foreach (InputDescription inputdesc in inputDescriptions)
                    {
                        min = min < inputdesc.Minimum ? min : inputdesc.Minimum;
                    }
                    return min;
                }
                */
        /*
                private double GetMax(List<TrainSetElement> trainSet, InputDescription[] inputDescriptions)
                {
                    double max = inputDescriptions[0].Maximum;
                    foreach (InputDescription inputdesc in inputDescriptions)
                    {
                        max = max > inputdesc.Maximum ? max : inputdesc.Maximum;
                    }
                    return max;
                }
        */
        private void Perceptron_OnTrainStep(object sender, EventArgs e)
        {
            UpdateGrid();
            if (pt.Perceptron.Weights.Length - 1 == 2)
                UpdateGraph(true);
        }

        private void UpdateGrid()
        {
            DGVValues.Rows[0].Cells["Output"].Value = pt.Perceptron.Output;
            DGVValues.Rows[0].Cells["Iteration"].Value = pt.Perceptron.CurrentIter;
            DGVValues.Rows[0].Cells["CurrentInput"].Value = pt.Perceptron.CurrentInputIndex;
            int incount;
            //foreach (InputDescription input in pt.Perceptron.InputDescriptions)
            for (incount = 0; incount < pt.Perceptron.InputDescriptions.Length; incount++)
            {
                DGVValues.Rows[0].Cells[pt.Perceptron.InputDescriptions[incount].Name].Value = Math.Round(pt.Perceptron.Inputs[incount + 1], 4);
            }
            int wcount = 0;
            foreach (double weight in pt.Perceptron.Weights)
            {
                DGVValues.Rows[0].Cells[String.Format("Weight{0}", wcount)].Value = Math.Round(pt.Perceptron.Weights[wcount], 4);
                wcount++;
            }
            DGVValues.Update();
            DGVValues.Refresh();

        }

        private void UpdateGraph(bool training)
        {
            area = new AreaSeries()
            {
                Title = "grid",
                Color = OxyPlot.OxyColors.Violet,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerType = OxyPlot.MarkerType.Circle
            };
            trainingpoints0 = new OxyPlot.Series.LineSeries()
            {
                Title = "training points0",
                Color = OxyPlot.OxyColors.Green,
                StrokeThickness = 0,
                MarkerSize = 3,
                MarkerType = OxyPlot.MarkerType.Circle
            };

            trainingpoints1 = new OxyPlot.Series.LineSeries()
            {
                Title = "training points1",
                Color = OxyPlot.OxyColors.Blue,
                StrokeThickness = 0,
                MarkerSize = 3,
                MarkerType = OxyPlot.MarkerType.Circle
            };

            testpoints0 = new OxyPlot.Series.LineSeries()
            {
                Title = "test points0",
                Color = OxyPlot.OxyColors.Green,
                StrokeThickness = 0,
                MarkerSize = 3,
                MarkerType = OxyPlot.MarkerType.Square
            };
            testpoints1 = new OxyPlot.Series.LineSeries()
            {
                Title = "test points1",
                Color = OxyPlot.OxyColors.Blue,
                StrokeThickness = 0,
                MarkerSize = 3,
                MarkerType = OxyPlot.MarkerType.Square
            };
            foreach (TrainSetElement ts in pt.TrainSet) { /*Console.WriteLine(ts);*/ if (ts.Output > 0) { trainingpoints1.Points.Add(new DataPoint(ts.Inputs[0], ts.Inputs[1])); } else { trainingpoints0.Points.Add(new DataPoint(ts.Inputs[0], ts.Inputs[1])); } }
            if (!training)
            {
                for (int t = 0; t < pt.TestSet.Count; t++)
                {
                    TestSetElement ts = pt.TestSet[t];
                    /*Console.WriteLine(ts);*/
                    if (outputs[t] > 0)
                    { testpoints1.Points.Add(new DataPoint(ts.Inputs[0], ts.Inputs[1])); }
                    else
                    { testpoints0.Points.Add(new DataPoint(ts.Inputs[0], ts.Inputs[1])); }
                }

            }


            xaxis = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = GetMin(pt.TrainSet, pt.TestSet) - 1,
                Maximum = GetMax(pt.TrainSet, pt.TestSet) + 1,
                MajorGridlineStyle = LineStyle.Solid
            };

            yaxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                Minimum = GetMin(pt.TrainSet, pt.TestSet) - 1,
                Maximum = GetMax(pt.TrainSet, pt.TestSet) + 1
            };



            line = new FunctionSeries()
            {
                Title = String.Format("Decision line step {0}", pt.Perceptron.CurrentIter),
                Color = OxyPlot.OxyColors.Black,
                StrokeThickness = 2,
                MarkerSize = 0

            };

            FunctionSeries testline = new FunctionSeries()
            {
                Title = String.Format("Test Decision line step {0}", pt.Perceptron.CurrentIter),
                Color = OxyPlot.OxyColors.Violet,
                StrokeThickness = 2,
                MarkerSize = 0

            };

            double[] w = pt.Perceptron.Weights;
            //  double[] wb = new double[] {0,1,-1 };

            for (double x = GetMin(pt.TrainSet, pt.TestSet) - 2; x <= GetMax(pt.TrainSet, pt.TestSet) + 2; x++)
            {


                // wb[0] = w[1];
                // wb[1] = w[2];
                //wb[2] = w[0];
                //adding the points based x,y
                //double y = (-(w[0] / w[2]) / (w[0] / w[1])) * x + (-w[0] / w[2]);
                //  double testy = (-(wb[0] / wb[2]) / (wb[0] / wb[1])) * x + (-wb[0] / wb[2]);
                double y = -(w[1] / w[2]) * x + (-w[0] / w[2]);
                // double testy = -(wb[1] / wb[2]) * x + (-wb[0] / wb[2]);


                //double y = (-w[2] * x / w[1]) - (w[0] / w[1]);
                //double y = ((-w[1] * x) / w[2]) - (w[0] / w[2]);
                DataPoint data = new DataPoint(x, y);

                //adding the point to the serie
                line.Points.Add(data);

                // Console.WriteLine("{0} accepted {1},{2}",line.Title,x,y);

            }
            oxymodel = new PlotModel()
            {
                Title = "Neural Learning"
            };
            oxymodel.Series.Add(trainingpoints0);
            oxymodel.Series.Add(trainingpoints1);

            oxymodel.Series.Add(testpoints0);
            oxymodel.Series.Add(testpoints1);

            oxymodel.Series.Add(area);
            oxymodel.Axes.Add(xaxis);
            oxymodel.Axes.Add(yaxis);

            oxymodel.Series.Add(line);


            PVGraph.Model = oxymodel;
            PVGraph.InvalidatePlot(true);
            // System.Threading.Thread.Sleep(100);
            //PVGraph.Invalidate();
            PVGraph.Update();
            //PVGraph.Refresh();

        }

        private void BStartLearn_Click(object sender, EventArgs e)
        {
            pt.Perceptron.Train(pt.TrainSet, (int)NUDIterations.Value);
            // Bstep.Enabled = true;
        }

        private void NUDSpeedController_ValueChanged(object sender, EventArgs e)
        {
            if (pt != null && pt.Perceptron != null)
            {
                pt.Perceptron.TimeOutStep = (int)((NumericUpDown)sender).Value;
            }

        }

        private void BStartTest_Click(object sender, EventArgs e)
        {
            outputs = pt.Perceptron.Test(pt.TestSet);
            UpdateGraph(false);
        }
    }
}
