
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Resource", menuName = "Assets/Scriptable Resource", order = 0)]
public class ScriptableResource : ScriptableObject
{
    public string displayName   = "DefaultResource";
    public Sprite sprite;

    [SerializeField]
    private List<GameObject> models;

    [HideInInspector] public int ID = -1;

    public GameObject ConstructResourceInstance(Vector3 position, Quaternion rotation, Transform parent)
    {
        int index = Random.Range(0, models.Count);

        if (index >= 0)
        {
            return Instantiate(models[index], position, rotation, parent);
        }

        return null;
    }
}