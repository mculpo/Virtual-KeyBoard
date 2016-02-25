using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public delegate void KeyboardParameter <T> ( T _parameter);
public delegate void Keyboard();

public class VirtualKeyboadManager : MonoBehaviour
{
    #region PUBLIC VARIABLE
    public string currentInputString { set; get; }          //String com a palavra atual digitada
    public string lastInputString { set; get; }             //String com a a palavra anterior digitada
    public bool capsLock { set; get; }                      //Boolean para verificar se o teclado está em CapsLocks
    public bool usingVirtualKeyBoard { set; get; }          //Verifica se está usando o Teclado Virtual

    public GameObject keyBoard;                             //Referencia do GameObject que tem todo configuração do Teclado Virtual
    public VirtualKeyBoard virtualKeyboard { set; get; }    //Referencia do VirtualKeyBoard (responsalve por inicializar o teclado e trocar)
    public TextAsset json;                                  //ref json
    public static VirtualKeyboadManager instance;           //Singleton deste Objeto 
    #endregion

    #region EVENTOS PUBLICS
    public KeyboardParameter<string> eventWriteInputStringParameter;        //Evento disparado do teclado com um parametro de uma string
    public Keyboard eventWriteInputString;                                  //Evento disparado do teclado sem parametro

    public KeyboardParameter<bool> chargeCapsLock;                          //Evento disparado quando o capslock é precionado
    #endregion

    #region Variable UI Reference
    public Text currentSelectText { set; get; }                             //Referencia do objeto "TEXT" no qual o keyboard está apontado para Escrita/Leitura
    #endregion

    #region METHODS UNITY
    /// <summary>
    /// Inicialização das variaveis
    /// </summary>
    void Awake()
    {
        instance = this;
        this.usingVirtualKeyBoard = false;
        this.capsLock = false;
        this.virtualKeyboard = GetComponent<VirtualKeyBoard>();
        this.chargeVirtualKeyBoard();
    }
    #endregion

    #region METHODS PUBLICS
    /// <summary>
    /// Ligar e Desligar Keyboard sem parametro (faz as trocas opostas
    /// </summary>
    public void keyBoardOnOff()
    {
        this.keyBoard.SetActive(!this.keyBoard.activeSelf);
        if (this.keyBoard.activeSelf) this.usingVirtualKeyBoard = true;
        else this.usingVirtualKeyBoard = false;
    }

    /// <summary>
    /// Ligar e Desligar Keyboard com parametro
    /// </summary>
    public void keyBoardOnOff(bool _on_off)
    {
        this.keyBoard.SetActive(_on_off);
        if (this.keyBoard.activeSelf) this.usingVirtualKeyBoard = true;
        else this.usingVirtualKeyBoard = false;
    }

    /// <summary>
    /// Limpar a string principal que foi montada 
    /// </summary>
    public void clearInputString()
    {
        this.currentInputString = "";
    }

    /// <summary>
    /// Metodo que dispara os eventos para ser escrito nos objeto que implementaram os metodos eventWrite eventWriteParameter
    /// </summary>
    /// <param name="_obj"></param>
    public void writeInputStringEvent(GameObject _obj)
    {
        string _letter = _obj.GetComponent<VirtualKeyBoardBehaviour>().getStringInput(this.capsLock);
        this.currentInputString += _letter;
        this.lastInputString = _letter;

        if (this.eventWriteInputString != null) this.eventWriteInputString();
        if (this.eventWriteInputStringParameter != null) this.eventWriteInputStringParameter(this.lastInputString);
    }

    /// <summary>
    /// Metodo que dispara os eventos para ser escrito nos objeto que implementaram os metodos eventWrite eventWriteParameter
    /// </summary>
    /// <param name="_obj"></param>
    public void writeInputStringEvent(string _letter)
    {
        string letter = _letter;
        this.currentInputString += letter;
        this.lastInputString = letter;

        if (this.eventWriteInputString != null) this.eventWriteInputString();
        if (this.eventWriteInputStringParameter != null) this.eventWriteInputStringParameter(this.lastInputString);
    }

    /// <summary>
    /// Metodo que dispara os eventos para ser escrito nos objeto que implementaram os metodos eventWrite eventWriteParameter
    /// </summary>
    /// <param name="_obj"></param>
    public void writeInputString(GameObject _obj)
    {

        string _letter = _obj.GetComponent<VirtualKeyBoardBehaviour>().getStringInput(this.capsLock);
        this.currentInputString += _letter;
        this.lastInputString = _letter;

        if (this.eventWriteInputString != null) this.eventWriteInputString();
        if (this.eventWriteInputStringParameter != null) this.eventWriteInputStringParameter(this.lastInputString);

        if (this.currentSelectText == null)
            return;
        
        this.currentSelectText.text += _letter;
    }

    /// <summary>
    /// Método responsavel por apagar o ultimo Caracter do TEXT de referencia aonde o Manager aponta
    /// </summary>
    public void backspace()
    {
        if (this.currentSelectText == null)
            return;

        string last = this.currentSelectText.text;
        string newText = "";

        Debug.Log(last.Length);

        for(int i = 0 ; i < last.Length; i++)
        {
            Debug.Log(i);
            if (i != last.Length - 1)
            {
                Debug.Log(i + ".." + last[i]);
                newText += last[i];
            }
            
        }
        this.currentSelectText.text = newText;
    }

    /// <summary>
    /// Método para ativar e desativar o CAPsLock
    /// </summary>
    public void CapsLock()
    {
        this.capsLock = !this.capsLock;
        if (this.chargeCapsLock != null) this.chargeCapsLock(this.capsLock);
    }

    /// <summary>
    /// Método para fazer a troca do uso do Teclado
    /// </summary>
    public void chargeVirtualKeyBoard()
    {
        this.usingVirtualKeyBoard = !this.usingVirtualKeyBoard;
        if (this.usingVirtualKeyBoard)
            keyBoardOnOff(true);
        else
            keyBoardOnOff(false);
    }
    #endregion
}
