using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class clickmove : MonoBehaviour
{
    private GameObject target;
    NavMeshAgent agent;
    public LayerMask hitlayers;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(1))
        { if (Physics.Raycast(mouseRay, out hit, 1000f, hitlayers))
                
                {

                Debug.Log("hit");
                agent.SetDestination(hit.point);
            }

        }



            
    }

   
}
