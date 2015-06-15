using UnityEngine;
using System.Collections;

public class FriendDragonsManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTapToPlay() {
		FriendDragonController[] friendDragonControllers = GetComponentsInChildren<FriendDragonController> ();
		foreach (var controller in friendDragonControllers) {
			controller.OnTapToPlay();
		}

		FriendScroller[] friendDragonScrollers = GetComponentsInChildren<FriendScroller> ();
		foreach (var scroller in friendDragonScrollers) {
			scroller.OnTapToPlay();
		}
	}
}
