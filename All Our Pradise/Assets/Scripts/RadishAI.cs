using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RadishAI : MonoBehaviour {

    private Vector3 target;
    NavMeshAgent agent;
    public LayerMask hitlayers;
    public float moveTimer;
    public float idleTimer;
    Animator anim;
    private int randx;
    private int randy;
 
    //State for radish movement/animation 
    public int RadishState;
    public float Timer;
   

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = this.gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        Timer+= Time.deltaTime;
        if(Timer >= idleTimer)
        {
            RadishState = 1;
            Timer = 0;
        }





        switch (RadishState)
        {
            //case 2:



            //  break;

            case 1:

                //moving around

                Timer++;
             
                randx = Random.Range(0, Screen.width);
                randy = Random.Range(0, Screen.height);
                Vector3 rand3 = new Vector3(randx, randy, 0);
                Ray randRay = Camera.main.ScreenPointToRay(rand3);
                RaycastHit hit;


                if ((Physics.Raycast(randRay, out hit, 1000f, hitlayers)) && Timer >= moveTimer)

                {

                    Debug.Log("hit");
                    anim.SetBool("isRunning", true);
                    agent.SetDestination(hit.point);
                    Timer = 0;

                }

               

                

           

            break;

            default:

            //idle

            anim.SetBool("isRunning", false);
                
                break;



        }

        
         



    }

}
