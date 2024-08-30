using System;
using Bardent.CoreSystem;
using UnityEngine;
using Bardent.Utilities;

namespace Bardent.Weapons
{
    public class Weapon : MonoBehaviour
    {
        //Player 추가
        Player player;

        public event Action<bool> OnCurrentInputChange;

        public event Action OnEnter;
        public event Action OnExit;
        public event Action OnUseInput;

        [SerializeField] private float attackCounterResetCooldown;

        public bool CanEnterAttack { get; private set; }
        
        public WeaponDataSO Data { get; private set; }

        public int CurrentAttackCounter
        {
            get => currentAttackCounter;
            private set => currentAttackCounter = value >= Data.NumberOfAttacks-2 ? 0 : value;
        }

        public bool CurrentInput
        {
            get => currentInput;
            set
            {
                if (currentInput != value)
                {
                    currentInput = value;
                    OnCurrentInputChange?.Invoke(currentInput);
                }
            }
        }

        public float AttackStartTime { get; private set; }

        public Animator Anim { get; private set; }
        public GameObject BaseGameObject { get; private set; }
        public GameObject WeaponSpriteGameObject { get; private set; }

        public AnimationEventHandler EventHandler
        {
            get
            {
                if (!initDone)
                {
                    GetDependencies();
                }

                return eventHandler;
            }
            private set => eventHandler = value;
        }

        public Core Core { get; private set; }

        private int currentAttackCounter;

        private TimeNotifier attackCounterResetTimeNotifier;

        private bool currentInput;

        private bool initDone;
        private AnimationEventHandler eventHandler;

        public void Enter()
        {
            AttackStartTime = Time.time;
            attackCounterResetTimeNotifier.Disable();
            //여기서 부터
            if (player.StateMachine.previousState == player.CrouchIdleState)
            {
                player.StateMachine.previousState = player.IdleState;
                Debug.Log("앉기 공격 실행");
                currentAttackCounter = 3;
            }
            else if (player.StateMachine.previousState == player.InAirState)
            {
                player.StateMachine.previousState = player.IdleState;
                currentAttackCounter = 4;
            }
            Anim.SetInteger("counter", currentAttackCounter);
            Anim.SetBool("active", true);
            OnEnter?.Invoke();
            // Debug.Break();
            //print($"{transform.name} enter");
        }

        public void SetCore(Core core)
        {
            Core = core;
        }

        public void SetData(WeaponDataSO data)
        {
            Data = data;
            
            if(Data is null)
                return;
            
            ResetAttackCounter();
        }

        public void SetCanEnterAttack(bool value) => CanEnterAttack = value;

        public void Exit()
        {
            Anim.SetBool("active", false);

            CurrentAttackCounter++;
            attackCounterResetTimeNotifier.Init(attackCounterResetCooldown);

            OnExit?.Invoke();
        }

        private void Awake()
        {
            GetDependencies();

            attackCounterResetTimeNotifier = new TimeNotifier();
            // 여기부터 추가
            player = transform.GetComponentInParent<Player>(); 
        }

        private void GetDependencies()
        {
            if (initDone)
                return;

            BaseGameObject = transform.Find("Base").gameObject;
            WeaponSpriteGameObject = transform.Find("WeaponSprite").gameObject;

            Anim = BaseGameObject.GetComponent<Animator>();

            EventHandler = BaseGameObject.GetComponent<AnimationEventHandler>();

            initDone = true;
        }

        private void Update()
        {
            attackCounterResetTimeNotifier.Tick();
            //Debug.Log(player.StateMachine.CurrentState.animBoolName);
        }

        private void ResetAttackCounter()
        {
            print("Reset Attack Counter");
            CurrentAttackCounter = 0;
        }

        private void OnEnable()
        {
            EventHandler.OnUseInput += HandleUseInput;
            attackCounterResetTimeNotifier.OnNotify += ResetAttackCounter;
        }

        private void OnDisable()
        {
            EventHandler.OnUseInput -= HandleUseInput;
            attackCounterResetTimeNotifier.OnNotify -= ResetAttackCounter;
        }

        /// <summary>
        /// Invokes event to pass along information from the AnimationEventHandler to a non-weapon class.
        /// </summary>
        private void HandleUseInput() => OnUseInput?.Invoke();
    }
}