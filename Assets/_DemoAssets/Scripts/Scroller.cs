using UnityEngine;
using System.Collections;

public class Scroller : MonoBehaviour {
	
	public float ScrollSpeedNormal = 2.0f;
	public float ScrollSpeedBonusMax = 3.0f;
	
	private float scrollSpeed = 2.0f;
	private float scrollSpeedBonus = 1.0f;
	private float scrollTimeTotal = 0.0f;

	private string bonusData = "";
	private bool gotBonusBall = false;

	public string BonusData {
		get {
			return bonusData;
		}
	}

	void Awake() {
		enabled = false;
		scrollTimeTotal = 0.0f;
		gotBonusBall = false;
	}

	// Use this for initialization
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		scrollTimeTotal += Time.deltaTime;

		if (gotBonusBall) {
			gotBonusBall = false;
			scrollSpeedBonus = ScrollSpeedBonusMax;

			bonusData += scrollTimeTotal + "|";
		}

		if (scrollSpeedBonus > 1.0f) {
			scrollSpeedBonus = Mathf.Lerp (scrollSpeedBonus, 1.0f, Time.deltaTime);
		}

		scrollSpeed = scrollSpeedBonus * ScrollSpeedNormal;

		transform.Translate (Vector3.right * Time.deltaTime * scrollSpeed);
	}

	public void OnTapToPlay() {
		enabled = true;
		scrollTimeTotal = 0.0f;
	}

	public void OnGetBonusBall() {
		gotBonusBall = true;
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
	private void Reset() {
		enabled = false;
		scrollTimeTotal = 0.0f;
		gotBonusBall = false;
	}
}
