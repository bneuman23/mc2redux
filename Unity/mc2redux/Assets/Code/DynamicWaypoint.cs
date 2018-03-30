using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWaypoint : MonoBehaviour
{
    [SerializeField]
    protected float _connectivityRadius = 50f;

    [SerializeField]
    float debugDrawRadius = 1.0f;

    List<DynamicWaypoint> _connections; 

	void Start ()
    {
        //Grab all waypoint objects in scene.
        GameObject[] AllWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        //Create a list of waypoints I can refer to later.
        _connections = new List<DynamicWaypoint>();

        //Check if they're a connected waypoint.
        for(int i = 0; i < AllWaypoints.Length; i++)
        {
            DynamicWaypoint nextWaypoint = AllWaypoints[i].GetComponent<DynamicWaypoint>();

            //i.e. we found a waypoint.
            if(nextWaypoint != null)
            {
                if(Vector3.Distance(this.transform.position, nextWaypoint.transform.position) <= _connectivityRadius && nextWaypoint != this)
                {
                    _connections.Add(nextWaypoint);
                }
            }
        }
	}

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _connectivityRadius);
    }

    public DynamicWaypoint NextWaypoint(DynamicWaypoint previousWaypoint)
    {
        if(_connections.Count == 0)
        {
            //No waypoints? Retrun null and complain.
            Debug.LogError("Insufficient waypoint count.");
            return null;
        }
        else if(_connections.Count == 1 && _connections.Contains(previousWaypoint))
        {
            //Only one waypoint and its the previous one? Just use that.
            return previousWaypoint;
        }
        else //Otherwise, find a random one that isn't the precious one.
        {
            DynamicWaypoint nextWaypoint;
            int nextIndex = 0;

            do
            {
                nextIndex = UnityEngine.Random.Range(0, _connections.Count);
                nextWaypoint = _connections[nextIndex];
            } while (nextWaypoint == previousWaypoint);

            return nextWaypoint;
        }
    }
}
