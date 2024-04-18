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

namespace BackPropag
{
    public partial class Form1 : Form
    {
        private BackPropagationTask bp;
        private PlotModel oxymodel;
        private LinearAxis xaxis;
        private LinearAxis yaxis;
        private AreaSeries area;
        private LineSeries trainingpoints0;
        private LineSeries trainingpoints1;
        private BackPropagationTask bp2;

        public Form1()
        {
            InitializeComponent();
      
        
        
        
        }

        private  double[] GenerateArray(int count)
        {
            Random random = new Random();
            double[] values = new double[count];

            for (int i = 0; i < count; ++i)
                values[i] = random.NextDouble();

            return values;
        }
        private void NUDSpeedController_ValueChanged(object sender, EventArgs e)
        {
            if (bp != null && bp.MultiLayerNetwork != null)
            {
                bp.MultiLayerNetwork.TimeOutStep = (int)((NumericUpDown)sender).Value;
            }
        }

        private void BStartTest_Click(object sender, EventArgs e)
        {
            List<double[]> points = new List<double[]>();
            foreach (TestSetElement tse in bp.MultiLayerNetwork.TestSet)
            {
                points.Add(tse.Inputs);
            }

            List<List<double>> results = bp.MultiLayerNetwork.Test(points);
            for (int i = 0; i < points.Count; i++)
            {
                Console.WriteLine("Point {0} output {1}",String.Join(";",points[i]), String.Join(";", results[i]));
            }
        }

        private void BStartLearn_Click(object sender, EventArgs e)
        {

            List<double[]> points = new List<double[]>();
            foreach (TrainSetElement tse in bp.MultiLayerNetwork.TrainSet)
            {
                points.Add(tse.Inputs);
            }
            List<List<double>> expect = new List<List<double>>();
            foreach (TrainSetElement tse in bp.MultiLayerNetwork.TrainSet)
            {
                expect.Add(tse.Outputs.ToList());
            }

            bp.MultiLayerNetwork.Train(points, expect, (int)NUDIterations.Value);
        }

