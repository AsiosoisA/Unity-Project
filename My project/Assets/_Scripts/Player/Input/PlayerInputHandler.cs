﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region Componenets
    private PlayerInput playerInput;
    private Camera cam;
    #endregion

    #region Input Properties
    public Vector2 RawMovementInput { get; private set; }
    public Vector2 RawDashDirectionInput { get; private set; }
    public Vector2Int DashDirectionInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool GrabInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }
    public bool[] AttackInputs { get; private set; }
    public bool InteractionInput { get; private set; }
    public bool SubActionInput { get; private set; }
    public bool SubActionInputStop { get; private set; }
    public bool Skill1Input { get; private set; }
    public bool canSkill1 {  get; private set; }
    public bool Skill2Input { get; private set; }
    public bool canSkill2 { get; private set; }
    #endregion

    #region Input Control Variables
    [SerializeField]
    private float inputHoldTime = 5f;

    private float jumpInputStartTime;
    private float dashInputStartTime;
    private float InteractionInputStartTime;
    private float subActionInputStartTime;
    private float skill1InputStartTime;
    private float skill1Cooldown = 2f;
    private float skill2InputStartTime;
    private float skill2Cooldown = 2f;

    #endregion

    #region Unity Callback Functions
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        AttackInputs = new bool[count];
        SubActionInputStop = false;
        canSkill1 = true;
        cam = Camera.main;
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
        CheckSkill1CoolDown();
        CheckSkill2CoolDown();
    }
    #endregion

    #region Input Handle Function
    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputs[(int)CombatInputs.primary] = true;
        }

        if (context.canceled)
        {
            AttackInputs[(int)CombatInputs.primary] = false;
        }
    }

    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputs[(int)CombatInputs.secondary] = true;
        }

        if (context.canceled)
        {
            AttackInputs[(int)CombatInputs.secondary] = false;
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = Mathf.RoundToInt(RawMovementInput.x);
        NormInputY = Mathf.RoundToInt(RawMovementInput.y);       
        
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GrabInput = true;
        }

        if (context.canceled)
        {
            GrabInput = false;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirectionInput = context.ReadValue<Vector2>();

        if(playerInput.currentControlScheme == "Keyboard")
        {
            RawDashDirectionInput = cam.ScreenToWorldPoint((Vector3)RawDashDirectionInput) - transform.position;
        }

        DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
    }

    public void OnInterActionInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            InteractionInput = true;
        }

        if (context.canceled)
        {
            InteractionInput = false;
        }
    }
    
    public void OnSubActionInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SubActionInput = true;
            SubActionInputStop = false;
            subActionInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            SubActionInputStop = true;
        }
    }

    public void OnSkill1Input(InputAction.CallbackContext context)
    {
        if (context.started && canSkill1)
        {
            canSkill1 = false;
            Skill1Input = true;
            skill1InputStartTime = Time.time;
        }

        if (context.canceled)
        {
            Skill1Input = false;
        }
    }

    public void OnSkill2Input(InputAction.CallbackContext context)
    {
        if (context.started && canSkill2)
        {
            canSkill2 = false;
            Skill2Input = true;
            skill2InputStartTime = Time.time;
        }

        if (context.canceled)
        {
            Skill2Input = false;
        }
    }

    public void UseJumpInput() => JumpInput = false;

    public void UseDashInput() => DashInput = false;

    public void UseInteractionInput() => InteractionInput = false;

    public void UseSkill1Input() => Skill1Input = false;

    public void UseSkill2Input() => Skill2Input = false;
    #endregion

    #region Check Functions
    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        if(Time.time >= dashInputStartTime + inputHoldTime)
        {
            DashInput = false;
        }
    }

    private void CheckSkill1CoolDown()
    {
        if(!canSkill1 && Time.time >= skill1InputStartTime + skill1Cooldown)
        {
            canSkill1 = true;
        }
    }

    private void CheckSkill2CoolDown()
    {
        if (!canSkill2 && Time.time >= skill2InputStartTime + skill2Cooldown)
        {
            canSkill2 = true;
        }
    }
    #endregion
}

public enum CombatInputs
{
    primary,
    secondary
}
