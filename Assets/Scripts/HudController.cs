using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour
{
    public GameObject hudPanel;


    private void Start()
    {
        hudPanel.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {       
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("keypressed");

            if (!hudPanel.activeInHierarchy)
            {
                hudPanel.SetActive(true);
            }
            else
            {
                hudPanel.SetActive(false);
            }
        }
    }
}
