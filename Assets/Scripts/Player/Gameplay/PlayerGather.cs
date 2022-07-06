using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerGather : MonoBehaviour
{
    bool isGathering = false;
    float timeGathered = 0f;

    [SerializeField] Image foreground;

    [SerializeField] TMPro.TextMeshProUGUI woodDisplay;
    [SerializeField] TMPro.TextMeshProUGUI stoneDisplay;

    public List<int> resources = new List<int>();
    
    public void StartGathering(GatherableCellEntity resource) 
    {
        if (isGathering)
            return;

        isGathering = true;
        foreground.transform.parent.gameObject.SetActive(true);
        StartCoroutine(Gather(resource));
    }

    public void Start()
    {
        foreground.transform.parent.gameObject.SetActive(false);
        foreach (ScriptableResource resource in Resources.handler.Resources) 
        {
            resources.Add(0);
        }
    }
    IEnumerator Gather(GatherableCellEntity resource)
    {
        while (timeGathered <= resource.gatheringTime && isGathering && !resource.Empty) 
        {
            foreground.rectTransform.localScale = new Vector2 ( timeGathered / resource.gatheringTime, foreground.rectTransform.localScale.y);
            timeGathered += Time.deltaTime;
            yield return null;
        }

        if (isGathering && !resource.Empty) 
        {
            Debug.Log("Gathered " + Resources.handler.IdToResource(resource.ResourceID));
            //Add resourceID To inventory
            resources[resource.ResourceID]++;
            resource.Decrement();
            RefreshUI();
        }

        timeGathered = 0f;
        isGathering = false;
        foreground.transform.parent.gameObject.SetActive(false);
    }

    public void RefreshUI() 
    {
        woodDisplay.text  = resources[0].ToString();
        stoneDisplay.text = resources[1].ToString();
    }
}
