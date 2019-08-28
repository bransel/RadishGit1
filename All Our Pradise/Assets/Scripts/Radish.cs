using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Radish : MonoBehaviour
{
	public Animator anim;
	public NavMeshAgent agent;

	public NavRandomPoint navRandomPoint;
    public int RadishState;
    private Vector3 dest;
    public bool FrontRadish = false; 
    public float animationClock =5f; // the timer for animations
    public float animationInterval = 5f; // the amount of time for animations to play
    public float runClock = 6f; // the timer for running
    public float runCycle = 6f; // the time for running
    public bool radishRunning; // if radish is moving
    public bool radStop = true;
    public ImageLoader imagescript;
    public Vector3 radBlob;

    void OnDrawGizmosSelected()
    {
        // Draw a blue sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(dest, 0.3f);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if animation is playing
        if (animationClock > 0)
        {
            animationClock -= Time.deltaTime; 

        }
        else
        {
            AnimationCycle();
        }

        switch (RadishState)
        {

            case 7:

                //idle

                anim.SetBool("isIdle", true);
                RadishState = 0;
                agent.speed = 2;

                break;

            case 6:

                //eartwitch
                anim.SetBool("isTwitching", true);
                RadishState = 0;
                break;


            case 5:

                //lookaround

                anim.SetBool("isLooking", true);
                RadishState = 0;
                break;

            case 4:

                //headsidebob

                anim.SetBool("isBobbing", true);
                RadishState = 0;
                break;


            case 3:

                //snooze

                anim.SetBool("isSnoozing", true);
                RadishState = 0;


                break;


            case 2:
                //nod


                anim.SetBool("isNodding", true);
                RadishState = 0;
                break;

            case 1:

                //moving around

                                radRun();
               

                break;

            default:

               


                break;



        }

    }

    void AnimationCycle()
    {
        if (!radishRunning) // if radish finished running/isn't moving - radish needs to be still before idle or animations play
        {
            animsOff();
            // 1/3 chance to run, to idle, or to do animations

            int randIndex = Random.Range(0, 3);
            if (randIndex == 0)

            {
                //running 
                RadishState = 1; // 1 is running in switch statement
                animationClock = animationInterval; // reset animation timer
            }

            else if (randIndex == 1)
            {
                // animations
                int randIndex2 = Random.Range(2, 6); // 2-6 are animation states
                RadishState = randIndex2; 
                animationClock = animationInterval; // reset timer
            }

            else
            {

                //idle
                RadishState = 7; // idle state
                animationClock = animationInterval;

            }
        }
    
        
       
    }

    public Vector3 radBlobP()
    {
        int RandomBlob = Random.Range(0, imagescript.blobsPosPlane.Length); // get a random blob's index
        Vector3 radBlob = imagescript.blobsPosPlane[RandomBlob];
        return radBlob;
    }

    void radRun() // Peter's script that ive ruined
    {
        runClock -= Time.deltaTime; // start the run clock
        anim.SetBool("isRunning", true);
        radishRunning = true;

        if (runClock >= 0) // while in run mode
        {

            if (dest == Vector3.zero)
            {
                dest = navRandomPoint.GetPoint();

                if (dest != Vector3.zero)
                    agent.SetDestination((Vector3)dest);
            }
            else
            {
                agent.speed = 2;

                if ((transform.position - (Vector3)dest).magnitude < 0.5f)
                {
                    dest = Vector3.zero;
                }
            }

        }
        else if (runClock <= 0) //after running
        {
            if (imagescript.blobsPosPlane.Length >= 1) // if there are blobs present
            {

                
                

                if (radStop == true)
                {
                    dest = radBlobP();
                    radBlob = radBlobP();

                    NavMeshHit navHit;

                    if (NavMesh.SamplePosition(dest, out navHit, 0.1f, NavMesh.AllAreas)) // trying to point to navmesh
                    {
                        agent.SetDestination(navHit.position);
                        Debug.Log("followingblob");
                        Debug.Log(navHit.position);
                        Debug.Log(radBlob);
                        radStop = false; //stop looking for a destination
                        
                    }
                   

                }

                if ((transform.position - (Vector3)dest).magnitude <= 0.5f) //is this when radish gets to the point?
                {
                    radishRunning = false;
                    radStop = true;// hmm
                    runClock = runCycle; // reset the runclock
                }
            }
            else
            {

                if (radStop == true)
                {
                    dest = navRandomPoint.GetPoint(); // get a random point ONCE hopefully


                    if (dest.z >= 4) // trying to reposition the radish to the front
                    {
                        dest.z = Random.Range(0f, 3f); // picking a closer z value


                    }
                    NavMeshHit navHit;

                    if (NavMesh.SamplePosition(dest, out navHit, 0.1f, NavMesh.AllAreas))
                    {
                        agent.SetDestination(navHit.position);
                        radStop = false; //stop looking for a destination

                    }
                }

                if ((transform.position - (Vector3)dest).magnitude <= 0.5f) //is this when radish gets to the point?
                {
                    radishRunning = false;
                    radStop = true;// hmm reset this bol
                    runClock = runCycle; // reset the runclock
                }
            }

        }
    }
    void animsOff()
    {
        anim.SetBool("isSnoozing", false);
        anim.SetBool("isTwitching", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isLooking", false);
        anim.SetBool("isNodding", false);
        anim.SetBool("isIdle", false);
        anim.SetBool("isBobbing", false);

    }
}
