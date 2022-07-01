using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class MenuLevelLoad : MonoBehaviour
{
    [SerializeField] public int gameSceneBuildIdx;
    [SerializeField] public int editorSceneBuildIdx;

    public TMP_Dropdown levelDropdown;

    private void Awake()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] fileInfo = directoryInfo.GetFiles();


        List<string> levelNames = new List<string>();

        foreach (FileInfo file in fileInfo)
        {
            levelNames.Add(file.Name.Remove(file.Name.Length - 5, 5));
        }

        levelDropdown.ClearOptions();
        levelDropdown.AddOptions(levelNames);
    }

    public void LoadLevelGame()
    {
        PlayerPrefs.SetString("sceneName", levelDropdown ? levelDropdown.captionText.text : "Level_00");

        SceneManager.LoadScene(gameSceneBuildIdx);
    }

    public void LoadLevelEditor()
    {
        PlayerPrefs.SetString("sceneName", levelDropdown ? levelDropdown.captionText.text : "Level_00");

        SceneManager.LoadScene(editorSceneBuildIdx);
    }
}
