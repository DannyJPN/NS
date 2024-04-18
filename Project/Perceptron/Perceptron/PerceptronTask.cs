using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Perceptron
{
	[XmlRoot("perceptronTask")]
    public class PerceptronTask
    {


		private Perceptron perceptron;
		private List<TrainSetElement> trainSet;
		private List<TestSetElement> testSet;

		/*public static void main(String[] args)
		{
			try
			{
				PerceptronTask pt2 = PerceptronTask.LoadFromXML("obdelnik_rozsah.xml");
				pt2.StoreToXML("obdelnik_rozsah.xml");
				
				Console.WriteLine(pt2.ToString());
			}
			catch (Exception e1)
			{
				// TODO Auto-generated catch block
				Console.WriteLine(e1.StackTrace);
			}
		}*/

		public void StoreToXML(string filename)
		{/*
			JAXBContext context = JAXBContext.newInstance(PerceptronTask.class);
		Marshaller m = context.createMarshaller();
		m.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, true);
		m.marshal(this, file);*/

			XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());

			using (StreamWriter writer = new StreamWriter(filename))
			{
				xmlSerializer.Serialize(writer, this);
			}

		}

		public static PerceptronTask LoadFromXML(string filename) 
		{/*
			JAXBContext context = JAXBContext.newInstance(PerceptronTask.class);
		Unmarshaller m = context.createUnmarshaller();
		return (PerceptronTask) m.unmarshal(file);*/

			System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(PerceptronTask));

			using (StreamReader sr = new StreamReader(filename))
			{
				return (PerceptronTask)ser.Deserialize(sr);
			}

			
		}


	[XmlElement("perceptron")]
	public Perceptron Perceptron
	{
		get {return perceptron;}
			set { perceptron = value; }
	}


	




	[XmlArray("TrainSet")]
	[XmlArrayItem("element")]
	public List<TrainSetElement> TrainSet
	{
			get { return trainSet; }
			set { trainSet = value; }
	}

		[XmlArray("TestSet")]
		[XmlArrayItem("element")]
		public List<TestSetElement> TestSet
	{
			get { return testSet; }
			set { testSet = value; }
	}

	public PerceptronTask()
	{
		/*perceptron = new Perceptron(2,0.3);
		perceptron.InputDescriptions[0].Minimum=-10;
		perceptron.InputDescriptions[0].Maximum=40;
		trainSet = new List<TrainSetElement>();
		trainSet.Add(new TrainSetElement(new double[] { 0, 0.4 }, 1));
		trainSet.Add(new TrainSetElement(new double[] { 15, 0.1 }, 0));
		trainSet.Add(new TrainSetElement(new double[] { 20, 0.9 }, 1));
		trainSet.Add(new TrainSetElement(new double[] { 30, 0.6 }, 0));
		testSet = new List<TestSetElement>();
		testSet.Add(new TestSetElement(new double[] { 15, 0.5 }));
		testSet.Add(new TestSetElement(new double[] { -5, 0.1 }));
		testSet.Add(new TestSetElement(new double[] { 35, 0.9 }));
			*/
	}

	
	public override String ToString()
	{
		return "PerceptronTask [perceptron=" + perceptron + ", trainSet="
				+ trainSet + ", testSet=" + testSet + "]";
	}

	}
}
