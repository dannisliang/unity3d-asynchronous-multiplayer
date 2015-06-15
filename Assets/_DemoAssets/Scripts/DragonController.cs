using UnityEngine;
using System.Collections;

public class DragonController : MonoBehaviour {
	
	public GameObject dragonModel = null;
	// The force which is added when the player jumps
	// This can be changed in the Inspector window
	public Vector3 jumpForce = new Vector3(0, 300, 0);
	
	private float replayTimeTotal = 0.0f;
	private float lastJumpTimestamp = 0.0f;
	private Rigidbody dragonRigidbody;
	private string replayData = "";
	private bool isReplay = false;
	private float[] replayTimestamps;

	public string ReplayData {
		get {
			return replayData;
		}
		set {
			replayData = value;
		}
	}
	
	public bool IsReplay {
		get {
			return isReplay;
		}
		set {
			isReplay = value;
		}
	}


	void Awake() {
		dragonRigidbody = dragonModel.GetComponent<Rigidbody> ();
		replayData = "0.486957|1.433732|2.753847|3.788472|4.837597|6.086403|7.237501|8.505794|9.805615|10.93811|12.12536|13.45778|14.47602|15.7496|16.99405|17.95811|19.27988|20.47958";
		string[] replayTimestampSt = replayData.Split('|');
		if (replayTimestampSt.Length > 	1) {
			Debug.Log(replayTimestampSt.Length);
			replayTimestamps = new float[replayTimestampSt.Length];

			for (int i = 0; i < replayTimestamps.Length; i++) {
				float timestamp = float.Parse (replayTimestampSt [i]);
				replayTimestamps [i] = timestamp;
			}
		}
	}

	// Use this for initialization
	void Start () {
		replayTimeTotal = 0.0f;
		lastJumpTimestamp = 0.0f;

		enabled = false;
	}
	
	// Update is called once per frame
	private float playTime = 0f;
	private int currentTimestampIndex = 0;
	void Update () {
		replayTimeTotal += Time.deltaTime;

		if (isReplay && currentTimestampIndex < replayTimestamps.Length) {
			playTime += Time.deltaTime;
			if (playTime >= replayTimestamps[currentTimestampIndex]) {
				dragonRigidbody.velocity = Vector3.zero;
				dragonRigidbody.AddForce (jumpForce);

				currentTimestampIndex++;
			}
		} else {
			// Jump dragon model
			if (Input.GetMouseButtonDown (0)) {
				dragonRigidbody.velocity = Vector3.zero;
				dragonRigidbody.AddForce (jumpForce);

				lastJumpTimestamp = replayTimeTotal;
				replayData += lastJumpTimestamp + "|";
			}
		}
	}

	public void OnTapToPlay() {
		enabled = true;
	}
	
	/// <summary>
	/// Raises the dragon killed event.
	/// </summary>
	public void OnDragonKilled() {
		Reset ();
	}
	
	/// <summary>
	/// Reset this instance.
	/// </summary>
	public void Reset() {
		replayTimeTotal = 0.0f;
		lastJumpTimestamp = 0.0f;
		replayData = "";
	}
}
