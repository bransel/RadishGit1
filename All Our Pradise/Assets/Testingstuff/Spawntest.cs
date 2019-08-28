using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawntest : MonoBehaviour
{
    private GameObject target;
    public GameObject[] Spawn;
    public LayerMask hitlayers;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(mouseRay, out hit, 10000f, hitlayers))

            {

                Debug.Log("hit");
                Instantiate(Spawn[0], hit.point, Quaternion.identity);
            }

        }




    }

    
}
