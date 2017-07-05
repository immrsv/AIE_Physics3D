using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class SpringDecay : MonoBehaviour {


    public float MaxStrength = 500.0f;
    public float MinStrength = 50f;
    public float DecayRate = 2.0f;

    private HingeJoint[] Hinges;

	// Use this for initialization
	void Start () {
        Hinges = GetComponentsInChildren<HingeJoint>();

        ResetSprings();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        foreach (var hinge in Hinges) {
            var spr = hinge.spring;
            spr.spring = Mathf.Max(MinStrength, spr.spring - (DecayRate * Time.fixedDeltaTime));
            hinge.spring = spr;
        }
    }

    public void ResetSprings()
    {
        foreach (var hinge in Hinges)
        {
            var spr = hinge.spring;
            spr.spring = MaxStrength;
            hinge.spring = spr;
        }
    }
}
