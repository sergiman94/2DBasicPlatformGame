using UnityEngine;
using System.Collections;

public class armRotation : MonoBehaviour {

	public int rotationOffSet = 90;

	// Update is called once per frame
	void Update () {
	
		Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position; // subtracting thye position of the player from mouse position 

		difference.Normalize ();			// Normalizing the vector. meaning that all the sum of the vector will be one.

		float rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;  // find the angle in Degrees3

		transform.rotation = Quaternion.Euler (0f,0f, rotZ + rotationOffSet );

	}
}
