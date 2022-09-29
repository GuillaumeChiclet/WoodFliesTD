using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutlipleCameraTarget : MonoBehaviour
{
    public List<GameObject> players; 
    
    [SerializeField] private Camera cam;

    private Vector3 m_targetPosition;
    [SerializeField] private float m_defaultZoom = 4f;
    [SerializeField] private float m_minZoom = 25f;
    [SerializeField] private float m_zoomFactor = 0.6f;
    [SerializeField] private float m_zoom = 1f;


    void Awake()
    {
        players = GameInstance.Instance.playerConfigs.GetPlayersObjects();
    }


    void Update()
    {
        ComputeTargetPosition();

        transform.position = m_targetPosition;

        float targetedZoom = Mathf.Clamp(m_defaultZoom + m_zoomFactor * m_zoom, m_defaultZoom, m_minZoom);
        cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetedZoom, .1f);
    }

    private void ComputeTargetPosition()
    {
        Vector3 posSum = Vector3.zero;
        float maxDist = 0f;
        foreach (GameObject go in players)
        {
            posSum += go.transform.position;

            float dist = Vector3.Distance(transform.position, go.transform.position);

            if (dist > maxDist) maxDist = dist;
        }

        m_zoom = maxDist;
        m_targetPosition = posSum / players.Count;
    }
}
