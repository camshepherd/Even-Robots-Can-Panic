using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairController : MonoBehaviour
{

    public Camera cam;
    public ShipComputer shipComputer;
    public int playerNum;
    public GameObject cogs;
    
    public float center;

    public float repair_time = 0.0f;
    protected AudioSource speaker;
    public AudioClip[] audioClips;
    public float detectDistance = 100f;
    // Start is called before the first frame update
    void Start()
    {
        speaker = GetComponent<AudioSource>();
        shipComputer = GameObject.FindObjectOfType<ShipComputer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 CameraCenter = cam.ScreenToWorldPoint(new Vector3( center* Screen.width/4f, Screen.height/2f, cam.nearClipPlane));
        Debug.DrawRay(CameraCenter, cam.transform.forward, Color.red, detectDistance);
        RaycastHit hit;
        if(Physics.Raycast(CameraCenter, cam.transform.forward, out hit, detectDistance)) {
             Debug.Log(hit.collider.gameObject.tag);
             if((hit.collider.gameObject.CompareTag("Machine") || hit.collider.transform.GetChild(0).CompareTag("Machine")) && Vector3.Distance(transform.position, hit.transform.position) < 2f) {
                 cogs.SetActive(true);
                   Debug.Log(hit.transform.name);

                   Debug.Log(hit.transform.GetComponentInParent<ISubsystem>().GetHealth());
                if (hit.transform.GetComponentInParent<ISubsystem>().GetPercentHealth() >= 100)
                {
                    cogs.GetComponent<Image>().color = Color.red;
                    cogs.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                }
                else
                {
                    cogs.GetComponent<Image>().color = Color.white;

                    if (Input.GetKey(KeyCode.E) && playerNum == 1 || Input.GetKey(KeyCode.Joystick1Button0) && playerNum == 2)
                    {
                        cogs.transform.Rotate(0.0f, 4.0f, 0.0f);
                        repair_time += Time.deltaTime * ((shipComputer.GetHealth() / 100) + 0.4f);
                        if (repair_time > 0.5f)
                        {
                            hit.transform.GetComponentInParent<ISubsystem>().Repair();
                            repair_time = 0.0f;
                            speaker.clip = audioClips[0];
                            speaker.Play();
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.E) && playerNum == 1 || Input.GetKeyUp(KeyCode.Joystick1Button0) && playerNum == 2)
                    {
                        repair_time = 0.0f;
                        cogs.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    }
                    else
                    {
                        repair_time = 0.0f;
                        cogs.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    }
                }
             }
        }
        else
        {
            cogs.SetActive(false);
            cogs.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }

             

         
    }



}
