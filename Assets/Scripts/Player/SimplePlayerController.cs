using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject sphereVisual;
    public float halfHeight = 0.05f;
    public LayerMask terrainLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveInput = (new Vector3(horizontal, 0, vertical)).normalized;

        moveInput = Quaternion.AngleAxis(45, Vector3.up) * moveInput;

        transform.position += moveInput * Time.deltaTime * 5.0f;
        transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * 500.0f, Vector3.up);

        bool touch = Physics.Raycast(gameObject.transform.position, Vector3.down, out RaycastHit hit, 50, terrainLayerMask);
        if (touch)
        {
            sphereVisual.transform.position = hit.point + Vector3.up * halfHeight;
            if (Input.GetKeyDown(KeyCode.E)) 
            {
                hit.transform.GetComponent<IInteractable>()?.PrimarAction(gameObject);
            }
        }
    }
}
