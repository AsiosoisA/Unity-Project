using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerWindKnockbackSkillState : PlayerAbilityState
{
    #region Variables
    private float knockbackRadius;
    private float knockbackForce;
    private float upwardForce;
    private LayerMask enemyLayer;
    #endregion

    #region Unity Callback Functions
    public PlayerWindKnockbackSkillState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        PerformKnockback();

        knockbackRadius = playerData.knockbackRadius;
        knockbackForce = playerData.knockbackForce;
        upwardForce = playerData.upwardForce;
        enemyLayer = playerData.enemyLayer;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        stateMachine.ChangeState(player.IdleState);
    }
    #endregion

    #region Skil Functions
    private void PerformKnockback()
    {
        Collider2D[] enemiesToKnockback = Physics2D.OverlapCircleAll(player.transform.position, knockbackRadius, enemyLayer);

        foreach (Collider2D enemy in enemiesToKnockback)
        {
            Rigidbody2D enemyRB = enemy.GetComponent<Rigidbody2D>();

            if (enemyRB != null)
            {
                // ¿ÞÂÊ : -1 
                Vector2 knockbackDirection = (enemy.transform.position - player.transform.position).normalized;
                knockbackDirection.y += upwardForce;

                enemyRB.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
    #endregion
}
