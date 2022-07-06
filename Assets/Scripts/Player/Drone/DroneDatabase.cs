using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DroneDatabase", menuName = "Assets/Databases/DroneDataBase")]
public class DroneDatabase : ScriptableObject
{
    #region Variables

    public List<ScriptableDrone> allDrones;

    #endregion Variables
}
