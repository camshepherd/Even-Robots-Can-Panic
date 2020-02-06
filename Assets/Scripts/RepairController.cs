using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairController : MonoBehaviour
{

    public Camera cam;


    public GameObject cogs;
    
    public float center;

    public float repair_time = 0.0f;

    public float detectDistance = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
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

                    if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Joystick1Button0))
                    {
                        cogs.transform.Rotate(0.0f, 4.0f, 0.0f);
                        repair_time += Time.deltaTime;
                        if (repair_time > 2.0f)
                        {
                            hit.transform.GetComponentInParent<ISubsystem>().Repair();
                            repair_time = 0.0f;
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Joystick1Button0))
                    {
                        repair_time = 0.0f;
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
