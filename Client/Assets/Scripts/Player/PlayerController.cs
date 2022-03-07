using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : IsMine
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

    public float verticalVelocity;
    public float x, y;
    public Vector2 input;
    public Vector3 _velocity, movedir;
    public bool _lower = false;
    public bool _shift = false;
    public bool isAir = false;
    public float currentVelocity;
    public float WalkVelocity = 4.5f;
    public float LowerVelocity = 3.5f;
    public float WeaponWeight = 2f;
    public float JumpForce = 2f;
    public float DownForce = 1f;
    public float AereoSpeed = 3f;
    public float TimeAereoSpeed = 0f;
    public float speedtest = 1f;
    [Space]
    [Header("Character Controller FLOATS")]
    public float inLower = 1f;
    public float notLower = 1.68f;

    [Space]
    public int testa = 0;
    public float JumpVeloc = 2f;
    [Space]
    CharacterController characterController;
    AnimController animController;
    PlayerManager _playerManager;
    PlayerNetwork _playerNetwork;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animController = GetComponent<AnimController>();
        _playerManager = GetComponent<PlayerManager>();
        _playerNetwork = GetComponent<PlayerNetwork>();
        if (!isMine())
        {
            _playerManager.DesactiveItemsFPV();
            characterController.enabled = false;
            this.enabled = false;
        }
        else
        {
            _playerManager.DesactiveItemsTPV();
        }

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!isMine())
            return;

        Invoke("SendWalk", 0.1f);

        if (Pvp_ChatController.instance.PVP_InputText.IsActive())
        {
            x = 0;
            y = 0;
            return;
        }

        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        _lower = Input.GetKey(KeyCode.LeftControl);
        _shift = Input.GetKey(KeyCode.LeftShift);

        input = new Vector2(x, y);


        if (_lower)
        {
            characterController.height = 1.1f;
            if (characterController.isGrounded)
                _playerManager._FPSMainObject.transform.localPosition = Vector3.Lerp(_playerManager._FPSMainObject.transform.localPosition, new Vector3(0, -0.6f, 2), 3f);
        }
        else
        {
            characterController.height = 1.68f;
            _playerManager._FPSMainObject.transform.localPosition = Vector3.Lerp(_playerManager._FPSMainObject.transform.localPosition, new Vector3(0, -0.3f, 2), 3f);
        }

        CheckInputs();

        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            verticalVelocity = jumpSettings.jumpSpeed;
        }

        UpdateGravity();

        if (characterController.isGrounded)
            UpdateXZMovement(x, y);

        characterController.Move(movedir * Time.deltaTime);
        movedir = characterController.velocity;

        if (x != 0 || y != 0)
        {
            UICrossHair.instance.walking = true;
        }
        else
        {
            UICrossHair.instance.walking = false;
        }

        CheckSpeed();
        speed = currentVelocity;
        //speed = _lower == true || _shift == true ? LowerVelocity : WalkVelocity;
    }

    void CheckSpeed()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isAir && testa == 0 && testa != 2)
        {
            testa = 1;
            currentVelocity = WalkVelocity;
        }
        else if (_lower && !isAir && testa == 0 && testa != 1)
        {
            testa = 2;
            currentVelocity = LowerVelocity;
        }
        else
        {
            if (_shift && testa != 2 && testa != 1 && !isAir)
                currentVelocity = LowerVelocity;
            else if (testa == 0)
                currentVelocity = WalkVelocity;
        }
    }

    private void FixedUpdate()
    {
        /* if (!isAir)
         {
             testa = 0;
             _velocity = (transform.forward * input.y) + (transform.right * input.x);
             TimeAereoSpeed = 0;
         }
         else
         {
             if (TimeAereoSpeed > 3)
             {
                 float inp = input.normalized.x > 0 ? AereoSpeed : input.normalized.x < 0 ? -AereoSpeed : 0;
                 _velocity += (transform.right.normalized * inp) * Time.fixedDeltaTime;
             }
             else
                 TimeAereoSpeed++;
         }

         movedir.x = _velocity.normalized.x;
         movedir.z = _velocity.normalized.z;
        */
    }

    void SendWalk()
    {
        animController.CheckWalk(x, y, _playerManager._cameraController.verticalRotation.ToString(), _lower, _shift);
    }

    void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _playerNetwork.SendChangeWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            _playerNetwork.SendChangeWeapon(2);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            _playerNetwork.SendChangeWeapon(3);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            _playerNetwork.SendChangeWeapon(4);
    }

    private void UpdateXZMovement(float horizontalInput, float verticalInput)
    {
        Vector3 to = Vector3.zero;
        to = CalculateDesiredVelocity(horizontalInput, verticalInput);
        to.y = 0f;

        if (Mathf.Abs(_velocity.magnitude) > 0f && Mathf.Abs(to.magnitude) < 0.1f)
        {
            SetAndLerpRealMovedDirection(_velocity, to, 0.1f);
        }
        else if (characterController.isGrounded)
        {
            SetAndLerpRealMovedDirection(_velocity, to, 0.3f);
        }
    }

    private void UpdateGravity()
    {
        if (!characterController.isGrounded)
        {
            //Ray ray = new Ray(transform.position, Vector3.up);
            //if (Physics.Raycast(ray, out var hitInfo, 1.2f))
            //{
            //    Debug.LogError(hitInfo.collider.name);
            //    verticalVelocity = 0;
            //}
            verticalVelocity += jumpSettings.globalGravity * Time.deltaTime;
            if (verticalVelocity < -55f)
            {
                verticalVelocity = -55f;
            }
        }
        movedir.y = verticalVelocity;
    }

    private Vector3 CalculateDesiredVelocity(float horizontalInput, float verticalInput)
    {
        Vector3 direction;
        direction = Vector3.ClampMagnitude(new Vector3(horizontalInput, 0f, verticalInput), 1f);
        Vector3 normalized = base.transform.TransformDirection(direction).normalized;
        normalized.y = 0f;
        normalized = normalized.normalized;
        direction = base.transform.TransformDirection(direction).normalized;

        if (characterController.isGrounded)
        {
            float num2 = 1f;
            float num = CalculateDirectionRatio(verticalInput);
            direction *= groundSettings.moveSpeed * num2 * num * movementSettings.weaponRatio;
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
            movedir = x;
        }, to, interpolateTime).SetId("RealMovedDirection");
        return movedir;
    }

    internal float _speed = 0;
    public float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value - WeaponWeight;
            _speed = Mathf.Clamp(_speed, 2, 10);
        }
    }

}
