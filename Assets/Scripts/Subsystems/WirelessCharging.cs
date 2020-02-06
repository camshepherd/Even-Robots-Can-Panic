using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WirelessCharging : MonoBehaviour, ISubsystem {

    private GameDirector gameDirector;
    [SerializeField] public int componentHealth;
    public readonly int maxHealth = 100;
    public GameObject visionBlackout;

    // Start is called before the first frame update
    void Start() {
        this.componentHealth = maxHealth;
        this.gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
    }

    public int GetHealth() {
        return (int) this.componentHealth;
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
        LightController[] lights = FindObjectsOfType<LightController>();
        foreach (LightController light in lights)
        {
            light.timeToNextPossibleOccurrence = 0.0f;
        }
    }

    public override string ToString()
    {
        return "Lighting System";
    }

    public int GetPercentHealth()
    {
        return componentHealth * 100 / maxHealth;
    }

    public string GetDescription()
    {
        return "Controls lighting on the ship";
    }
}
