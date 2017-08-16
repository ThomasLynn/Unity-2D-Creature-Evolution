using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureControl : MonoBehaviour {

    NNetwork controlNetwork;
    List<string> limbNames;

    private float startingPosition;

    // Use this for initialization
    void Start() {
        limbNames = new List<string>(new string[] { "Leg Front Upper", "Leg Front Lower" , "Leg Back Upper", "Leg Back Lower"});
        startingPosition = getXPosition();
    }

    public void setNetwork(NNetwork network)
    {
        controlNetwork = network;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.Find("Body").GetComponent<Rigidbody2D>().rotation > 0)
        {
            controlNetwork.networkValue = Mathf.Max(0.01f, getXPosition() - startingPosition);
        }
        else
        {
            controlNetwork.networkValue = 0.01f;
        }
        if (controlNetwork != null)
        {
            List<double> input = new List<double>();
            foreach (string name in limbNames)
            {
                input.Add(getY(name));
            }
            /*if (Input.GetButton("Fire1"))
            {
                input = new List<double>(new double[] { 0, 1, 0, 1 });
            }*/
            List<double> networkOutput = controlNetwork.get_outputs(input.ToArray());
            for (int i = 0; i < limbNames.Count; i++)
            {
                UpdateSpeed(limbNames[i], (float)networkOutput[i] * 100);
            }
        }
    }

    float getXPosition()
    {
        return transform.parent.Find("Body").GetComponent<Rigidbody2D>().transform.position.x;
    }

    void UpdateSpeed(string name, float speed)
    {
        HingeJoint2D joint = transform.parent.Find(name).GetComponent<HingeJoint2D>();
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = speed;
        joint.motor = motor;
    }

    float getY(string name)
    {
        Rigidbody2D rigitBody = transform.parent.Find(name).GetComponent<Rigidbody2D>();
        return rigitBody.position.y;
    }
}
