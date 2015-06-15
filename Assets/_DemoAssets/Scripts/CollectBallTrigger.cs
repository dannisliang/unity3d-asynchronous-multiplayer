using UnityEngine;
using System.Collections;

public class CollectBallTrigger : MonoBehaviour {

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
		other.enabled = false;
		other.GetComponentInParent<MeshRenderer> ().enabled = false;

		if (other.tag == "BonusBall") {
			GameManager.Instance.OnDragonGetBall (true);
		} else {
			GameManager.Instance.OnDragonGetBall (false);
		}
	}
}
