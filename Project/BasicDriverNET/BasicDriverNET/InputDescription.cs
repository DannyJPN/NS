using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BackPropag
{
	public class InputDescription
	{

		private String name;
		private double minimum;
		private double maximum;

		[XmlElement("name")]
		public String Name
		{
			get { return name; }
			set { name = value; }
		}
		[XmlElement("minimum")]
		public double Minimum
		{
			get { return minimum; }
			set { minimum = value; }
		}
		[XmlElement("maximum")]
		public double Maximum
		{
			get { return maximum; }
			set { maximum = value; }
		}

		public InputDescription() : this("", 0, 1)
		{

		}
		public InputDescription(String name, double minimum, double maximum)
		{

			this.name = name;
			this.minimum = minimum;
			this.maximum = maximum;
		}

		public override String ToString()
		{
			return name + " (" + minimum
					+ ", " + maximum + ")";
		}
	}
}
