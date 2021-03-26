using UnityEngine;

[CreateAssetMenu(fileName = "PlatformData", menuName = "ScriptableObjects/Platform Data")]
public class PlatformData : ScriptableObject
{
	[Header("Transform")]
	public Vector3 Rotation;
	public Vector3 Scale = Vector3.one;

	[Header("Platform Script Values")]
	public float speed;
	public bool isGood;
	public float timeLeft;
	public bool shouldFade;

	[Header("Sprite")]
	public Sprite sprite;

	[Header("Box Collider2D Settings")]
	public Vector2 Size;

	[Header("Particle Settings")]
	public GameObject ParticlePrefab;
}
