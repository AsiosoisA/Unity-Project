using System;
using UnityEngine;

namespace Bardent.Weapons
{
    public class AnimationEventHandler : MonoBehaviour
    {
        //여기부터 추가
        public GameObject fireBall;
        public void GenFireBall()
        {
            GameObject b = Instantiate(fireBall, transform);
        }

        public GameObject flameField;
        public void GenFlameField()
        {
            GameObject b = Instantiate(flameField, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        }

        public GameObject iceBall;
        public void GenIceBall()
        {
            GameObject b = Instantiate(iceBall, transform);
        }

        public GameObject iceField;
        public void GenIceField()
        {
            GameObject b = Instantiate(iceField, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        }

        //여기까지

        public Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }


        public void crouchEnd()
        {
            animator.SetInteger("counter", 0);
        }

        public event Action OnFinish;
        public event Action OnStartMovement;
        public event Action OnStopMovement;
        public event Action OnAttackAction;
        public event Action OnMinHoldPassed;

        /*
         * This trigger is used to indicate in the weapon animation when the input should be "used" meaning the player has to release the input key and press it down again to trigger the next attack.
         * Generally this animation event is added to the first "action" frame of an animation. e.g the first sword strike frame, or the frame where the bow is released.
         */
        public event Action OnUseInput;

        public event Action OnEnableInterrupt; 

        public event Action<bool> OnSetOptionalSpriteActive;

        public event Action<bool> OnFlipSetActive; 

        public event Action<AttackPhases> OnEnterAttackPhase;

        /*
         * Animations events used to indicate when a specific time window starts and stops in an animation. These windows are identified using the
         * AnimationWindows enum. These windows include things like when the shield's block is active and when it can parry.
         */
        public event Action<AnimationWindows> OnStartAnimationWindow;
        public event Action<AnimationWindows> OnStopAnimationWindow;
        

        private void AnimationFinishedTrigger() => OnFinish?.Invoke();
        private void StartMovementTrigger() => OnStartMovement?.Invoke();
        private void StopMovementTrigger() => OnStopMovement?.Invoke();
        private void AttackActionTrigger() => OnAttackAction?.Invoke();
        private void MinHoldPassedTrigger() => OnMinHoldPassed?.Invoke();
        private void UseInputTrigger() => OnUseInput?.Invoke();

        private void SetOptionalSpriteEnabled() => OnSetOptionalSpriteActive?.Invoke(true);
        private void SetOptionalSpriteDisabled() => OnSetOptionalSpriteActive?.Invoke(false);

        private void SetFlipActive() => OnFlipSetActive?.Invoke(true);
        private void SetFlipInactive() => OnFlipSetActive?.Invoke(false);

        private void EnterAttackPhase(AttackPhases phase) => OnEnterAttackPhase?.Invoke(phase);

        private void StartAnimationWindow(AnimationWindows window) => OnStartAnimationWindow?.Invoke(window);
        private void StopAnimationWindow(AnimationWindows window) => OnStopAnimationWindow?.Invoke(window);

        private void EnableInterrupt() => OnEnableInterrupt?.Invoke();
    }
}