        private void ChooseSet(object sender, EventArgs e)
        {
            if (OFDOpener.ShowDialog() == DialogResult.OK)
            {
                bp = new BackPropagationTask();
                    bp.LoadFromXML(OFDOpener.FileName);
                bp.MultiLayerNetwork.OnTrainStep += MultiLayerNetwork_OnTrainStep;


                Console.WriteLine(bp);

                Console.WriteLine(bp.MultiLayerNetwork);





                DGVValues.Columns.Clear();
                DGVValues.Columns.Add("Iteration", "I");
                DGVValues.Columns.Add("CurrentInput", "Cur");
                int outcount = 0;
                for (outcount = 0; outcount < bp.MultiLayerNetwork.OutputDescriptions.Length; outcount++)

                {
                    DGVValues.Columns.Add(bp.MultiLayerNetwork.OutputDescriptions[outcount], bp.MultiLayerNetwork.OutputDescriptions[outcount]);
                    Console.WriteLine("OutputDesc {0}", bp.MultiLayerNetwork.OutputDescriptions[outcount]);
                }
                


                //                foreach (InputDescribpion input in bp.MultiLayerNetwork.InputDescriptions)
                int incount;
                //foreach (InputDescribpion input in bp.MultiLayerNetwork.InputDescriptions)
                Console.WriteLine("InputDesc count {0}", bp.MultiLayerNetwork.InputDescriptions.Length);
                Console.WriteLine("Input count {0}", bp.MultiLayerNetwork.Inputs.Length);

                for (incount = 0; incount < bp.MultiLayerNetwork.InputDescriptions.Length; incount++)

                {
                    DGVValues.Columns.Add(bp.MultiLayerNetwork.InputDescriptions[incount].Name, bp.MultiLayerNetwork.InputDescriptions[incount].Name);
                    Console.WriteLine("InputDesc {0}", bp.MultiLayerNetwork.InputDescriptions[incount]);
                }
               /* int wcount = 0;
                foreach (double weight in bp.MultiLayerNetwork.Weights)
                {
                    DGVValues.Columns.Add(String.Format("Weight{0}", wcount), String.Format("W{0}", wcount));
                    wcount++;
                }
                DGVValues.AutoResizeColumns();*/

                if (bp.MultiLayerNetwork.InputDescriptions.Length - 1 == 2)
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

                   // foreach (TrainSetElement ts in bp.MultiLayerNetwork.TrainSet) { Console.WriteLine(ts); if (ts.Outputs > 0) { trainingpoints1.Points.Add(new DataPoint(ts.Inputs[0], ts.Inputs[1])); } else { trainingpoints0.Points.Add(new DataPoint(ts.Inputs[0], ts.Inputs[1])); } }
                    //foreach (TestSetElement ts in bp.MultiLayerNetwork.TestSet) { Console.WriteLine(ts); testpoints.Points.Add(new DataPoint(ts.Inputs[0], ts.Inputs[1])); }
          /*          oxymodel = new OxyPlot.PlotModel
                    {
                        Title = "Neural Learning"
                    };
                    xaxis = new LinearAxis
                    {
                        Position = AxisPosition.Bottom,
                        Minimum = GetMin(bp.MultiLayerNetwork.TrainSet, bp.MultiLayerNetwork.TestSet) - 1,
                        Maximum = GetMax(bp.MultiLayerNetwork.TrainSet, bp.MultiLayerNetwork.TestSet) + 1,
                        MajorGridlineStyle = LineStyle.Solid
                    };

                    yaxis = new LinearAxis
                    {
                        Position = AxisPosition.Left,
                        MajorGridlineStyle = LineStyle.Solid,
                        Minimum = GetMin(bp.MultiLayerNetwork.TrainSet, bp.MultiLayerNetwork.TestSet) - 1,
                        Maximum = GetMax(bp.MultiLayerNetwork.TrainSet, bp.MultiLayerNetwork.TestSet) + 1
                    };
*/
                  /* TODO line = new FunctionSeries()
                    {
                        Title = "Decision line",
                        Color = OxyPlot.OxyColors.Black,
                        StrokeThickness = 2,
                        MarkerSize = 0,

                    };*/
                    //double[] w = bp.MultiLayerNetwork.Weights;
                    //for (double x = GetMin(bp.MultiLayerNetwork.TrainSet, bp.MultiLayerNetwork.TestSet) - 2; x <= GetMax(bp.MultiLayerNetwork.TrainSet, bp.MultiLayerNetwork.TestSet) + 2; x++)
                    //{
                    //
                    //    //adding the points based x,y
                    //    double y = (-(w[0] / w[2]) / (w[0] / w[1])) * x + (-w[0] / w[2]);
                    //    DataPoint data = new DataPoint(x, y);
                    //
                    //    //adding the point to the serie
                    //    //TODO line.Points.Add(data);
                    //
                    //}

                    oxymodel.Series.Add(trainingpoints0);
                    oxymodel.Series.Add(trainingpoints1);
                    //oxymodel.Series.Add(line);
                    // oxymodel.Series.Add(testpoints);
                    oxymodel.Series.Add(area);
                    oxymodel.Axes.Add(xaxis);
                    oxymodel.Axes.Add(yaxis);

                    PVGraph.Model = oxymodel;
                    PVGraph.BackColor = Color.White;

                    UpdateGraph(true);
                }

                //
             //   int inputcount = 2;
             //   bp2 = new BackPropagationTask();
             //   bp2.MultiLayerNetwork = new MultiLayerNetwork(inputcount, 0.25, new int[] { 3,2,2}) ;
             //   bp2.MultiLayerNetwork.Name = "Net";
             //   Random n = new Random();
             //   int trainlen = n.Next(5, 10);
             //   int testlen = n.Next(5, 10);
             //
             //   for (int i = 0; i < inputcount; i++)
             //   {
             //       for (int j = 0; j < bp2.MultiLayerNetwork.Layer_Definition.Length; j++)
             //       {
             //           for (int k = 0; k < bp2.MultiLayerNetwork.Layer_Definition[j]; k++)
             //           {
             //               bp2.MultiLayerNetwork.NeuralNetwork[j][k].weights[i] = n.NextDouble();
             //               bp2.MultiLayerNetwork.InputDescriptions[i].Maximum *= 1.1;
             //           }
             //       }
             //       
             //
             //
             //   }
             //   bp2.MultiLayerNetwork.TrainSet = new List<TrainSetElement>();
             //   bp2.MultiLayerNetwork.TrainSet.Add(new TrainSetElement(new double[] { 0, 0.4 }, GenerateArray(bp2.MultiLayerNetwork.Layer_Definition[bp2.MultiLayerNetwork.Layer_Definition.Length-1])));
             //   bp2.MultiLayerNetwork.TrainSet.Add(new TrainSetElement(new double[] { 15, 0.1 }, GenerateArray(bp2.MultiLayerNetwork.Layer_Definition[bp2.MultiLayerNetwork.Layer_Definition.Length - 1])));
             //   bp2.MultiLayerNetwork.TrainSet.Add(new TrainSetElement(new double[] { 20, 0.9 }, GenerateArray(bp2.MultiLayerNetwork.Layer_Definition[bp2.MultiLayerNetwork.Layer_Definition.Length - 1])));
             //   bp2.MultiLayerNetwork.TrainSet.Add(new TrainSetElement(new double[] { 30, 0.6 }, GenerateArray(bp2.MultiLayerNetwork.Layer_Definition[bp2.MultiLayerNetwork.Layer_Definition.Length - 1])));
             //   bp2.MultiLayerNetwork.TestSet = new List<TestSetElement>();
             //   bp2.MultiLayerNetwork.TestSet.Add(new TestSetElement(new double[] { 15, 0.5 }));
             //   bp2.MultiLayerNetwork.TestSet.Add(new TestSetElement(new double[] { -5, 0.1 }));
             //   bp2.MultiLayerNetwork.TestSet.Add(new TestSetElement(new double[] { 35, 0.9 }));
             //   double min = -20, max = 20;
             //
             //   for (int i = 0; i < trainlen; i++)
             //   {
             //       bp2.MultiLayerNetwork.TrainSet.Add(new TrainSetElement(new double[] { n.NextDouble() * (max - min) + min, n.NextDouble() * (max - min) + min }, GenerateArray(bp2.MultiLayerNetwork.Layer_Definition[bp2.MultiLayerNetwork.Layer_Definition.Length - 1])));
             //   }
             //   for (int i = 0; i < testlen; i++)
             //   {
             //       bp2.MultiLayerNetwork.TestSet.Add(new TestSetElement(new double[] { n.NextDouble() * (max - min) + min, n.NextDouble() * (max - min) + min }));
             //   }
             //
             //
             //
             //
             //   bp2.StoreToXML("Export.xml");

                BStartLearn.Enabled = BStartTest.Enabled = true;
                
            
                UpdateGrid();


            }
        }

