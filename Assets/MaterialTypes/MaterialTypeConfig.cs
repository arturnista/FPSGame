using UnityEngine;

[System.Serializable]
public class MaterialTypeConfig {

    public string name;
    [Range(0f, 1f)]
    public float penetration = 1f;
    public AudioClip[] impactAudios;
    public AudioClip[] stepsAudios;
    public GameObject[] bulletHoleDecals;

}
