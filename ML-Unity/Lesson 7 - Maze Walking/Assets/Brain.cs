using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    int DNALength = 2;
    public DNA dna;
    public GameObject eyes;

    bool seeWall = true;
    Vector3 startPosition;
    public float distanceTravelled = 0;
    bool alive = true;

    // Initialise DNA
    public void Init()
    {
        // 0 Forward
        // 1 Angle Turn
        dna = new DNA(DNALength, 360);
        startPosition = this.transform.position;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "dead")
        {
            distanceTravelled = 0;
            alive = false;
        }
    }

    void Update()
    {
        if (!alive) return;

        seeWall = false;
        RaycastHit hit;
        Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 0.5f, Color.red);
        if(Physics.SphereCast(eyes.transform.position, 0.1f, eyes.transform.forward, out hit, 0.5f))
        {
            if(hit.collider.gameObject.tag == "wall")
            {
                seeWall = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (!alive) return;

        // Read the DNA
        float h = 0;
        float v = dna.GetGene(1);

        if (seeWall)
        {
            h = dna.GetGene(1);
        }

        this.transform.Translate(0, 0, v * 0.001f);
        this.transform.Rotate(0, h, 0);
        distanceTravelled = Vector3.Distance(startPosition, this.transform.position);
    }
}