using UnityEngine;
using System.Collections;

public class DragonController : MonoBehaviour {

	private Rigidbody rb;

	// The force which is added when the player jumps
	// This can be changed in the Inspector window
	public Vector3 jumpForce = new Vector3(0, 300, 0);

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Jump
		if (Input.GetMouseButtonDown (0)) {
			rb.velocity = Vector3.zero;
			rb.AddForce(jumpForce);
		}
	}
}
