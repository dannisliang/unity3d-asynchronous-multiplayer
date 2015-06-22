using UnityEngine;
using System.Collections;

public class DragonController : MonoBehaviour {
	
	public GameObject dragonModel = null;
	// The force which is added when the player jumps
	// This can be changed in the Inspector window
	public Vector3 jumpForce = new Vector3(0, 300, 0);

	private float playTimeTotal = 0.0f;
	private Rigidbody dragonRigidbody = null;
	private string jumpData = "";

	public string JumpData {
		get {
			return jumpData;
		}
	}

	void Awake() {
		dragonRigidbody = dragonModel.GetComponent<Rigidbody> ();
		jumpData = "";
	}

	// Use this for initialization
	void Start () {
		playTimeTotal = 0.0f;

		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		playTimeTotal += Time.deltaTime;
		
		// Jump dragon model
		if (Input.GetMouseButtonDown (0)) {
			dragonRigidbody.velocity = Vector3.zero;
			dragonRigidbody.AddForce (jumpForce);

			jumpData += playTimeTotal + "|";
		}
	}

	public void OnTapToPlay() {
		enabled = true;
		dragonRigidbody.useGravity = true;
		dragonRigidbody.isKinematic = false;
	}
	
	/// <summary>
	/// Reset this instance.
	/// </summary>
	public void Reset() {
		playTimeTotal = 0.0f;
		enabled = false;
		jumpData = "";
		dragonRigidbody.velocity = Vector3.zero;
		dragonRigidbody.useGravity = false;
		dragonModel.transform.position = new Vector3 (-2.5f, -0.25f, 0f);
	}
}
