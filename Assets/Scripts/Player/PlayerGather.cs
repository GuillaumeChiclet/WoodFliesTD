using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGather : MonoBehaviour
{
    bool isGathering = false;
    float timeGathered = 0f;

    [SerializeField] Image foreground;
    
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
            resource.Decrement();
        }

        timeGathered = 0f;
        isGathering = false;
        foreground.transform.parent.gameObject.SetActive(false);
    }
}
