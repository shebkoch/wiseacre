using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
public static class SaveLoad
{
	public static NeuralNetwork neuralNetwork;
	public static string filename = "/neural.net";
	public static DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath);
	public static void Save(NeuralNetwork net) {
		neuralNetwork = net;
		BinaryFormatter bf = new BinaryFormatter();
		using (FileStream file = File.Create(directory + filename)) {
			bf.Serialize(file, neuralNetwork);
		}
	}
	public static NeuralNetwork Load() {
		if (File.Exists(directory + filename)) {
			BinaryFormatter bf = new BinaryFormatter();
			using (FileStream file = File.Open(directory + filename, FileMode.Open)) {
				neuralNetwork = (NeuralNetwork)bf.Deserialize(file);
			}
			return neuralNetwork;
		}
		throw new FileNotFoundException();
	}
	public static bool IsSaveExist() {
		return File.Exists(directory + filename);
	}
	public static void DeleteFile() {
		if (File.Exists(directory + filename)) {
			using (FileStream file = File.Open(directory + filename, FileMode.Create)) {
				
			}
		}
	}
}
/// <summary>
/// Neural Network C# (Unsupervised)
/// </summary>
///
[System.Serializable]
public class NeuralNetwork
{
	private int[] layers; //layers
	private float[][] neurons; //neuron matix
	private float[][][] weights; //weight matrix
	private float fitness; //fitness of the network


	/// <summary>
	/// Initilizes and neural network with random weights
	/// </summary>
	/// <param name="layers">layers to the neural network</param>
	NeuralNetwork loaded;
	public NeuralNetwork(int[] layers) {
		try {
			loaded = SaveLoad.Load();
			this.layers = new int[loaded.layers.Length];
			for (int i = 0; i < loaded.layers.Length; i++) {
				this.layers[i] = loaded.layers[i];
			}
			InitNeurons();
			InitWeights();
			CopyWeights(loaded.weights);
		}
		catch (Exception e ) {
			Debug.Log("load failed " + e.Message);
			this.layers = new int[layers.Length];
			for (int i = 0; i < layers.Length; i++) {
				this.layers[i] = layers[i];
			}
			InitNeurons();
			InitWeights();
			SaveLoad.Save(this);
		} 
		
	}
	public void Save() {
		SaveLoad.Save(this);
	}
	
	

	/// <summary>
	/// Deep copy constructor 
	/// </summary>
	/// <param name="copyNetwork">Network to deep copy</param>
	public NeuralNetwork(NeuralNetwork copyNetwork) {
		this.layers = new int[copyNetwork.layers.Length];
		for (int i = 0; i < copyNetwork.layers.Length; i++) {
			this.layers[i] = copyNetwork.layers[i];
		}

		InitNeurons();
		InitWeights();
		CopyWeights(copyNetwork.weights);
	}

	private void CopyWeights(float[][][] copyWeights) {
		for (int i = 0; i < weights.Length; i++) {
			for (int j = 0; j < weights[i].Length; j++) {
				for (int k = 0; k < weights[i][j].Length; k++) {
					weights[i][j][k] = copyWeights[i][j][k];
				}
			}
		}
	}

	/// <summary>
	/// Create neuron matrix
	/// </summary>
	private void InitNeurons() {
		//Neuron Initilization
		List<float[]> neuronsList = new List<float[]>();

		for (int i = 0; i < layers.Length; i++) //run through all layers
		{
			neuronsList.Add(new float[layers[i]]); //add layer to neuron list
		}

		neurons = neuronsList.ToArray(); //convert list to array
	}

	/// <summary>
	/// Create weights matrix.
	/// </summary>
	private void InitWeights() {

		List<float[][]> weightsList = new List<float[][]>(); //weights list which will later will converted into a weights 3D array

		//itterate over all neurons that have a weight connection
		for (int i = 1; i < layers.Length; i++) {
			List<float[]> layerWeightsList = new List<float[]>(); //layer weight list for this current layer (will be converted to 2D array)

			int neuronsInPreviousLayer = layers[i - 1];

			//itterate over all neurons in this current layer
			for (int j = 0; j < neurons[i].Length; j++) {
				float[] neuronWeights = new float[neuronsInPreviousLayer]; //neruons weights

				//itterate over all neurons in the previous layer and set the weights randomly between 0.5f and -0.5
				for (int k = 0; k < neuronsInPreviousLayer; k++) {
					//give random weights to neuron weights
					neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
				}

				layerWeightsList.Add(neuronWeights); //add neuron weights of this current layer to layer weights
			}

			weightsList.Add(layerWeightsList.ToArray()); //add this layers weights converted into 2D array into weights list
		}

		weights = weightsList.ToArray(); //convert to 3D array
	}

