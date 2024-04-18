package hebbPerceptron;

import java.util.Arrays;

import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlElementWrapper;

public class Perceptron {

	private double[] weights;
	private double[] inputs;
	private InputDescription[] inputDescriptions;
	private double output;
	private double lerningRate = 0.3;
	private String name = "y";
	
	
	public Perceptron() {
		this(2);
	}

	public Perceptron(int numberOfInputs){
		weights = new double[numberOfInputs+1];
		inputs = new double[numberOfInputs+1];
		inputDescriptions = new InputDescription[numberOfInputs];
		for (int i = 0; i < inputDescriptions.length; i++) {
			inputDescriptions[i] = new InputDescription("Input " + i, 0, 1); 
		}
	}
	
	public String getName() {
		return name;
	}


	public void setName(String name) {
		this.name = name;
	}


	@XmlElementWrapper(name="weights")
	@XmlElement(name="weight")
	public double[] getWeights() {
		return weights;
	}

	public void setWeights(double[] weights) {
		this.weights = weights;
	}

	public double getOutput() {
		return output;
	}

	
	public InputDescription[] getInputDescriptions() {
		return inputDescriptions;
	}

	public void setInputDescriptions(InputDescription[] inputDescriptions) {
		this.inputDescriptions = inputDescriptions;
		if(inputDescriptions.length != inputs.length+1){
			inputs = new double[inputDescriptions.length+1];
		}
	}
	
	public double getLerningRate() {
		return lerningRate;
	}

	public void setLerningRate(double lerningRate) {
		this.lerningRate = lerningRate;
	}

	@Override
	public String toString() {
		return "Perceptron [weights=" + Arrays.toString(weights) + ", inputs="
				+ Arrays.toString(inputs) + ", inputDescriptions="
				+ Arrays.toString(inputDescriptions) + ", output=" + output
				+ ", lerningRate=" + lerningRate + ", name=" + name + "]";
	}

}
