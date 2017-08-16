using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNetwork{

    public float networkValue;

    private int recurrentNeurons;
    private int[] neuronLayout;
    private List<List<Neuron>> neuralNetwork;

	public NNetwork(List<int> networkNeuronLayout, int networkRecurrentNeurons)
    {
        recurrentNeurons = networkRecurrentNeurons;
        neuronLayout = networkNeuronLayout.ToArray();
        neuralNetwork = new List<List<Neuron>>();
        setupNetwork(null, 0);
        networkValue = 0;
    }

    public NNetwork(NNetwork copyNetwork, float randomModifier)
    {
        recurrentNeurons = copyNetwork.recurrentNeurons;
        neuronLayout = copyNetwork.neuronLayout;
        neuralNetwork = new List<List<Neuron>>();
        setupNetwork(copyNetwork, randomModifier);
    }

    private void setupNetwork(NNetwork copyNet, float randomModifier)
    {
        List<Neuron> previousLayer = null;
        for (int i = 0; i < neuronLayout.Length; i++)
        {
            List<Neuron> currentLayer = new List<Neuron>();
            if (previousLayer == null)
            {
                for (int j = 0; j < neuronLayout[i] + recurrentNeurons; j++)
                {
                    currentLayer.Add(new Neuron(0));
                }
            }
            else
            {
                for (int j = 0; j < neuronLayout[i]; j++)
                {
                    if (copyNet == null)
                    {
                        currentLayer.Add(new Neuron(previousLayer));
                    }
                    else
                    {
                        currentLayer.Add(new Neuron(previousLayer, copyNet.neuralNetwork[i][j].connections, randomModifier));
                    }
                    
                }
            }
            if (i == neuronLayout.Length - 1)
            {
                for (int j = 0; j < recurrentNeurons; j++)
                {
                    currentLayer.Add(new Neuron(0));
                }
            }
            else
            {
                currentLayer.Add(new Neuron(1));
            }
            previousLayer = currentLayer;
            neuralNetwork.Add(currentLayer);
        }
    }

    public List<double> get_outputs(double[] inputs)
    {
        for (int i = 0; i < neuronLayout[0]; i++)
        {
            neuralNetwork[0][i].setLockedInput(inputs[i]);
        }
        List<double> results = new List<double>();
        foreach (Neuron neuron in neuralNetwork[neuralNetwork.Count-1])
        {
            results.Add(neuron.getValue());
        }
        foreach (List<Neuron> neuronLayer in neuralNetwork)
        {
            foreach (Neuron neuron in neuronLayer)
            {
                neuron.invalidateResult();
            }
        }
        for (int i = 0; i < recurrentNeurons; i++)
        {
            neuralNetwork[0][i+ neuronLayout[0]].setLockedInput(results[i]);
        }
        return results;
    }
}
