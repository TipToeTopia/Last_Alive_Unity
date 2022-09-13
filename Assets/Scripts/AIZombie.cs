using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // needed for AI

public class AIZombie : MonoBehaviour
{
    public float EnemyDist; // distance

    public int health = 100; // zombie health

    public float patrolSpeed = 2f; // speed when patrolling.
    public float chaseSpeed = 4f; // speed when chasing.
    public float patrolWaitTime = 1f; // The amount of time to wait when the patrol way point is reached
    public float patrolTimer; // A timer for the patrolWaitTime

    public Transform[] patrolWayPoints; // An array of transforms for the patrol route

    private int wayPointIndex; // A counter for the way point array

    public Transform target; // player coordinate reference  

    private float visionangle = 100f; // field of view
    //private float viewdistance = 30f; // view distance (also area of sector radius)

    private NavMeshAgent agent; // ref to the navmesh
    private Animator animator; // ref to animator

    private bool isAware = false; // set the chase to false
    private bool isDetecting = false; // set the detector to false (this will be used to test if the zombie is currently perceiving when aware, to test if player is outside vision for zombie to return back to patrol state)

    private float afterdetectiontime = 5f; // so when the player is out of range, when this timer reaches 0 the zombie returns to patrolling
    private float afterdetectiontimercountdown = 0; // initialised timer going from: this to "detectiontime", when equal to detection time zombie returns to patrol

    private float detectiontime = 2.5f; // buffer for cone of vision
    private float detectiontimecountdown = 0; // countdown limit

    private float detectiontime2 = 0.25f; // buffer for cone of vision
    private float detectiontimecountdown2 = 0; // countdown limit

    private float detectiontime3 = 1.5f; // buffer for cone of vision
    private float detectiontimecountdown3 = 0; // countdown limit

    [HideInInspector]
    public static int healthreduction;

    private Collider[] ragdollcolliders; // an array for all the ragdollcolliders
    private Rigidbody[] ragdollrigidbodies; // an array for all rigid bodies for the ragdolls

    public AudioSource zombiehurt;
    public AudioSource zombieaware;
    private int y;

    public void Start()
    {
        y = 1;
        agent = GetComponent<NavMeshAgent>(); // get navmesh component attatched on the enemy
        animator = GetComponentInChildren<Animator>(); // get animator component attatched to the child object attatched to the main zombie object
        ragdollcolliders = GetComponentsInChildren<Collider>(); // ragdoll colliders 
        ragdollrigidbodies = GetComponentsInChildren<Rigidbody>(); // ragdoll rigidbody (kinematic must be turned off for the ragdoll to trigger upon death)

        foreach (Collider col in ragdollcolliders) // colliders on
        {
            col.enabled = true;
        }

        foreach (Rigidbody rb in ragdollrigidbodies) // kinematic on
        {
            rb.isKinematic = true;
        }
    }

