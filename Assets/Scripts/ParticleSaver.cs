using System.Reflection;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSaver : MonoBehaviour
{
#if UNITY_EDITOR
	[ContextMenu("Save Particle Settings")]
	public void SaveSettings()
	{
		string path = EditorUtility.SaveFilePanel("Save Particle Settings", "Assets/", "Particle.txt", "txt");
		string buffer = "";

		ParticleSystem particles = GetComponent<ParticleSystem>();
		ParticleSystemRenderer renderer = GetComponent<ParticleSystemRenderer>();

		PropertyInfo[] props = particles.GetType().GetProperties();

		foreach(var prop in props)
		{
			if (prop.PropertyType.Name.Contains("Module") && !prop.PropertyType.Name.Contains("SubEmitters"))
			{
				Debug.Log("NEW TYPE: " + prop.PropertyType.Name);
				object module = prop.GetValue(particles);
				PropertyInfo[] properties = module.GetType().GetProperties();
				foreach (var property in properties)
				{
					if (property.GetIndexParameters().Length == 0)
					{
						Debug.Log(string.Format("{0}: ({1}) {2}", property.Name, property.PropertyType.Name, property.GetValue(module)));
						
					}
					else
					{
						Debug.Log(string.Format("{0}: ({1}) <INDEXED>", property.Name, property.PropertyType.Name));
					}
				}
			}
		}

		
	}
#endif
}
