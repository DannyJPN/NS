package hebbPerceptron;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Marshaller;
import javax.xml.bind.Unmarshaller;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlElementWrapper;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement
public class PerceptronTask {

	private Perceptron perceptron;
	private List<TrainSetElement> trainSet;
	private List<TestSetElement> testSet;
	
	public static void main(String[] args) {
		try {
//			pt.storeToXML(new File("obdelnik_rozsah.xml"));
			PerceptronTask pt2 = PerceptronTask.loadFromXML(new File("obdelnik_rozsah.xml"));
			System.out.println(pt2.toString());
		} catch (JAXBException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}
	}

	public void storeToXML(File file) throws JAXBException{
		JAXBContext context = JAXBContext.newInstance(PerceptronTask.class);
		Marshaller m =  context.createMarshaller();
		m.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, true);
		m.marshal(this, file);
	}
	
	public static PerceptronTask loadFromXML(File file) throws JAXBException{
		JAXBContext context = JAXBContext.newInstance(PerceptronTask.class);
		Unmarshaller m = context.createUnmarshaller();
		return (PerceptronTask)m.unmarshal(file);
	}
	

	@XmlElement
	public Perceptron getPerceptron() {
		return perceptron;
	}

	
	public void setPerceptron(Perceptron perceptron) {
		this.perceptron = perceptron;
	}

	public void setTrainSet(List<TrainSetElement> trainSet) {
		this.trainSet = trainSet;
	}

	public  void setTestSet(List<TestSetElement> testSet) {
		this.testSet = testSet;
	}

	@XmlElementWrapper(name="TrainSet")
	@XmlElement(name="element")
	public List<TrainSetElement> getTrainSet() {
		return trainSet;
	}

	@XmlElementWrapper(name="TestSet")
	@XmlElement(name="element")
	public List<TestSetElement> getTestSet() {
		return testSet;
	}

	public PerceptronTask() {
		perceptron = new Perceptron(2);
		perceptron.getInputDescriptions()[0].setMinimum(-10);
		perceptron.getInputDescriptions()[0].setMaximum(40);
		trainSet = new ArrayList<TrainSetElement>();
		trainSet.add(new TrainSetElement(new double[]{0, 0.4}, 1));
		trainSet.add(new TrainSetElement(new double[]{15, 0.1}, 0));
		trainSet.add(new TrainSetElement(new double[]{20, 0.9}, 1));
		trainSet.add(new TrainSetElement(new double[]{30, 0.6}, 0));
		testSet = new ArrayList<TestSetElement>();
		testSet.add(new TestSetElement(new double[]{15, 0.5}));
		testSet.add(new TestSetElement(new double[]{-5, 0.1}));
		testSet.add(new TestSetElement(new double[]{35, 0.9}));

	}

	@Override
	public String toString() {
		return "PerceptronTask [perceptron=" + perceptron + ", trainSet="
				+ trainSet + ", testSet=" + testSet + "]";
	}
	
	
}
