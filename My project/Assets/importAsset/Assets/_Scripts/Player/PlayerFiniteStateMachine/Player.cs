using System;
using System.Collections;
using System.Collections.Generic;
using Bardent.CoreSystem;
using Bardent.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }
    public PlayerSuperJumpState SuperJumpState { get; private set; }
    public PlayerCarveState CarveState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }
    public PlayerWindKnockbackSkillState WindKnockbackSkillState { get; private set; }
    public PlayerInteractState InteractState {get; private set;}

    public Inventory inventory;
    public static Item[] inventoryToFoodStuff = new Item[10];
    public int iTFIndex = 0;


    #region 유석 추가부분
    public RestaurantInventory restaurantInventory {get; set;}
    public MinigameManager minigameManager;
    public SO_MinigameData carveMinigameData; // 정말정말정말 이러면 안 되는건데 일단 시연을 위해 때려박음.
    public int SwordMastery{get; private set;} // 나중에 동일 역할을 하는 변수 추가될시 묻지도 따지지도 말고 가차없이 삭제할 것.
    public void SliceLevelUp() => SwordMastery++; // 마찬가지로 삭제할 것.
    #endregion


    [SerializeField]
    private PlayerData playerData;  
    #endregion

    #region Components
    public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }

    public Stats Stats { get; private set; }
    
    public InteractableDetector InteractableDetector { get; private set; }

    private PlayerInput playerInput; //임시로 추가
    #endregion

    #region Other Variables         

    private Vector2 workspace;

    private Weapon primaryWeapon;
    private Weapon secondaryWeapon;
    
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        primaryWeapon = transform.Find("PrimaryWeapon").GetComponent<Weapon>();
        secondaryWeapon = transform.Find("SecondaryWeapon").GetComponent<Weapon>();
        
        primaryWeapon.SetCore(Core);
        secondaryWeapon.SetCore(Core);

        Stats = Core.GetCoreComponent<Stats>();
        InteractableDetector = Core.GetCoreComponent<InteractableDetector>();
        
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
        DashState = new PlayerDashState(this, StateMachine, playerData, "inAir");
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");
        SuperJumpState = new PlayerSuperJumpState(this, StateMachine, playerData, "superJump");
        CarveState = new PlayerCarveState(this, StateMachine, playerData, "carve");
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack", primaryWeapon, CombatInputs.primary);
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack", secondaryWeapon, CombatInputs.secondary);
        WindKnockbackSkillState = new PlayerWindKnockbackSkillState(this, StateMachine, playerData, "idle");
        InteractState = new PlayerInteractState(this, StateMachine, playerData, "idle");

        if (SceneManager.GetActiveScene().name == "RestaurantStartScene")
        {
            Debug.Log("레스토랑 씬");
        }
        else
            inventory = GameObject.Find("Inventory").GetComponent<Inventory>(); //일단 이렇게 설정해놨긴 했는데 추후에 자원 덜 소모하는 쪽으로 업데이트 해놓겠습니다..

        playerInput = GetComponent<PlayerInput>();









        #region 유석 추가
        restaurantInventory = GetComponentInChildren<RestaurantInventory>();
        #endregion
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();

        InputHandler.OnInteractInputChanged += InteractableDetector.TryInteract;
        
        RB = GetComponent<Rigidbody2D>();
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");
        MovementCollider = GetComponent<BoxCollider2D>();
        

        /*
        Stats.Poise.OnCurrentValueZero += HandlePoiseCurrentValueZero;
        */
        StateMachine.Initialize(IdleState);
    }

    /*
    private void HandlePoiseCurrentValueZero()
    {
        StateMachine.ChangeState(PlayerStunState);
    }
    */

    private void Update()
    {
        //테스트용
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            for (int i = 0; i < 3; i++)
                Debug.Log(inventoryToFoodStuff[i].name);
        }

        //테스트용
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    /*
    private void OnDestroy()
    {
        Stats.Poise.OnCurrentValueZero -= HandlePoiseCurrentValueZero;
    }\
    */

    #endregion

    #region Other Functions

    public void SetColliderHeight(float height)
    {
        Vector2 center = MovementCollider.offset;
        workspace.Set(MovementCollider.size.x, height);

        center.y += (height - MovementCollider.size.y) / 2;

        MovementCollider.size = workspace;
        MovementCollider.offset = center;
    }   

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimtionFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

   
    // 일단 이렇게 추가했습니다.
    public void playerInputDisable()
    {
        playerInput.enabled = false;
    }

    public void playerInputEnable()
    {
        playerInput.enabled = true;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (collision.tag == "machine")
            {
                foreach (GameObject slot in inventory.slots)
                {
                    InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
                    if (inventorySlot.item == null)
                        continue;
                    if (inventorySlot.item.itemType == Item.ItemType.ingredients)
                    {
                        inventoryToFoodStuff[iTFIndex++] = inventorySlot.item;
                        inventorySlot.ClearSlot();
                    }    
                }
            }
        }
    }
    #endregion
}
