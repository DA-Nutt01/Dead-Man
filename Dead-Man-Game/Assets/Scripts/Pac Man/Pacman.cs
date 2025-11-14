using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Movement))]
public class Pacman : PacmanBase
{
    public static Pacman Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        if (Instance != null && Instance != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    private void OnEnable()
    {
        base.OnEnable(); // Ensure base class event subscriptions are handled
    }

    private void OnDisable()
    {
        base.OnDisable(); // Ensure base class event unsubscriptions are handled
    }

    public override void ResetState()
    {
        base.ResetState();
        // Add any specific reset logic for Pacman here
    }

    public override void DeathSequence()
    {
        base.DeathSequence();
        // Add any specific death sequence logic for Pacman here
    }
}