        private void MultiLayerNetwork_OnTrainStep(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        private void UpdateGraph(bool v)
        {
            
        }



        private void UpdateGrid()
        {
            //DGVValues.Rows[0].Cells["Output"].Value =bp.MultiLayerNetwork.Output;



            int outcount = 0;
            for (outcount = 0; outcount < bp.MultiLayerNetwork.Outputs.Count; outcount++)

            {
                DGVValues.Rows[0].Cells[bp.MultiLayerNetwork.OutputDescriptions[outcount]].Value = bp.MultiLayerNetwork.Outputs[outcount];
           
            }

            DGVValues.Rows[0].Cells["Iteration"].Value = bp.MultiLayerNetwork.CurrentIter;
            DGVValues.Rows[0].Cells["CurrentInput"].Value = bp.MultiLayerNetwork.CurrentInputIndex;
            int incount;
            //foreach (InputDescription input in bp.MultiLayerNetwork.InputDescriptions)
            Console.WriteLine("InputDesc  {0} Inputs {1}", bp.MultiLayerNetwork.InputDescriptions.Length, bp.MultiLayerNetwork.OrigInputs.Length);
            for (incount = 0; incount < bp.MultiLayerNetwork.InputDescriptions.Length; incount++)
            {
                DGVValues.Rows[0].Cells[bp.MultiLayerNetwork.InputDescriptions[incount].Name].Value = Math.Round(bp.MultiLayerNetwork.OrigInputs[incount ], 4);
            }
         
            DGVValues.Update();
            DGVValues.Refresh();

        }

        private void PVGraph_Click(object sender, EventArgs e)
        {

        }
    }
}
