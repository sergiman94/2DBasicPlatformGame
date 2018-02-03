using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]

public class EnemyAI : MonoBehaviour {


	public Transform target;

	// How many times each second we will update our path
	public float updateRate;

	// Caching 
	private Seeker seeker;
	private Rigidbody2D rb;

	// The Calculate Path 
	public Path path;

	// The AI speed per second 

	public float speed = 300f;
	public ForceMode2D fmode;

	[HideInInspector]

	public bool pathIsEnded = false;

	// The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWayPointDistance = 3f;

	private int currentWayPoint = 0;

	private bool searchingForPlayer = false;




	void Start () {
	
		seeker = GetComponent<Seeker> ();
		rb = GetComponent<Rigidbody2D> ();

		if (target == null) {
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (searchForPlayer ());
			}
			return;
		}


		// Start a new path to the target position, return the result to the OnPathComplet method 
		seeker.StartPath (transform.position, target.position, OnPathComplete);

		StartCoroutine (UpdatePath ());


	}

	IEnumerator searchForPlayer () {

		GameObject sResult = GameObject.FindGameObjectWithTag ("Player");

		if (sResult == null) {

			yield return new WaitForSeconds (0.5f);
			StartCoroutine (searchForPlayer ());
		} else {
		
			target = sResult.transform;
			searchingForPlayer = false;
			StartCoroutine (UpdatePath ());
			yield return false;
		}
	}
	IEnumerator UpdatePath (){

		if (target == null) {
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (searchForPlayer ());
			}
			yield return false;
		}
			
			seeker.StartPath (transform.position, target.position, OnPathComplete);

			yield return new  WaitForSeconds (1f / updateRate);
			StartCoroutine (UpdatePath ());

	}
	public void OnPathComplete (Path p){

		Debug.LogError ("We got a path. Did it have an erro ?"+ p.error) ;

		if (!p.error) {

			path = p;
			currentWayPoint = 0;
		}
	}

	public void FixedUpdate () {
	
		if (target == null) {
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (searchForPlayer ());
			}
			return;
		}

		// TODO: Always look at player ? 

		if (path == null) {

			return;
		}

		if (currentWayPoint >= path.vectorPath.Count) {

			if (pathIsEnded)
				return;

			Debug.Log ("End of path reached.");
			pathIsEnded = true;
			return;
		}
		pathIsEnded = false;

		// Direction to the next waypoint

		Vector3 dir = (path.vectorPath [currentWayPoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;

		//Move the AI 
		rb.AddForce (dir, fmode);

		float dist = Vector3.Distance (transform.position, path.vectorPath [currentWayPoint]);

		if (dist < nextWayPointDistance) {

			currentWayPoint++;
			return;
		}
	}			
}

