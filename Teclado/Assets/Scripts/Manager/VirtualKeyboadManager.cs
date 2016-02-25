using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public delegate void KeyboardParameter <T> ( T _parameter);
public delegate void Keyboard();

public class VirtualKeyboadManager : MonoBehaviour
{
    public string currentInputString { set; get; }
    public string lastInputString { set; get; }
    public bool capsLock { set; get; }

    public static VirtualKeyboadManager instance;

    #region EVENTOS PUBLICS
    public KeyboardParameter<string> eventWriteInputStringParameter;
    public Keyboard eventWriteInputString;

    public KeyboardParameter<bool> chargeCapsLock;
    #endregion

    void Awake()
    {
        instance = this;
        this.capsLock = true;
    }

    public void clearInputString()
    {
        this.currentInputString = "";
    }

    public void writeInputString(GameObject _obj)
    {
        string _letter = _obj.GetComponent<VirtualKeyboadBehaviour>().getStringInput(this.capsLock);
        this.currentInputString += _letter;
        this.lastInputString = _letter;

        if (this.eventWriteInputString != null) this.eventWriteInputString();
        if (this.eventWriteInputStringParameter != null) this.eventWriteInputStringParameter(this.lastInputString);
    }

    public void CapsLock()
    {
        this.capsLock = !this.capsLock;
        if (this.chargeCapsLock != null) this.chargeCapsLock(this.capsLock);
    }
}
