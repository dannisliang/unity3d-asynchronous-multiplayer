using UnityEngine;
using System.Collections;

public class FriendCrashedTrigger : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other) {
		GetComponentInParent<Rigidbody> ().isKinematic = true;
		GetComponentInParent<FriendScroller> ().enabled = false;
		GetComponentInParent<FriendDragonController> ().enabled = false;
	}
}
