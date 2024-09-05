using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MinigameManager : MonoBehaviour
{
    /*
        이 클래스는 중계 역할만 할 것. 미니게임 객체들이 적절하게 시작될 수 있도록, 끝난다면 리워드도 잘 줄 수 있도록 해야 한다!
    
        + 나중엔 싱글톤패턴으로 구현할 것.
    */

    public KeyInstanceObjectPool pool {get; private set;}
    public KeyContainer keyContainer {get; private set;}
    private Minigame minigameToTrack;
    private SO_MinigameData minigameToTrackData;
    private IMinigamable requester;

    void Awake()
    {
        pool = GetComponentInChildren<KeyInstanceObjectPool>();
        keyContainer = GetComponentInChildren<KeyContainer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateAndStartMinigame(IMinigamable requester, SO_MinigameData minigameData)
    {
        this.requester = requester;

        if(minigameData.GetMinigameType() == typeof(SequencialKeyMinigame))
        {
            minigameToTrack = new SequencialKeyMinigame(this, minigameData as SO_SequencialKeyMinigame);
            minigameToTrackData = minigameData;
        }
        else if(minigameData.GetMinigameType() == typeof(HoldKeyMinigame))
        {
            minigameToTrack = new HoldKeyMinigame(this, minigameData as SO_HoldKeyMinigame);
            minigameToTrackData = minigameData;
        }
        else
        {
            Debug.LogError("타입을 뭔가 잘 못 넣은 것 같습니다!");
        }

        minigameToTrack.StartMinigame(requester.GetGameObject(), requester.GetContainerOffset());
    }

    public void OnMinigameFinished()
    {
        requester.OnMyMinigameFinished(minigameToTrackData.minigameName);
    }

    public void OnMinigameFailed()
    {
        requester.OnMyMinigameFailed(minigameToTrackData.minigameName);
    }

    public void OnKeyInputSuccessed()
    {
        requester.OnKeyInputSuccessed();
    }
}