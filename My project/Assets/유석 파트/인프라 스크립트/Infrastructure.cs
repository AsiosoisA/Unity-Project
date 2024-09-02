using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infrastructure : MonoBehaviour
{    
    #region 플레이어 데이터
    public PlayerDataForSave playerData;
    #endregion

    #region 유니티 메소드
    public virtual void Awake(){
        playerData = PlayerDataManager.Instance.Data;
    }

    private void Update()
    {
        LogicUpdate();
    }
    protected virtual void LogicUpdate(){

    }

    private void FixedUpdate() {
        PhysicsUpdate();
    }
    protected virtual void PhysicsUpdate(){

    }
    #endregion
}
