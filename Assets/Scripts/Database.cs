using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public AssetDatabase<ScriptableDrone> DroneDatabase;
    public AssetDatabase<ScriptableCellType>   CellTypeDatabase;
    public AssetDatabase<ScriptableResource>   ResourceDatabase;
}
