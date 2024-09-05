using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public event Action<bool> OnInteractInputChanged;

    private PlayerInput playerInput;
    private Camera cam;

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
    public bool canSkill1 { get; private set; }
    public bool Skill2Input { get; private set; }
    public bool canSkill2 { get; private set; }

    [SerializeField]
    private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;
    private float dashInputStartTime;
    private float skill1InputStartTime;
    private float skill1Cooldown = 2f;
    private float skill2InputStartTime;
    private float skill2Cooldown = 2f;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        AttackInputs = new bool[count];
        SubActionInputStop = true;
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

    /*
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnInteractInputChanged?.Invoke(true);
            return;
        }

        if (context.canceled)
        {
            OnInteractInputChanged?.Invoke(false);
        }
    }
    */
    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started && SceneManager.GetActiveScene().buildIndex != 1)
        {
            AttackInputs[(int)CombatInputs.primary] = true;
        }

        if (context.canceled)
        {
            AttackInputs[(int)CombatInputs.primary] = false;
        }
    }

    /*
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
    무기 안 써서 주석 처리 했습니다(우클릭 시 배정 안된 무기에 대한 오류 발생 예방
    */

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

        if (playerInput.currentControlScheme == "Keyboard")
        {
            //일단 주석처리 해놓음 
            //RawDashDirectionInput = cam.ScreenToWorldPoint((Vector3)RawDashDirectionInput) - transform.position;
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

    /// <summary>
    /// Used to set the specific attack input back to false. Usually passed through the player attack state from an animation event.
    /// </summary>
    public void UseAttackInput(int i) => AttackInputs[i] = false;

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        if (Time.time >= dashInputStartTime + inputHoldTime)
        {
            DashInput = false;
        }
    }

    private void CheckSkill1CoolDown()
    {
        if (!canSkill1 && Time.time >= skill1InputStartTime + skill1Cooldown)
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


}

public enum CombatInputs
{
    primary,
    secondary
}
