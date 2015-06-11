using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Make my scriptable object.
/// </summary>
public class MakeMyScriptableObject : MonoBehaviour {
	[MenuItem("Assets/Create/Spawned Points Scriptable Object")]
	public static void CreateSpawnedPointsScriptableObject() {
		SpawningPoints asset = ScriptableObject.CreateInstance<SpawningPoints> ();

		AssetDatabase.CreateAsset (asset, "Assets/_DemoAssets/ScriptableObjects/SpawnedPoints.asset");
		AssetDatabase.SaveAssets ();

		EditorUtility.FocusProjectWindow ();

		Selection.activeObject = asset;
	}
	
	[MenuItem("Assets/Create/Spawned Item Scriptable Object")]
	public static void CreateSpawnedItemScriptableObject() {
		SpawnedItem asset = ScriptableObject.CreateInstance<SpawnedItem> ();
		
		AssetDatabase.CreateAsset (asset, "Assets/_DemoAssets/ScriptableObjects/SpawnedItem.asset");
		AssetDatabase.SaveAssets ();
		
		EditorUtility.FocusProjectWindow ();
		
		Selection.activeObject = asset;
	}
}
