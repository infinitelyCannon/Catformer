using UnityEngine;
using UnityEditor;

/*
[CustomEditor(typeof(ParticleSystem))]
public class ParticleSaverEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		//base.DrawDefaultInspector();

		GUILayout.BeginHorizontal();

		if(GUILayout.Button("Save Settings"))
		{
			string path = EditorUtility.SaveFilePanel("Save Particle Settings", "Assets/", "Particle.txt", "txt");

			Debug.Log("GOT: " + path);
		}

		GUILayout.EndHorizontal();
	}
}
*/