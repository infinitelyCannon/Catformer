using System;
using System.Reflection;
using UnityEngine;

public class Test : MonoBehaviour
{
    private ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        Scan();
    }

    private void Scan()
	{
        ParticleSystem.MainModule main = particles.main;

        PropertyInfo[] props = main.GetType().GetProperties();

        foreach(var prop in props)
		{
            if(prop.GetIndexParameters().Length == 0)
			{
                Debug.Log(string.Format("{0}: ({1}) {2}", prop.Name, prop.PropertyType.Name, prop.GetValue(main)));
			}
            else
			{
                Debug.Log(string.Format("{0}: ({1}) <INDEXED>", prop.Name, prop.PropertyType.Name));
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
