using UnityEngine;
using System.Collections;

public class VirtualKeyBoardInput : MonoBehaviour 
{
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (VirtualKeyboadManager.instance != null && !VirtualKeyboadManager.instance.usingVirtualKeyBoard)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                VirtualKeyboadManager.instance.backspace();
            }
            else if (Input.anyKeyDown)
            {
                VirtualKeyboadManager.instance.writeInputStringEvent(Input.inputString);
            }
        }
	}
}
