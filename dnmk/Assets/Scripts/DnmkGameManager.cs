using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnmkGameManager : MonoBehaviour {

    public enum DifficultyLevel { Easy, Normal, Hard, Lunatic };

    public static DnmkGameManager Instance { get; private set; }
    [SerializeField]
    private bool gameEnabled;
    [SerializeField]
    private int targetFrameRate; //TODO: edit in inspector

    [SerializeField]
    private DnmkPlayingField dnmkPlayingField = null;
    public DnmkPlayingField DnmkPlayingField
    {
        get { return dnmkPlayingField; }
    }

    [SerializeField]
    private DnmkParticleSystemPool dnmkParticleSystemPool = null;
    public DnmkParticleSystemPool DnmkParticleSystemPool
    {
        get { return dnmkParticleSystemPool; }
    }

    [SerializeField]
    private DnmkBulletPool dnmkBulletPool = null;
    public DnmkBulletPool DnmkBulletPool
    {
        get { return dnmkBulletPool; }
    }

    [SerializeField]
    private DnmkStage dnmkStage = null;
    public DnmkStage DnmkStage
    {
        get { return dnmkStage; }
    }

    [SerializeField]
    private DnmkPlayerManager dnmkPlayer = null;
    public DnmkPlayerManager DnmkPlayer
    {
        get { return dnmkPlayer; }
    }

    private void Awake()
    {
        gameEnabled = false;
        Application.targetFrameRate = 60;
        if (Instance != null)
        {
            Debug.LogWarning("Multiple instances of DnmkGameManager!", gameObject);
            return;
        }
        Instance = this;
        StartCoroutine(CheckIfAllComponentsAreReady());
    }
    
    IEnumerator CheckIfAllComponentsAreReady()
    {
        yield return new WaitUntil(() => dnmkPlayingField.IsReady && dnmkParticleSystemPool.IsReady &&
           dnmkBulletPool.IsReady && dnmkStage.IsReady && dnmkPlayer.IsReady);
        EnableGame();
        gameEnabled = true;
    }

    private void EnableGame()
    {
        Debug.Log("Game components ready at time: " + Time.fixedTime);
        Debug.Log("This frame time: " + Time.fixedUnscaledDeltaTime);
        dnmkPlayer.StartPlayer();
        dnmkStage.StartEvents();
    }
}
