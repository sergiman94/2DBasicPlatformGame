using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

	public int offSetX = 2; 					// The offset so that we don't get any weird errors 

	// these are used for checking if we need to instantiate stuff 
	public bool hasARightBuddy = false;					
	public bool hasALeftBuddy = false;

	public bool reverseScale = false;	// used if the object is not tilable 

	private float spriteWidth = 0f;		// the width of our element
	private Camera cam;
	private Transform myTransform;

	void Awake () {


		cam = Camera.main;	
		myTransform = transform;

	}


	// Use this for initialization
	void Start () {

		SpriteRenderer sRenderer = GetComponent<SpriteRenderer> ();
		spriteWidth = sRenderer.sprite.bounds.size.x;

	}

	// Update is called once per frame
	void Update () {

		// does it still need buddies ? if not do anything
		if (hasALeftBuddy == false || hasARightBuddy == false ){

			// calculate cameras extend (half the width) of what the cameras can see in world coordinates  
			float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

			// calculate de x position where the camera can see the edge of the sprite (element)
			float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2)- camHorizontalExtend;
			float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;

			// checking if we can see the edge of the element and then calling makeNewBuddy if we can 	
			if (cam.transform.position.x >= edgeVisiblePositionRight - offSetX && hasARightBuddy == false ) {

				makeNewBuddy (1);
				hasARightBuddy = true;

			} else if (cam.transform.position.x <= edgeVisiblePositionLeft + offSetX && hasALeftBuddy == false ) {

				makeNewBuddy (-1);
				hasALeftBuddy = true;

			} 
		}
	}
	// A function that creates a buddy on the side required 
	void makeNewBuddy (int rightOrLeft) {

		// calculating the new position for our new buddy 
		Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z); 

		// Instantiating our new body and storing him in a variable 
		Transform newBuddy = Instantiate (myTransform, newPosition, myTransform.rotation) as Transform;

		// if not tilable let's reverse the x size of our object to get rid of ugly seams 
		if (reverseScale == true){
			newBuddy.localScale = new Vector3 (newBuddy.localScale.x*-1, newBuddy.localScale.y, newBuddy.localScale.z);
		} 

		newBuddy = myTransform.parent;

		if (rightOrLeft > 0) {

			newBuddy.GetComponent<Tiling> ().hasALeftBuddy = true;

		} else {

			newBuddy.GetComponent<Tiling> ().hasARightBuddy = true;
		}
	}

}
