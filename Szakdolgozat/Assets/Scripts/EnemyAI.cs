using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof (Rigidbody2D))]
[RequireComponent(typeof (Seeker))]
public class EnemyAI : MonoBehaviour
{
    GameObject player;
    public Transform target;
    public float updateRate = 2f;
    private Seeker seeker;
    private Rigidbody2D rb;
    public Path path;
    public float speed = 7f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;
    public float nextWaypointDistance = 3;
    private int currentWaypoint = 0;
    public EnemyState CurrS;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if(target == null){
            Debug.Log("No player found");
            return;
        }   

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath(){
        if(CurrS ==EnemyState.Idle){
            yield return false;
        }
        if(player == null){
            yield return false;
        }else{
            seeker.StartPath(transform.position, target.position, OnPathComplete);

            yield return new WaitForSeconds(1f/updateRate);

            StartCoroutine(UpdatePath());
        }

        
    }

    public void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        CurrS = GetComponent<EnemyController>().currState;

        if(CurrS ==EnemyState.Idle){
            return;
        }
        
        if(target == null){
            return;
        }
        if(path == null){
            return;
        }
        if(currentWaypoint >= path.vectorPath.Count){
            if(pathIsEnded) return;

            Debug.Log("End of path reached.");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
        dir *= speed *Time.fixedDeltaTime;

        rb.AddForce(dir,fMode);

        float dist = Vector3.Distance (transform.position, path.vectorPath[currentWaypoint]);
        if(dist < nextWaypointDistance){
            currentWaypoint++;
            return;
        }
    }
}
