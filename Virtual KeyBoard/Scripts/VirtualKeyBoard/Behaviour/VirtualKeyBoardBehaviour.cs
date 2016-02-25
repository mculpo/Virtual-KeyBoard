using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VirtualKeyBoardBehaviour : MonoBehaviour 
{
    [SerializeField] private string Capital;
    [SerializeField] private string Tiny;

    public Text childText;

    void Start()
    {
        this.childText = gameObject.transform.GetChild(0).GetComponent<Text>();
        VirtualKeyboadManager.instance.chargeCapsLock += HandleChargeCapsLock;
    }

    void OnDestroy()
    {
        VirtualKeyboadManager.instance.chargeCapsLock -= HandleChargeCapsLock;
    }

    public VirtualKeyBoardBehaviour(VirtualKeyBoardBehaviour _behaviour)
    {
        this.Capital = _behaviour.capital;
        this.Tiny = _behaviour.tiny;
    }

    public string capital
    {
        set { this.Capital = value; }
        get { return this.Capital; }
    }

    public string tiny
    {
        set { this.Tiny = value; }
        get { return this.Tiny; }
    }

    public string getStringInput(bool _capsLock)
    {
        if (!_capsLock)
            return this.Tiny;
        else
            return this.Capital;
    }

    public void HandleChargeCapsLock(bool _capsLock)
    {
        if (!_capsLock)
            this.childText.text = this.Tiny;
        else
            this.childText.text = this.Capital;
    }
}
