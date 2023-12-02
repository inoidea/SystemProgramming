using System;
using Unity.Netcode;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public abstract class Character : NetworkBehaviour
{
    protected Action OnUpdateAction { get; set; }
    protected abstract FireAction fireAction { get; set; }

    /*[SyncVar] */protected Vector3 serverPosition;

    protected virtual void Initiate()
    {
        OnUpdateAction += Movement;
    }

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
        OnUpdateAction?.Invoke();
    }

    //[Command]
    protected void CmdUpdatePosition(Vector3 position)
    {
        serverPosition = position;
    }

    public abstract void Movement();
}
