using BackPropag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
namespace BasicDriverNET
{
    class SimpleDriver : IDriver
    {
		public Dictionary<String, float> drive(Dictionary<String, float> values,MultiLayerNetwork net) {
			Dictionary<String, float> responses = new Dictionary<String, float>();
			List<double[]> points = new List<double[]>();
			double[] point = new double[values.Count];
			int i = 0;
			foreach (KeyValuePair<string, float> kvp in values)
			{
				point[i++] = kvp.Value;
				//Console.WriteLine("{0}={1}", kvp.Key, kvp.Value);
			}
			points.Add(point);
			List<List<double>> results = net.Test(points);


			//float distance0 = values["distance0"];
			float distance0 = (float)results[0][0];
			responses.Add("wheel", distance0);
			// pokud je v levo jede doprava, jinak do leva
			/*if (distance0 < 0.5) {
				responses.Add("wheel", 0.8f);
			} else {
				responses.Add("wheel", 0.2f);
			}*/
			// maximalni zrychleni
			//responses.Add("acc", 0.3f);

			responses.Add("acc", (float)results[0][1]);
			Console.WriteLine("Wheel {0} Acc {1}",distance0, (float)results[0][1]);
			return responses;
		}
    }
}
