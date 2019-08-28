using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testswarm : MonoBehaviour {
    //var target:Vector3;
    private Vector3 target;
    //var timer:float;
    private float timer;
    //var sec:int;
    private int sec;

    void Start()
    {
        target = ResetTarget();
        sec = ResetSec();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > sec)
        {
            target = ResetTarget();
            sec = ResetSec();
        }
        transform.Translate(target  * Time.deltaTime);
    }

    Vector3 ResetTarget()
    {
        return new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
    }

    int ResetSec()
    {
        timer = 0;
        return Random.Range(1, 3);
    }

}
