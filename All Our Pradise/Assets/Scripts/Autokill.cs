using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autokill : MonoBehaviour
{
    public float lifeTime = 0.5f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, lifeTime);
	}

}
