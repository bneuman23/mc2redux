using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    //Dictates whether the agent waits on each node.
    [SerializeField]
    bool _patrolWaiting;

    //The total time we wait at each node.
    [SerializeField]
    float _totalWaitTime = 3f;

    //Private Variables for base behavior.
    NavMeshAgent _navMeshAgent;
    DynamicWaypoint _currentWaypoint;
    DynamicWaypoint _previousWaypoint;

    bool _travelling;
    bool _waiting;
    float _waitTimer;
    int _waypointsVisited;

	// Use this for initialization
	void Start ()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if(_navMeshAgent == null)
        {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        }
        else
        {
            if(_currentWaypoint == null)
            {
                //Set it at random.
                //Grab all waypoint objects in scene.
                GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

                if (allWaypoints.Length > 0)
                {
                    while(_currentWaypoint == null)
                    {
                        int random = UnityEngine.Random.Range(0, allWaypoints.Length);
                        DynamicWaypoint startingWaypoint = allWaypoints[random].GetComponent<DynamicWaypoint>();

                        //i.e. we found a waypoint.
                        if(startingWaypoint != null)
                        {
                            _currentWaypoint = startingWaypoint;
                        }
                    }
                }
                else
                {
                    Debug.Log("Failed to find any waypoints for use in the scene.");
                }
            }
            else
            {
                Debug.Log("Insufficient patrol points for basic patrolling behavior.");
            }

            SetDestination();
        }
	}

    // Update is called once per frame
    public void Update()
    {
        //Checks if we're close to the destination.
        if(_travelling && _navMeshAgent.remainingDistance <= 1.0f)
        {
            _travelling = false;
            _waypointsVisited++;

            //If we're going to wait, then wait.
            if (_patrolWaiting)
            {
                _waiting = true;
                _waitTimer = 0f;
            }
            else
            {
                SetDestination();
            }
        }

        //Instead if we're waiting.
        if (_waiting)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _totalWaitTime)
            {
                _waiting = false;

                SetDestination();
            }
        }
    }

    private void SetDestination()
    {
        if(_waypointsVisited > 0)
        {
            DynamicWaypoint nextWaypoint = _currentWaypoint.NextWaypoint(_previousWaypoint);
            _previousWaypoint = _currentWaypoint;
            _currentWaypoint = nextWaypoint;
        }
        Vector3 targetVector = _currentWaypoint.transform.position;
        _navMeshAgent.SetDestination(targetVector);
        _travelling = true;
    }
}
