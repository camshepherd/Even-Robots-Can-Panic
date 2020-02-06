using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityAudio : MonoBehaviour
{
    public AudioSource speaker;
    public AudioClip soundEffect;
    int nearby = 0;
    // Start is called before the first frame update
    void Start()
    {
        speaker = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            nearby += 1;
            
            if(nearby == 1)
            {
                speaker.clip = soundEffect;
                speaker.loop = true;
                speaker.Play();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            nearby -= 1;
            if (nearby == 0)
            {
                speaker.Stop();
            }
            
        }
    }

}
