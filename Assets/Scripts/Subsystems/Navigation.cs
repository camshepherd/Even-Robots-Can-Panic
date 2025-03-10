﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour, ISubsystem {

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
        if (componentHealth == 0) {
            ActivateEffect();
        }
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
        gameDirector.ModifyRNG(((float) maxHealth - componentHealth)/300);
    }

    public override string ToString()
    {
        return "Navigation Unit";
    }

    public int GetPercentHealth()
    {
        return componentHealth * 100 / maxHealth;
    }

    public string GetDescription()
    {
        return "Allows avoidance of asteroids";
    }
}
