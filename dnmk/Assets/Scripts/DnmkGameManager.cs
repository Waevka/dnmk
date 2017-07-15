﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnmkGameManager : MonoBehaviour {

    public static DnmkGameManager Instance { get; private set; }
    [SerializeField]
    private bool gameEnabled;

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

    private void Awake()
    {
        gameEnabled = false;
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
           dnmkBulletPool.IsReady && dnmkStage.IsReady);
        EnableGame();
        gameEnabled = true;
    }

    private void EnableGame()
    {
        Debug.Log("Game components ready at time: " + Time.fixedTime);
        Debug.Log("This frame time: " + Time.fixedUnscaledDeltaTime);
        dnmkStage.StartEvents();
    }
}
