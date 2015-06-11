using UnityEngine;
using System.Collections;

public class FloorScroller : MonoBehaviour {

	/// <summary>
	/// The scroll speed.
	/// </summary>
	public float scrollSpeed = 2.0f;

	private Renderer floorRenderer = null;
	private Vector2 savedTextureOffset;

	// Use this for initialization
	void Start () {
		floorRenderer = GetComponent<Renderer> ();

		if (floorRenderer == null) {
			Debug.LogError("No renderer component found!");
		} else {
			savedTextureOffset = floorRenderer.sharedMaterial.GetTextureOffset("_MainTex");
		}

	}
	
	// Update is called once per frame
	void Update () {
		float yOffset = Mathf.Repeat (Time.time * scrollSpeed, 1);
		Vector2 offset = new Vector2 (savedTextureOffset.x, yOffset);

		floorRenderer.sharedMaterial.SetTextureOffset ("_MainTex", offset);
	}
}
