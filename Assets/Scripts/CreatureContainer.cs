using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureContainer{
    public GameObject creature;
    public NNetwork network;
    
    private float personalBest;
    private float timer;
    private float age;

    public CreatureContainer(GameObject inputCreature, NNetwork inputNetwork)
    {
        creature = inputCreature;
        if (inputNetwork == null)
        {
            network = new NNetwork(new List<int>(new int[] { 9, 15, 15, 9 }), 5);
            creature.transform.Find("CreatureControl").SendMessage("setNetwork", network);
            //Debug.Log("making random network");
        }
        else
        {
            float factor = Mathf.Max(1, Mathf.Min(1000, 1000 / inputNetwork.networkValue));
            //Debug.Log("making network with variance factor "+factor);
            network = new NNetwork(inputNetwork,factor);//1000);
            creature.transform.Find("CreatureControl").SendMessage("setNetwork", network);
        }
        age = 0;
        personalBest = 0;
        timer = 5;
    }

    public bool isDead()
    {
        age += Time.deltaTime;
        if (creature.transform.Find("Body").transform.position.x <= personalBest + Mathf.Max(0, age/5))
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 5;
            personalBest = creature.transform.Find("Body").transform.position.x;
        }
        return timer <= 0;
    }
	
}
