﻿using Unity.Netcode;
using UnityEngine;


public class PlayerCharacter : Character, IHealth
{
    [Range(0, 100)][SerializeField] private int health = 100;
    [Range(0.5f, 10.0f)][SerializeField] private float movingSpeed = 8.0f;
    [SerializeField] private float acceleration = 3.0f;

    private const float gravity = -9.8f;
    private CharacterController characterController;
    private MouseLook mouseLook;
    private Vector3 currentVelocity;

    protected override FireAction fireAction { get; set; }
    public int Health { get { return health; } set { health = value; } }

    protected override void Initiate()
    {
        base.Initiate();

        Health = health;

        fireAction = gameObject.GetComponent<RayShooter>();
        fireAction ??= gameObject.AddComponent<RayShooter>();
        fireAction.Reloading();

        characterController = GetComponentInChildren<CharacterController>();
        characterController ??= gameObject.AddComponent<CharacterController>();

        mouseLook = GetComponentInChildren<MouseLook>();
        mouseLook ??= gameObject.AddComponent<MouseLook>();
    }

    public override void Movement()
    {
        if (mouseLook != null && mouseLook.PlayerCamera != null)
        {
            mouseLook.PlayerCamera.enabled = IsOwner;
        }

        if (IsOwner)
        {
            var moveX = Input.GetAxis("Horizontal") * movingSpeed;
            var moveZ = Input.GetAxis("Vertical") * movingSpeed;
            var movement = new Vector3(moveX, 0, moveZ);
            movement = Vector3.ClampMagnitude(movement, movingSpeed);
            movement *= Time.deltaTime;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                movement *= acceleration;
            }

            movement.y = gravity;
            movement = transform.TransformDirection(movement);
            characterController.Move(movement);
            mouseLook.Rotation();
            CmdUpdatePosition(transform.position);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, serverPosition, ref currentVelocity, movingSpeed * Time.deltaTime);
        }
    }

    private void Start()
    {
        Initiate();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (Health <= 0)
        {
            Debug.Log($"Dead. Health = {Health}");
            NetworkManager.Singleton.DisconnectClient(OwnerClientId);
        }
    }

    private void OnGUI()
    {
        if (Camera.main == null)
        {
            return;
        }

        var info = $"Health: {health}\nClip: {fireAction.BulletCount}";
        var size = 12;
        var bulletCountSize = 50;
        var posX = Camera.main.pixelWidth / 2 - size / 4;
        var posY = Camera.main.pixelHeight / 2 - size / 2;
        var posXBul = Camera.main.pixelWidth - bulletCountSize * 2;
        var posYBul = Camera.main.pixelHeight - bulletCountSize;
        GUI.Label(new Rect(posX, posY, size, size), "+");
        GUI.Label(new Rect(posXBul, posYBul, bulletCountSize * 2, bulletCountSize * 2), info);
    }
}
