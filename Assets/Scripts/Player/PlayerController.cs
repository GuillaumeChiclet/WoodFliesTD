using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Model")]
    public GameObject model;
    public GameObject decalTarget;

    [Header("Movement")]
    public float moveSpeed = 5.0f;
    public float acceleration = 100.0f;
    public float maxAcceleration = 250.0f;
    public AnimationCurve accelerationFactorFromDot;
    public AnimationCurve maxAccelerationFactorFromDot;
    public float rotSpeed = 50.0f;

    [Header("Height")]
    public float flyHeight = 5.0f;

    [Header("Layers")]
    public LayerMask terrainLayerMask;



    Rigidbody rb;
    Vector3 inputDir;
    Vector3 goalVelocity;

    Vector3 forceScale = new Vector3(1, 0, 1);

    public Map map = null;
    Cell currentCellBelow = null;
    public Vector2Int currentCellBelowGridPos;

    public Cell CurrentCellBelow => currentCellBelow;
    Vector3 hitPoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("MapManager").GetComponent<Map>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputDir();

        if (Input.GetKeyDown(KeyCode.E)) 
        {
            currentCellBelow.ownedEntity?.GetComponent<IInteractable>().PrimarAction(gameObject);
        }
    }

    private void FixedUpdate()
    {
        UpdateCellBelow();

        UpdateMovement();
        RotateToFaceDirection();
        UpdateDecalTarget();
    }

    void UpdateCellBelow()
    {
        bool touch = Physics.SphereCast(gameObject.transform.position, 0.1f, Vector3.down, out RaycastHit hit, 50, terrainLayerMask);
        if (touch)
        {
            hitPoint = hit.point;
            map.TryGetCellFromWorldPos(hitPoint, out currentCellBelow);
            int x = 0, y = 0;
            MapCoordinates.WorldToCellCoords(hitPoint, ref currentCellBelowGridPos);
        }
    }

    void UpdateDecalTarget()
    {
        if (currentCellBelow == null)
            return;

        int x = 0, y = 0;
        MapCoordinates.WorldToCellCoords(hitPoint, ref x, ref y);
        //decalTarget.transform.position = Vector3.MoveTowards(decalTarget.transform.position, new Vector3(x * MapCoordinates.unitSize, currentCellBelow.height, y * MapCoordinates.unitSize), Time.deltaTime * 5.0f);
        decalTarget.transform.position = new Vector3(x * MapCoordinates.unitSize, currentCellBelow.height, y * MapCoordinates.unitSize);
    }

    void UpdateInputDir()
    {
        inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        inputDir = inputDir.magnitude > 1.0f ? inputDir.normalized : inputDir;
        inputDir = Quaternion.AngleAxis(45, Vector3.up) * inputDir;
    }


    void UpdateMovement()
    {
        Vector3 currentVelocity = rb.velocity;
        Vector3 velocityDir = rb.velocity;

        float velocityDot = Vector3.Dot(inputDir, velocityDir);
        float accel = acceleration * accelerationFactorFromDot.Evaluate(velocityDot);

        Vector3 goalVel = inputDir * moveSpeed;
        goalVelocity = Vector3.MoveTowards(goalVelocity, goalVel, accel * Time.fixedDeltaTime);

        Vector3 neededAccel = (goalVelocity - rb.velocity) / Time.fixedDeltaTime;
        float maxAccel = maxAcceleration * maxAccelerationFactorFromDot.Evaluate(velocityDot);
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);

        Vector3 neededForce = neededAccel * rb.mass;

        rb.AddForce(Vector3.Scale(neededForce,forceScale));
    }

    void RotateToFaceDirection()
    {
        Vector3 velocityDir = rb.velocity.normalized;
        float angle = Vector3.SignedAngle(-model.transform.right, velocityDir, Vector3.up);
        model.transform.rotation *= Quaternion.AngleAxis(angle * Time.deltaTime * rotSpeed, Vector3.up);
    }
}
