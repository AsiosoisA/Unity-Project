using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Doorway : RestaurantComponent, IInteractableStructure
{
    private PlayerInteractState state;
    public void Interact(PlayerInteractState state, Player requester)
    {
        this.state = state;

        if(!restaurant.isOpened)
        {
            AlertManager.Instance.MakeAlert(
            "나가기",
            "식당 파트를 종료하고 RPG 세계로 돌아갈까요?",
            GetOut,
            OnInteractFinished
            );
        }
    }

    public void GetOut()
    {
        OnInteractFinished();

        Debug.Log("나감! 여기서 씬 전환하면 됨.");
        SceneManager.LoadScene("Main");
    }

    public bool IsShouldHidePlayer()
    {
        return false;
    }

    public void OnInteractFinished()
    {
        state.OnInteractFinished();
    }
}
