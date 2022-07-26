using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK : MonoBehaviour
{
    [SerializeField] Transform [] nodes;
    [SerializeField] Transform target;

    float [] nodeslength;
    Vector3 [] tempTm;
    Vector3 pivot;

    private void Start() 
    {
        tempTm = new Vector3[nodes.Length];
        nodeslength = new float[nodes.Length-1];
        pivot = nodes[0].position;

        for(int i=0; i<nodes.Length-1; i++)
        {
            nodeslength[i] = Vector3.Distance(nodes[i].position, nodes[i+1].position);
        }
        

        for(int i=0; i<nodes.Length; i++)
            tempTm[i] = nodes[i].position;


        IK_FABRIK();            
    }


    void Step0()
    {
        // end effector position
        tempTm[nodes.Length-1] = target.position;
        Vector3 prevpos = target.position;

        // Target으로 node간의 길이 맞춰 이동
        for(int i=nodes.Length-2; i>=0; i--)
        {
            Vector3 dir = (tempTm[i] - prevpos).normalized;
            tempTm[i] = prevpos + dir * nodeslength[i];
            prevpos = tempTm[i]; 
        }
    }

    void Step1()
    {
        tempTm[0] = pivot;
        Vector3 prevpos = pivot;

        for(int i=1; i<nodes.Length; i++)
        {
            tempTm[i] = prevpos + (tempTm[i] - prevpos).normalized * nodeslength[i-1];
            prevpos = tempTm[i];
        }
        
    }

    void IK_FABRIK()
    {
        Step0();
        Step1();

        Vector3 prev = nodes[0].position;
        for(int i=0; i<nodes.Length; i++)
        {
            nodes[i].position = tempTm[i];

            if (i > 0)
                Debug.DrawLine(prev, tempTm[i]);
            prev = nodes[i].position;
        }
    }

    void Update()
    {
        IK_FABRIK();
    }
}
