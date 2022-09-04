using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Drone", menuName = "Assets/Scriptable Drone", order = 1)]
public class ScriptableDrone : ScriptableObject
{
    public float speed = 5.0f;
    public float dashpower = 5.0f;

    public float dashCooldown    = 0.5f;
    public float dashMaxDuration = 0.5f;

    //  Efficiencies
    public float extractionEfficiency = 1.0f;
    public float craftingEfficiency   = 1.0f;

    public int itemSlot = 3;

    public GameObject model = null;

}
