using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour {
    public Animation die;
	// Use this for initialization
	void Start () {
        die = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButton("Fire1")) {
            die.CrossFade("Attack_1");
        }
        if (Input.GetButton("Fire2")) { 
            die.CrossFade("Attack_2");
        }
        if (Input.GetButton("Fire3"))
        {
            die.CrossFade("Attack_3");
        }
        if (Input.GetButton("Jump"))
        {
            die.CrossFade("Attack_4");
        }

    }
}