    public void FixedUpdate() // useful for physics/ math intensive calculations
    {
        if (health <= 0)
        {
            foreach (Rigidbody rb in ragdollrigidbodies) // for all rigidbodies of all the joints...
            {
                rb.isKinematic = false; // trigger ragdoll
            }

            chaseSpeed = 0; // zombie stops moving
            animator.enabled = false; // zombie stops animating

            transform.GetChild(0).gameObject.SetActive(false); // turn off child (cube) that was used for collider
            transform.GetComponent<Collider>().enabled = false; // turn of zombie overall collider

            return; // zombie is dead, therefore all code below ceases to exist...
        }

        EnemyDist = Vector3.Distance(target.transform.position, transform.position); // distance between zombie and player

        if (EnemyDist <= 4)
        {
            animator.SetBool("Attack", true); // animation is true
            GameManager.Instance.PlayerHealth(); // call the health deduction function in GameManager through a singleton 
        }
        else
        {
            animator.SetBool("Attack", false); // return to chase or patrol
        }

        if (isAware) // chase player
        {
            animator.SetBool("Aware", true); // therefore we call the animate run as true
            agent.SetDestination(target.transform.position); // set navmesh destination

            agent.speed = chaseSpeed; // speed increases

            if (zombieaware.isPlaying == false && (y == 1))
            {
                zombieaware.Play();
                y = 2;
            }
           
            if (Vector3.Distance(target.transform.position, transform.position) < 4)
            {
                agent.speed = 0; // speed 0
            }
            else
            {
                agent.speed = chaseSpeed; // continues to chase
            }

            if (!isDetecting) // if isAware is true then isDetecting becomes true too
            {
              // isDetecting keeps isAware true, even if all vision/ sense zombie conditions are off for up to the "detection time" allows it too (given player is still outside radius etc.)
              afterdetectiontimercountdown = afterdetectiontimercountdown + Time.deltaTime;
              if (afterdetectiontimercountdown >= afterdetectiontime) // if all conditions are still not resatisfied by the timer concluding, then...
              {
                y = 1;
                isAware = false; // isAware is set back to false, thus returning the zombie to its patrol state
                afterdetectiontimercountdown = 0; // timer is reset for next instance
              }
            }
        }
        else // else continue looking and sense radius apply
        {
            animator.SetBool("Aware", false); // else normal walking applies
            PatrolRoutes();
            radius();
        }
        searchforplayer(); // set outside the isAware since this needs to called constanly given isDetecting is set false in its function after each if statement, therefore it won't be called
        searchforplayer10();
        searchforplayer20();
    }

    public void radius() // general sensing/ hearing condition
    {
        if (Vector3.Distance(target.transform.position, transform.position) < 5) // normal radius (if player is simply close enough its logical for enemy to chase).
        {
            OnAware();
        }
        else if ((Vector3.Distance(target.transform.position, transform.position) < 10) && Input.GetKey(KeyCode.Space)) // running therefore radius increases
        {
            OnAware();
        }
    }

    public void OnAware() // sets the isAware to true which is where the following code is defined in
    {
        isAware = true;
        isDetecting = true;
        afterdetectiontimercountdown = 0; // reset timer after each timer onAware is called so it doesn't start from the previous "detectiontimercountdown + time.deltaTime" 

        GameObject[] Zombies = GameObject.FindGameObjectsWithTag("enemy");

        foreach (GameObject Zombie in Zombies) // considers if there were any zombies nearby that would've also been alerted by the player presence
        {
            if ((Vector3.Distance(Zombie.transform.position, transform.position) < 30))
            {
                Zombie.GetComponent<AIZombie>().isAware = true; // isAware is set to true, which initiates the chase
            }
        }
    }

    void PatrolRoutes()
    {
        agent.speed = patrolSpeed; // patrol speed

        if (agent.remainingDistance < agent.stoppingDistance) // if near the next waypoint or there is no destination...
        {
            patrolTimer = patrolTimer + Time.deltaTime; // timer begins

            if (patrolTimer >= patrolWaitTime) // if the timer greater then wait time?
            {
                if (wayPointIndex == patrolWayPoints.Length - 1) // if at final destination?
                {
                    wayPointIndex = 0; // initialised at 0 (reset once reached final waypoint reached)
                }
                else
                {
                    wayPointIndex++; // otherwise if not we cycle through the destination
                }
                patrolTimer = 0; // reset timer after each destination
            }
        }
        else
        {
            patrolTimer = 0; // if not near a destination, reset the timer.
        }
        agent.destination = patrolWayPoints[wayPointIndex].position; // set the destination to the patrolWayPoint
    }

    public void ZombieHealth()
    {
        if (health <= 0)
        {
           return;
        }
        health = health - healthreduction; // health deduction
        zombiehurt.Play();
    }

