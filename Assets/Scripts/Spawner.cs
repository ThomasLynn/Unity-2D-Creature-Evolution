using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

    public GameObject myPrefab;
    public Text scoreText;
    public int CREATURE_COUNT;
    
    private List<CreatureContainer> containerList;
    private int randomToDeploy;

    // Use this for initialization
    void Start() {
        containerList = new List<CreatureContainer>();
        randomToDeploy = CREATURE_COUNT;
        //Time.timeScale = 2;
        Application.runInBackground = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        scoreText.text = Time.smoothDeltaTime.ToString();
        for (int spawnCount = 0; spawnCount < 2; spawnCount++)
        {
            if (containerList.Count < CREATURE_COUNT)
            {
                float resultsMax = 0;
                foreach (CreatureContainer creature in containerList)
                {
                    resultsMax += Mathf.Pow(creature.network.networkValue,2);
                }
                if (randomToDeploy <= 0)
                {
                    NNetwork networkToCopy = null;
                    float count = 0;
                    float randomCount = Random.value * resultsMax;
                    foreach (CreatureContainer creature in containerList)
                    {
                        count += Mathf.Pow(creature.network.networkValue, 2);
                        if (count >= randomCount)
                        {
                            networkToCopy = creature.network;
                            break;
                        }
                    }
                    if (networkToCopy == null)
                    {
                        Debug.Log("this... err. shouldn't happen");
                    }
                    containerList.Add(new CreatureContainer(makeCreature(), networkToCopy));
                }
                else
                {
                    containerList.Add(new CreatureContainer(makeCreature(), null));
                    randomToDeploy--;
                }
            }
            else
            {
                break;
            }
        }
        for(int i=0;i<containerList.Count;i++)
        {
            if (containerList[i].isDead())
            {
                Destroy(containerList[i].creature);
                containerList.RemoveAt(i);
                i--;
            }
        }
        
    }

    GameObject makeCreature()
    {
        return Instantiate(myPrefab, transform.position, Quaternion.identity);
    }
}
