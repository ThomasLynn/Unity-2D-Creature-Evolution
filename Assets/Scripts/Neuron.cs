using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron{

    public static float randomness = 0.01f;

    private bool resultsValid;
    private double temporyResult;
    private double lockedInput;
    public List<Connection> connections;

    public Neuron(List<Neuron> neuronList, List<Connection> connectionList, float randomMultipltier)
    {
        resultsValid = false;
        temporyResult = 0d;
        lockedInput = 0d;
        connections = new List<Connection>();
        for (int i=0;i<neuronList.Count;i++)
        {
            // need to normalize the newWeight against an average... at some point
            double newWeight = connectionList[i].weight + (Random.value - 0.5f) * randomness * randomMultipltier * Mathf.Exp(connectionList[i].variance);
            float newVariance = connectionList[i].variance + (Random.value - 0.5f);
            connections.Add(new Connection(neuronList[i], newWeight, newVariance));
        }

    }

    public Neuron(List<Neuron> neuronList)
    {
        resultsValid = false;
        temporyResult = 0d;
        lockedInput = 0d;
        connections = new List<Connection>();
        foreach (Neuron neuron in neuronList)
        {
            connections.Add(new Connection(neuron, Random.value*2-1, 0));
        }
        
    }

    public Neuron(double input)
    {
        resultsValid = false;
        temporyResult = 0d;
        lockedInput = input;
        connections = null;
    }

    public void setLockedInput(double value)
    {
        lockedInput = sigSquash(value);
    }

    private double sigSquash(double value)
    {
        return 2d / (1d + Mathf.Exp((float)-value)) - 1d;
    }

    public double getValue()
    {
        if (resultsValid)
        {
            return temporyResult;
        }
        if (connections == null)
        {
            return lockedInput;
        }
        double total_value = 0;
        foreach (Connection connection in connections)
        {
            total_value += connection.getValue();
        }
        temporyResult = sigSquash(total_value);
        resultsValid = true;
        return temporyResult;
    }

    public void invalidateResult()
    {
        resultsValid = false;
    }
}

public class Connection
{
    private Neuron neuron;
    public double weight;
    public float variance;
    public Connection(Neuron connectedNeuron, double startingWeight, float variance)
    {
        neuron = connectedNeuron;
        weight = startingWeight;
    }

    public double getValue()
    {
        return neuron.getValue() * weight;
    }
}