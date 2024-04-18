using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Perceptron
{
   public class TrainSetElement : TestSetElement
    {
		private double output;

		public TrainSetElement(): this(2)
		{
			
		}

		public TrainSetElement(int inputsSize):base(inputsSize)
		{
			//super(inputsSize);
		}

		[XmlElement("output")]
		public double Output
		{
			get{ return output; }
            set{ output = value; }
		}

		

		public TrainSetElement(double[] inputs, double output):base(inputs)
		{
			
			this.output = output;
		}

		public int getIntOutput()
		{
			return (int)Math.Round(output);
		}

		
	public override String ToString()
		{
			return "TrainSetElement [output=" + output + ", getInputs()="
					+ String.Join(";",Inputs) + "]";
		}
	}
}
