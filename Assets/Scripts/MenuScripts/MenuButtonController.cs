using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    [SerializeField] private int maxIndex;
    [SerializeField] private bool keyDown;

    public AudioSource audioMenu;
    public int index;

    private void Start()
    {
        audioMenu = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(Input.GetAxis("Horizontal") != 0)
        {
            if (!keyDown)
            {
                if (Input.GetAxis("Horizontal") > 0)
                {
                    if (index < maxIndex)
                    {
                        index++;
                    }
                    else
                        index = 0;
                }
                else if(Input.GetAxis("Horizontal") < 0)
                {
                    if (index > 0)
                    {
                        index--;
                    }
                    else
                        index = maxIndex;
                }
                keyDown = true;
            }
        }
        else
            keyDown = false;
    }
}
