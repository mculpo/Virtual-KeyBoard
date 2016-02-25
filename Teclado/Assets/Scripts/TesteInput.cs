using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TesteInput : MonoBehaviour 
{
    public Text write;

    void Start()
    {
        VirtualKeyboadManager.instance.eventWriteInputString += HandleWrite;
    }

    void OnDisable()
    {
        VirtualKeyboadManager.instance.eventWriteInputString -= HandleWrite;
    }

    void HandleWrite()
    {
        this.write.text += VirtualKeyboadManager.instance.lastInputString;
    }
}