	/// <summary>
	/// Feed forward this neural network with a given input array
	/// </summary>
	/// <param name="inputs">Inputs to network</param>
	/// <returns></returns>
	public float[] GetAnswer(float[] inputs) {
		//Add inputs to the neuron matrix
		Mutate();
		for (int i = 0; i < inputs.Length; i++) {
			neurons[0][i] = inputs[i];
		}

		//itterate over all neurons and compute feedforward values 
		for (int i = 1; i < layers.Length; i++) {
			for (int j = 0; j < neurons[i].Length; j++) {
				float value = 0f;
				for (int k = 0; k < neurons[i - 1].Length; k++) {
					value += weights[i - 1][j][k] * neurons[i - 1][k]; //sum off all weights connections of this neuron weight their values in previous layer
				}
			
				neurons[i][j] = (float)Math.Tanh(value); //Hyperbolic tangent activation
			}
		}

		return neurons[neurons.Length - 1]; //return output layer
	}
	private void Mutate() {
		for (int i = 0; i < weights.Length; i++) {
			for (int j = 0; j < weights[i].Length; j++) {
				for (int k = 0; k < weights[i][j].Length; k++) {
					float weight = weights[i][j][k];

					//mutate weight value 
					float randomNumber = UnityEngine.Random.Range(0f, 100f);

					if (randomNumber <= 2f) { //if 1
											  //flip sign of weight
						weight *= -1f;
					} else if (randomNumber <= 4f) { //if 2
													 //pick random weight between -1 and 1
						weight = UnityEngine.Random.Range(-0.5f, 0.5f);
					} else if (randomNumber <= 6f) { //if 3
													 //randomly increase by 0% to 100%
						float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
						weight *= factor;
					} else if (randomNumber <= 8f) { //if 4
													 //randomly decrease by 0% to 100%
						float factor = UnityEngine.Random.Range(0f, 1f);
						weight *= factor;
					}

					weights[i][j][k] = weight;
				}
			}
		}
	}
	public void Study(float[] input, float factor) {
		for (int i = 1; i < layers.Length; i++) {
			for (int j = 0; j < neurons[i].Length; j++) {
				for (int k = 0; k < neurons[i-1].Length; k++) {
					weights[i - 1][j][k] *= factor;
				}
			}
		}
	}
}



























//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;
//[System.Serializable]
//public class Neuron
//{
//	public int[] Weight { get; set; }
//	public int Handle( int[] input) {
//		var power = 0;
//		for (int i = 0; i < Weight.GetLength(0); i++) {
//			power += Weight[i] * input[i];
//		}
//		return power;
//	}
//	public void Study(int[] input, int factor) {
//		for (int i = 0; i < Weight.GetLength(0); i++) {
//			Weight[i] += factor * input[i];
//		}
//	}
//	public void InitWeight(int length) {
//		Weight = new int[length];
//		for (int i = 0; i < length; i++) {
//			Weight[i] = Random.Range(0,100);
//		}
//	}
//}
//
//[System.Serializable]
//public class NeuralNetwork {
//	public Neuron[] Neurons;
//	public NeuralNetwork(int neuronCount, int weightLength) {
//		if (SaveLoad.isSaveExist()) {
//			NeuralNetwork loaded = SaveLoad.Load();
//			Neurons = loaded.Neurons;
//		}
//		if (Neurons[0] == null) {
//			Neurons = new Neuron[neuronCount];
//			for (int i = 0; i < Neurons.Length; i++) {
//				Neurons[i] = new Neuron();
//				Neurons[i].InitWeight(weightLength);
//			}
//		}
//	}
//	~NeuralNetwork() {
//		SaveLoad.Save(this);
//	}
//	public int getAnswer(int[] input) {
//		var answer = new int[Neurons.Length];
//		for (int i = 0; i < Neurons.Length; i++) {
//			answer[i] = Neurons[i].Handle(input);
//		}
//		int maxIndex = 0;
//		for (int i = 1; i < answer.Length; i++) {
//			if(answer[i] > answer[maxIndex]) {
//				maxIndex = i;
//			}
//		}
//		return maxIndex;
//	}
//	public void Study(int[] input, int correctAnswer, int factor) {
//		Neurons[correctAnswer].Study(input, factor);
//	}
//}
