using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chiko : Entity
{
    #region States and Data
    public Chiko_MoveState moveState {  get; private set; }
    public Chiko_IdleState idleState { get; private set; }
    public Chiko_DeadState deadState { get; private set; }
    public Chiko_StunState stunState { get; private set; }
    public Chiko_PlayerDetectedState playerDetectedState { get; private set; }

    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField] 
    private D_DeadState deadStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedStateData;

    private int facingDirection = 1;
    #endregion


    public override void Awake()
    {
        base.Awake();

        moveState = new Chiko_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new Chiko_IdleState(this, stateMachine, "idle", idleStateData, this);
        stunState = new Chiko_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new Chiko_DeadState(this, stateMachine, "dead", deadStateData, this);
        playerDetectedState = new Chiko_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
    }

    private void Start()
    {
        stateMachine.Initialize(moveState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
