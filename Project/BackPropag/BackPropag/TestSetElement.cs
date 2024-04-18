using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BackPropag
{
	public class TestSetElement
	{
		private double[] inputs;


		public TestSetElement() : this(2)
		{

		}

		public TestSetElement(int inputsSize)
		{
			inputs = new double[inputsSize];
		}

		[XmlArray("inputs")]
		[XmlArrayItem("value")]

		public double[] Inputs
		{
			get { return inputs; }
			set { inputs = value; }
		}



		public TestSetElement(double[] inputs)
		{

			this.inputs = inputs;
		}


		public override String ToString()
		{
			return "TestSetElement [getInputs()="
					+ String.Join(";", Inputs) + "]";
		}
	}
}
