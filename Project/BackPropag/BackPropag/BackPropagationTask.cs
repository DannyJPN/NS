using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BackPropag
{
    
    class BackPropagationTask
    {


		private MultiLayerNetwork network;
		


		/*public static void main(String[] args)
		{
			try
			{
				BackPropagationTask pt2 = BackPropagationTask.LoadFromXML("obdelnik_rozsah.xml");
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
			JAXBContext context = JAXBContext.newInstance(BackPropagationTask.class);
		Marshaller m = context.createMarshaller();
		m.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, true);
		m.marshal(this, file);*/

			XmlSerializer xmlSerializer = new XmlSerializer(this.MultiLayerNetwork.GetType());

			using (StreamWriter writer = new StreamWriter(filename))
			{
				xmlSerializer.Serialize(writer, this.MultiLayerNetwork);
			}

		}

		public void LoadFromXML(string filename)
		{/*
			JAXBContext context = JAXBContext.newInstance(BackPropagationTask.class);
		Unmarshaller m = context.createUnmarshaller();
		return (BackPropagationTask) m.unmarshal(file);*/

			System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(MultiLayerNetwork));

			using (StreamReader sr = new StreamReader(filename))
			{
				MultiLayerNetwork= (MultiLayerNetwork)ser.Deserialize(sr);
			}


		}


		
		public MultiLayerNetwork MultiLayerNetwork
		{
			get { return network; }
			set { network = value; }
		}

      

        public BackPropagationTask()
		{
			/*network = new MultiLayerNetwork(2,0.3);
			network.InputDescriptions[0].Minimum=-10;
			network.InputDescriptions[0].Maximum=40;
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

		/*
		public override String ToString()
		{
			return "BackPropagationTask [network=" + network + ", trainSet="
					+ trainSet + ", testSet=" + testSet + "]";
		}*/

	}
}
