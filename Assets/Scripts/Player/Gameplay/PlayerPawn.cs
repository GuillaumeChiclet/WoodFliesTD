using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerController))]
public class PlayerPawn : MonoBehaviour
{
    //  Variables

    public ScriptableDrone drone;

    private Rigidbody rb;

    private PlayerController controller;

    private float dashElapsedCooldown = 0.0f;
    private float dashElapsed = 0.0f;

    private GameObject model;

    [Header("Movement")]
    public float moveSpeed = 5.0f;
    public float acceleration = 100.0f;
    public float maxAcceleration = 250.0f;
    public AnimationCurve accelerationFactorFromDot;
    public AnimationCurve maxAccelerationFactorFromDot;
    public float rotSpeed = 50.0f;

    [Header("Height")]
    public float flyHeight = 5.0f;

    private Vector3 goalVelocity;
    private Vector3 forceScale = new Vector3(1, 0, 1);

    //   MonoBehaviour Functions

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        model = Instantiate<GameObject>(drone.model, transform);

        rb = GetComponent<Rigidbody>();

        if (TryGetComponent<PlayerController>(out PlayerController cont))
        {
            controller = cont;
        }
    }

    private void OnDisable()
    {
        if(model) Destroy(model);
        if(rb)    Destroy(rb);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDash();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        RotateToFaceDirection();
    }

    //  Functions

    private void UpdateMovement()
    {
        Vector3 velocityDir = rb.velocity;
        Vector3 moveDir = Quaternion.AngleAxis(45, Vector3.up) * controller.MoveDirection;

        float velocityDot = Vector3.Dot(moveDir, velocityDir);
        float accel = acceleration * accelerationFactorFromDot.Evaluate(velocityDot);

        Vector3 goalVel = moveDir * drone.speed;
        goalVelocity = Vector3.MoveTowards(goalVelocity, goalVel, accel * Time.fixedDeltaTime);

        Vector3 neededAccel = (goalVelocity - rb.velocity) / Time.fixedDeltaTime;
        float maxAccel = maxAcceleration *  maxAccelerationFactorFromDot.Evaluate(velocityDot);
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);

        Vector3 neededForce = neededAccel * rb.mass;

        rb.AddForce(Vector3.Scale(neededForce, forceScale));
    }

    private void RotateToFaceDirection()
    {
        Vector3 velocityDir = rb.velocity.normalized;
        float angle = Vector3.SignedAngle(-model.transform.right, velocityDir, Vector3.up);
        model.transform.rotation *= Quaternion.AngleAxis(angle * Time.deltaTime * rotSpeed, Vector3.up);
    }

    private void UpdateDash()
    {
        //  Cooldown of the dash
        if (dashElapsedCooldown < drone.dashCooldown)
        {
            dashElapsedCooldown += Time.deltaTime;

            //  Cooldown not reached yet so return from here
            return;
        }

        bool dashInputIsDown = controller.dashInput > 0.0f;

        //  If dash input is pressed and dash elapsed time did not reach the maximum dash duration then apply the dash
        if (dashInputIsDown && dashElapsed < drone.dashMaxDuration)
        {
            rb.AddForce(rb.velocity.normalized * drone.dashpower * controller.dashInput);
            dashElapsed += Time.deltaTime;

            return;
        }

        //  If a dash occured reset the elapsed time counters
        if (dashElapsed != 0.0f)
        {
            //  Avoid dash spamming so only reset dashElapsed if dash input is not down
            if (dashInputIsDown) return;
                
            dashElapsed = 0.0f;
            dashElapsedCooldown = 0.0f;
        }
    }
}
