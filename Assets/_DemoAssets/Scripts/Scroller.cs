using UnityEngine;
using System.Collections;

public class Scroller : MonoBehaviour {

	public float ScrollSpeedMin = 1.0f;
	public float ScrollSpeedMax = 4.0f;
	public float ScrollTimeMax = 600.0f;

	private float scrollSpeed = 1.0f;
	private float scrollTimeTotal = 0.0f;

	// Use this for initialization
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		scrollSpeed = ScrollSpeedMin;
		scrollTimeTotal = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		scrollTimeTotal += Time.deltaTime;
		scrollSpeed = Mathf.Lerp (ScrollSpeedMin, ScrollSpeedMax, scrollTimeTotal/ScrollTimeMax);

		transform.Translate (Vector3.right * Time.deltaTime * scrollSpeed);
	}

	/// <summary>
	/// Raises the dragon killed event.
	/// </summary>
	void OnDragonKilled() {
		Reset ();
	}

	/// <summary>
	/// Reset this instance.
	/// </summary>
	void Reset() {
		scrollSpeed = ScrollSpeedMin;
		scrollTimeTotal = 0.0f;
	}
}
