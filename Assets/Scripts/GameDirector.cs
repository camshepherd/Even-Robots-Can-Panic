using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour {

    [SerializeField] private readonly float baseRNG = 0.1f;
    [SerializeField] private readonly float baseSpeed = 1f;
    [SerializeField] private float calculatedAsteroidChance;
    [SerializeField] private bool shieldsActive;
    [SerializeField] private float distanceToEnd;
    [SerializeField] private float speed;
    [SerializeField] private int shipHealth;
    private int shipHealthMax = 20;

    public AudioClip[] clips;


    public GameObject progressBar;
    public GameObject healthBar;


    private float timeToNextAsteroid = 2.0f;

    private float randomModifiers = 0f;

    private SubsystemController gameController;
    private CameraShake[] shakers;
    private AudioSource source;

    private void Start() {
        gameController = GameObject.Find("SubsystemController").GetComponent<SubsystemController>();
        shakers = GameObject.FindObjectsOfType<CameraShake>();
        source = GetComponent<AudioSource>();
        shieldsActive = true;
        distanceToEnd = 180f;
        speed = baseSpeed;
        shipHealth = shipHealthMax;
    }

    private void FixedUpdate()
    {
        timeToNextAsteroid = timeToNextAsteroid - Time.deltaTime;
    }

    private void Update()
    {
        if (timeToNextAsteroid < 0) {
            float asteroidDelayModifier = 0.5f / ((baseRNG + randomModifiers) * 1.7f);
            timeToNextAsteroid = Random.Range(5 + asteroidDelayModifier, 8 + asteroidDelayModifier);
            if (shieldsActive)
            {
                DamageRandomSubsystem();
                foreach (CameraShake shaker in shakers)
                {
                    shaker.ShakeCamera(3, 3);
                }
                source.clip = clips[Random.Range(0, clips.Length - 1)];
                source.Play();
            }
            else
            {
                DamageShip();
                foreach (CameraShake shaker in shakers)
                {
                    shaker.ShakeCamera(5, 5);
                }
                source.clip = clips[Random.Range(0, clips.Length - 1)];
                source.Play();
            }
        }
        if (distanceToEnd <= 0) {
            GameWin();
        }
        else {
            distanceToEnd -= speed * Time.deltaTime;
        }

        progressBar.GetComponent<ProgressController>().currentProgress = (180f - distanceToEnd) / 180f;

        healthBar.GetComponentInChildren<Text>().text = shipHealth + "";

    }

    public void ModifyRNG(float amount) {
        randomModifiers += amount;
    }

    private float GetRNG() {
        return baseRNG + randomModifiers;
    }

    public void SetShields(bool state) {
        shieldsActive = state;
    }

    public void SetSpeed(int speed) {
        this.speed = (float)(baseSpeed * speed) / 100.0f;
    }

    private void DamageRandomSubsystem() {
        List<ISubsystem> subsystems = gameController.GetSubsystems();
        float totalDamage = Random.Range(60, 121);
        int numberOfTargets = Random.Range(2, 5);
        int mainHit = (int) ((float) totalDamage * (Random.Range(30, 61) / 100f));
        int secondaryHits = ((int)((totalDamage - (float) mainHit) / (numberOfTargets - 1)));
        int iteration = 0;
        int choice = 0;
        int damage = mainHit;
        bool hitDesignatedTarget = false;
        ISubsystem subsystemHit;

        for (int hit = 0; hit < numberOfTargets; hit++) {
            choice = Random.Range(0, subsystems.Count);
            iteration = 0;
            hitDesignatedTarget = false;
            subsystemHit = null;
            foreach (ISubsystem subsystem in subsystems) {
                Debug.Log("Iteration: " + iteration);
                Debug.Log("Object: " + subsystem);
                if (iteration == choice && subsystem.GetHealth() > 0) {
                    subsystem.TakeDamage(damage);
                    if(subsystem.GetPercentHealth() == 0)
                    {
                        WindowsVoice.speak("The " + subsystem.ToString() + " is at " + subsystem.GetPercentHealth() + "% health");
                    }
                    
                    hitDesignatedTarget = true;
                    subsystemHit = subsystem;
                    Debug.Log(subsystem.GetType() + " took " + damage + " amount of damage.");
                    break;
                } else {
                    iteration++;
                }
            }
            if (!hitDesignatedTarget) {
                foreach (ISubsystem subsystem in subsystems) {
                    if (subsystem.GetHealth() > 0) {
                        subsystem.TakeDamage(damage);
                        if (subsystem.GetPercentHealth() == 0)
                        {
                            WindowsVoice.speak("The " + subsystem.ToString() + " is at " + subsystem.GetPercentHealth() + "% health");
                        }
                        hitDesignatedTarget = true;
                        subsystemHit = subsystem;
                        Debug.Log(subsystem.GetType() + " took " + damage + " amount of damage.");
                        break;
                    }
                }
            }
            if (!hitDesignatedTarget) {
                //Damage first subsystem found with HP
                // If all subsystems
                DamageShip();
                WindowsVoice.speak("Hull strength is at " + GetShipHealthPercent() + "%");
                return;
            }
            damage = secondaryHits;
            subsystems.Remove(subsystemHit);
        }
    }

    private void DamageShip() {
        this.shipHealth -= 7;
        if (shipHealth <= 0) {
            GameOver();
        }
    }

    private void GameOver() {
        SceneManager.LoadScene("GameOver");
    }

    private void GameWin() {
        SceneManager.LoadScene("GameWin");
    }

    private int GetShipHealthPercent()
    {
        return shipHealth * 100 / shipHealthMax;
    }
}
