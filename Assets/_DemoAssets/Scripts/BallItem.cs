using UnityEngine;
using System.Collections;

public class BallItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		GameManager.Instance.OnDragonGetBall ();
	}
}
