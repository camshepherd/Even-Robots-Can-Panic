﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engines : MonoBehaviour, ISubsystem {

    private GameDirector gameDirector;
    private int componentHealth;
    private readonly int maxHealth = 100;

    // Start is called before the first frame update
    void Start() {
        this.componentHealth = maxHealth;
        this.gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
    }

    public int GetHealth() {
        return this.componentHealth;
    }

    public void TakeDamage(int damageAmount) {
        this.componentHealth = Mathf.Max(0, this.componentHealth - damageAmount);
        ActivateEffect();
    }

    public void Repair() {
        int initialHealth = this.componentHealth;
        this.componentHealth += 10;
        ActivateEffect();
        if (this.componentHealth > maxHealth)
        {
            this.componentHealth = maxHealth;
        }
        if (GetPercentHealth() == 100 && initialHealth < maxHealth)
        {
            WindowsVoice.speak("The " + ToString() + " has been repaired");
        }
    }

    private void ActivateEffect() {
        gameDirector.SetSpeed(GetPercentHealth());
    }

    public override string ToString()
    {
        return "Engine";
    }

    public int GetPercentHealth()
    {
        return componentHealth * 100 / maxHealth;
    }

    public string GetDescription()
    {
        return "Affects speed of travel";
    }
}
