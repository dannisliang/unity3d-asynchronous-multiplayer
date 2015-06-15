using UnityEngine;
using System.Collections;

public class Scroller : MonoBehaviour {

	public float ScrollSpeedMin = 1.0f;
	public float ScrollSpeedMax = 4.0f;
	public float ScrollTimeMax = 600.0f;
	public float ScrollSpeedBonusMax = 5.0f;

	private float scrollSpeed = 1.0f;
	private float scrollTimeTotal = 0.0f;
	private float scrollSpeedBonus = 1.0f;

	// Use this for initialization
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		scrollSpeed = ScrollSpeedMin;
		scrollTimeTotal = 0.0f;
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		scrollTimeTotal += Time.deltaTime;
		if (scrollSpeedBonus > 1.0f) {
			scrollSpeedBonus = Mathf.Lerp (scrollSpeedBonus, 1.0f, Time.deltaTime);
			Debug.Log ("scrollSpeedBonus = " + scrollSpeedBonus);
		}

		scrollSpeed = scrollSpeedBonus * Mathf.Lerp (ScrollSpeedMin, ScrollSpeedMax, scrollTimeTotal/ScrollTimeMax);

		transform.Translate (Vector3.right * Time.deltaTime * scrollSpeed);
	}

	public void OnTapToPlay() {
		enabled = true;
	}

	public void OnGetBonusBall() {
		scrollSpeedBonus = ScrollSpeedBonusMax;
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
		scrollSpeed = ScrollSpeedMin;
		scrollTimeTotal = 0.0f;
	}
}
