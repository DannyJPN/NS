using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BackPropag
{
	public class TrainSetElement : TestSetElement
	{
		private double[] outputs;

		public TrainSetElement() : this(2)
		{

		}

		public TrainSetElement(int inputsSize) : base(inputsSize)
		{
			//super(inputsSize);
		}

		[XmlArray("outputs")]
		[XmlArrayItem("value")]
		public double[] Outputs
		{
			get { return outputs; }
			set { outputs = value; }
		}



		public TrainSetElement(double[] inputs, double[] outputs) : base(inputs)
		{

			this.outputs = outputs;
		}



		public override String ToString()
		{
			return "TrainSetElement [output=" + String.Join(";", Outputs) + ", getInputs()="
					+ String.Join(";", Inputs) + "]";
		}
	}
}
