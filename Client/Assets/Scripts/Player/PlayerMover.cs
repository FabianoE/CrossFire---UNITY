using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class PlayerMover : MonoBehaviour
{
    [Serializable]
    public class MovementRatio
    {
        public float stateRatio;

        public float behaviourRatio;

        public float sideDirectionRatio;

        public float backDirectionRatio;

        public float weaponRatio;

        public float boostRatio;
    }

    [Serializable]
    public class JumpSettings
    {
        public float jumpSpeed = 3.43f;

        public float[] landingBorderHeight = new float[2] { 0.5f, 5f };

        public float globalGravity = -9.81f;

        public float diveOffsetSpeed = 15f;

        public float parachuteYOffset = 1.5f;
    }


    [Serializable]
    public class GroundMovementSettings
    {
        public float moveSpeed = 6.25f;

        public float slopeLimit = 0.1f;

        public AnimationCurve SlopeXZSpeedModifier = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(35f, 0.75f), new Keyframe(40f, 0.5f), new Keyframe(50f, 0.25f), new Keyframe(60f, 0f), new Keyframe(90f, 0f));

        public AnimationCurve SlopeOrthogonalSpeedModifier = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(90f, 0f), new Keyframe(180f, 1f));
    }

    public JumpSettings jumpSettings = new JumpSettings();
    public GroundMovementSettings groundSettings = new GroundMovementSettings();
    public MovementRatio movementSettings = new MovementRatio();

    public float maxFallSpeed = -55f;
    private Vector3 CurrentVelocity = Vector3.zero;
    CharacterController controller;
    public float verticalVelocity;
    public float walkVelocity;
    public float checkCont = 1.5f;

    public bool readAir = false;
    public float x, y, lastY;
    private readonly float antiBumpFactor = -9.8f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        UpdateGravity();
        if (controller.isGrounded)
        {
            UpdateXZMovement(x, y);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                lastY = y;

                verticalVelocity = jumpSettings.jumpSpeed;
                if (x != 0)
                    readAir = true;
            }
        }

        controller.Move(CurrentVelocity * Time.deltaTime);
        CurrentVelocity = controller.velocity;
    }

    private void UpdateGravity()
    {
        if (!controller.isGrounded)
        {
            Ray ray = new Ray(transform.position, Vector3.up);
            if (Physics.Raycast(ray, out var hitInfo, checkCont))
            {
                Debug.LogError(hitInfo.collider.name);
                verticalVelocity = 0;
            }
            verticalVelocity += jumpSettings.globalGravity * Time.deltaTime;
            if (verticalVelocity < maxFallSpeed)
            {
                verticalVelocity = maxFallSpeed;
            }
        }
        CurrentVelocity.y = verticalVelocity;
    }

    private void UpdateXZMovement(float horizontalInput, float verticalInput)
    {
        Vector3 to = Vector3.zero;
        to = CalculateDesiredVelocity(horizontalInput, verticalInput);
        to.y = 0f;

        if (Mathf.Abs(CurrentVelocity.magnitude) > 0f && Mathf.Abs(to.magnitude) < 0.1f)
        {
            SetAndLerpRealMovedDirection(CurrentVelocity, to, 0.1f);
        }
        else if (controller.isGrounded)
        {
            SetAndLerpRealMovedDirection(CurrentVelocity, to, 0.3f);
        }
    }

    private Vector3 CalculateDesiredVelocity(float horizontalInput, float verticalInput)
    {
        Vector3 direction;
        direction = Vector3.ClampMagnitude(new Vector3(horizontalInput, 0f, verticalInput), 1f);
        Vector3 normalized = base.transform.TransformDirection(direction).normalized;
        normalized.y = 0f;
        normalized = normalized.normalized;
        direction = base.transform.TransformDirection(direction).normalized;


        if (controller.isGrounded)
        {
            float num2 = 1f;
            float num = CalculateDirectionRatio(verticalInput);
            direction *= groundSettings.moveSpeed * movementSettings.behaviourRatio * movementSettings.stateRatio * num2 * num * movementSettings.boostRatio * movementSettings.weaponRatio;
        }
        return direction;
    }

    private float CalculateDirectionRatio(float verticalInput)
    {
        float num = 1f;
        if (verticalInput > 0.05f)
        {
            return 1f;
        }
        if (verticalInput < -0.05f)
        {
            return movementSettings.backDirectionRatio;
        }
        return movementSettings.sideDirectionRatio;
    }

    private Vector3 SetAndLerpRealMovedDirection(Vector3 from, Vector3 to, float interpolateTime)
    {
        DOTween.To(() => from, delegate (Vector3 x)
        {
            CurrentVelocity = x;
        }, to, interpolateTime).SetId("RealMovedDirection");
        return CurrentVelocity;
    }
}