    public void searchforplayer() // cone of vision and obstacle detection (at 30 metres)
    {
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(target.transform.position)) < (visionangle / 2f)) // cone of vision angle, if player is within angle...
        {
            Debug.Log("Angle");
            if (Vector3.Distance(target.transform.position, transform.position) < 30) // then if true, if player is within the distance
            {
                Debug.Log("Distance");
                // if within sector of circle, is there an obstacle in the way?
                RaycastHit hit;
                if (Physics.Linecast((transform.position + transform.up), (target.transform.position + transform.up), out hit, -1)) // creates to lines at each end which will be the intersection lines (if they hit player (no obstacle)).
                {
                    Debug.Log("Obstacle");
                    if (hit.transform.CompareTag("Player")) // then we ask if the target is indeed the player (not an obstacle) for clarification through asking its tag
                    {
                        detectiontime = detectiontime - Time.deltaTime; // buffer begins
                        if (detectiontime <= detectiontimecountdown) // if less then 0
                        {
                            Debug.Log("Chase");
                            OnAware(); // chase
                        }
                    }
                }
                else
                {
                    detectiontime = 2.5f;
                    isDetecting = false;
                }
            }
            else
            {
                detectiontime = 2.5f;
                isDetecting = false;
            }
        }
        else
        {
            detectiontime = 2.5f;
            isDetecting = false;
        }
    }

    public void searchforplayer10() // cone of vision and obstacle detection (at 10 metres)
    {
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(target.transform.position)) < (visionangle / 2f)) // cone of vision angle, if player is within angle...
        {
            Debug.Log("Angle2");
            if (Vector3.Distance(target.transform.position, transform.position) < 10) // then if true, if player is within the distance
            {
                Debug.Log("Distance2");
                // if within sector of circle, is there an obstacle in the way?
                RaycastHit hit;
                if (Physics.Linecast((transform.position + transform.up), (target.transform.position + transform.up), out hit, -1)) // creates to lines at each end which will be the intersection lines (if they hit player (no obstacle)).
                {
                    Debug.Log("Obstacle2");
                    if (hit.transform.CompareTag("Player")) // then we ask if the target is indeed the player (not an obstacle) for clarification through asking its tag
                    { 
                        detectiontime2 = detectiontime2 - Time.deltaTime; // buffer begins
                        if (detectiontime2 <= detectiontimecountdown2) // if less then 0
                        {
                            Debug.Log("Chase2");
                            OnAware(); // chase
                        }
                    }
                }
                else
                {
                    detectiontime2 = 0.25f;
                    isDetecting = false;
                }
            }
            else
            {
                detectiontime2 = 0.25f;
                isDetecting = false;
            }
        }
        else
        {
            detectiontime2 = 0.25f;
            isDetecting = false;
        }
    }

    public void searchforplayer20() // cone of vision and obstacle detection (at 20 metres)
    {
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(target.transform.position)) < (visionangle / 2f)) // cone of vision angle, if player is within angle...
        {
            Debug.Log("Angle3");
            if (Vector3.Distance(target.transform.position, transform.position) < 20) // then if true, if player is within the distance
            {
                Debug.Log("Distance3");
                // if within sector of circle, is there an obstacle in the way?
                RaycastHit hit;
                if (Physics.Linecast((transform.position + transform.up), (target.transform.position + transform.up), out hit, -1)) // creates to lines at each end which will be the intersection lines (if they hit player (no obstacle)).
                {
                    Debug.Log("Obstacle3");
                    if (hit.transform.CompareTag("Player")) // then we ask if the target is indeed the player (not an obstacle) for clarification through asking its tag
                    {
                        detectiontime3 = detectiontime3 - Time.deltaTime; // buffer begins
                        if (detectiontime3 <= detectiontimecountdown3) // if less then 0
                        {
                            Debug.Log("Chase3");
                            OnAware(); // chase
                        }
                    }
                }
                else
                {
                    detectiontime3 = 1.5f;
                    isDetecting = false;
                }
            }
            else
            {
                detectiontime3 = 1.5f;
                isDetecting = false;
            }
        }
        else
        {
            detectiontime3 = 1.5f;
            isDetecting = false;
        }
    }
}
