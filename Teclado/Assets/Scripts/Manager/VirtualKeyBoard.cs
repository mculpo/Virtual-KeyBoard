using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;


public class VirtualKeyBoard : MonoBehaviour
{
    public string nameVirtualKeyBoard { set; get; }

    public List<StructKeyBoard> listKeyBoard = new List<StructKeyBoard>();

    private StructKeyBoard _keyboard;
    private bool isLoad = true;

    /// <summary>
    /// Carreagar teclados virtuais
    /// </summary>
    public void loadKeyBoards()
    {
        if (this.isLoad)
        {
            string path = Application.dataPath + "/Resource/virtualkeyboard.json";
            string _text = File.ReadAllText(path);

            if (_text != null)
            {
                listKeyBoard = new List<StructKeyBoard>(JsonMapper.ToObject<List<StructKeyBoard>>(_text));
                //this.isLoad = false;
            }
        }
    }
    /// <summary>
    /// Deletar Teclado Virtual
    /// </summary>
    /// <param name="_index"></param>
    public void deleteKeyBoard(int _index)
    {
        this.listKeyBoard.RemoveAt(_index);
        SaveKeyBoard();
    }

    /// <summary>
    /// Salvar Teclado Virtual
    /// </summary>
    public void saveVirtualKeyBoard()
    {
        if (listKeyBoard == null)
            this.listKeyBoard = new List<StructKeyBoard>();


        _keyboard = new StructKeyBoard();
        _keyboard.listStructKeyButton = new List<StrucKeyButton>();


        _keyboard.namePainel = nameVirtualKeyBoard;

        GameObject _child = gameObject.transform.GetChild(0).gameObject;
        _keyboard.anchored = _child.GetComponent<RectTransform>().anchoredPosition;
        _keyboard.sizedelta = _child.GetComponent<RectTransform>().sizeDelta;

        int count = _child.transform.childCount;

        
        for (int i = 0; i < count; i++)
        {
            StrucKeyButton _keyButton = new StrucKeyButton();
            _keyButton.active = _child.transform.GetChild(i).gameObject.activeSelf;
            _keyButton.anchored = _child.transform.GetChild(i).gameObject.GetComponent<RectTransform>().anchoredPosition;
            _keyButton.sizedelta = _child.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta;
            _keyButton.capital = _child.transform.GetChild(i).gameObject.GetComponent<VirtualKeyboadBehaviour>().capital;
            _keyButton.tiny = _child.transform.GetChild(i).gameObject.GetComponent<VirtualKeyboadBehaviour>().tiny;
            _keyboard.listStructKeyButton.Add(_keyButton);
        }

        this.listKeyBoard.Add(_keyboard);
        SaveKeyBoard();
    }

    public void updateVirtualKeyBoard(int index)
    {
        if (listKeyBoard == null)
        {
            Debug.Log("Não existe uma lista");
            return;
        }

        _keyboard = listKeyBoard[index];

        _keyboard.listStructKeyButton.Clear();
        _keyboard.namePainel = nameVirtualKeyBoard;

        GameObject _child = gameObject.transform.GetChild(0).gameObject;
        _keyboard.anchored = _child.GetComponent<RectTransform>().anchoredPosition;
        _keyboard.sizedelta = _child.GetComponent<RectTransform>().sizeDelta;

        int count = _child.transform.childCount;


        for (int i = 0; i < count; i++)
        {
            StrucKeyButton _keyButton = new StrucKeyButton();
            _keyButton.active = _child.transform.GetChild(i).gameObject.activeSelf;
            _keyButton.anchored = _child.transform.GetChild(i).gameObject.GetComponent<RectTransform>().anchoredPosition;
            _keyButton.sizedelta = _child.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta;
            _keyButton.capital = _child.transform.GetChild(i).gameObject.GetComponent<VirtualKeyboadBehaviour>().capital;
            _keyButton.tiny = _child.transform.GetChild(i).gameObject.GetComponent<VirtualKeyboadBehaviour>().tiny;
            _keyboard.listStructKeyButton.Add(_keyButton);
        }

        SaveKeyBoard();
    }

    /// <summary>
    /// Atualizar os teclado Virtual
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_currentIndex"></param>
    public void copyToUseVirtualKeyBoard(int _index, int _currentIndex)
    {
        if (listKeyBoard == null || _index == _currentIndex)
            return;

        StructKeyBoard _keyboard = this.listKeyBoard[_index];
        GameObject _child = gameObject.transform.GetChild(0).gameObject;

        _keyboard.listStructKeyButton.Clear();

        _keyboard.namePainel = this.listKeyBoard[_currentIndex].namePainel;
        _keyboard.anchored = this.listKeyBoard[_currentIndex].anchored;
        _keyboard.sizedelta = this.listKeyBoard[_currentIndex].sizedelta;

        int count = _child.transform.childCount;

        for (int i = 0; i < count; i++)
        {
            StrucKeyButton _keyButton = new StrucKeyButton();
            _keyButton.active = _child.transform.GetChild(i).gameObject.activeSelf;
            _keyButton.anchored = this.listKeyBoard[_currentIndex].listStructKeyButton[i].anchored;
            _keyButton.sizedelta = this.listKeyBoard[_currentIndex].listStructKeyButton[i].sizedelta;
            _keyButton.capital = this.listKeyBoard[_currentIndex].listStructKeyButton[i].capital;
            _keyButton.tiny = this.listKeyBoard[_currentIndex].listStructKeyButton[i].tiny;
            _keyboard.listStructKeyButton.Add(_keyButton);
        }

        SaveKeyBoard();
    }
    /// <summary>
    /// Método responsavel por setar o uso do teclado de acordo com o index passado dos teclados configurados
    /// </summary>
    /// <param name="_index"></param>
    public void useVirtualKeyBoard(int _index)
    {
        GameObject _painel = gameObject.transform.GetChild(0).gameObject;

        StructKeyBoard _keyBoard = listKeyBoard[_index];

        this.nameVirtualKeyBoard = _keyBoard.namePainel;

        RectTransform _painelRectTransform = _painel.transform.GetComponent<RectTransform>();
        _painelRectTransform.anchoredPosition = _keyBoard.anchored;
        _painelRectTransform.sizeDelta = _keyBoard.sizedelta;

        for (int i = 0; i < _keyBoard.listStructKeyButton.Count; i++)
        {
            RectTransform _rectTransform = _painel.transform.GetChild(i).GetComponent<RectTransform>();
            VirtualKeyboadBehaviour _virtual = _painel.transform.GetChild(i).GetComponent<VirtualKeyboadBehaviour>();

            _rectTransform.gameObject.SetActive(_keyBoard.listStructKeyButton[i].active);

            if (_rectTransform.gameObject.activeSelf)
            {
                _rectTransform.anchoredPosition = _keyBoard.listStructKeyButton[i].anchored;
                _rectTransform.sizeDelta = _keyBoard.listStructKeyButton[i].sizedelta;
                _virtual.capital = _keyBoard.listStructKeyButton[i].capital;
                _virtual.tiny = _keyBoard.listStructKeyButton[i].tiny;
            }

            _virtual.HandleChargeCapsLock(false);
        }
    }

    void SaveKeyBoard()
    {
        string path = Application.dataPath + "/Resource/virtualkeyboard.json";
        File.WriteAllText(path, JsonMapper.ToJson(this.listKeyBoard));
        this.isLoad = true;
    }
}

public class StructKeyBoard
{
    public string namePainel;                         //Nome do painel
    public Vector2 anchored;                          //Representa o valor anchored do painel principal
    public Vector2 sizedelta;                         //Representa o valor sizedelta do painel principal
    public List<StrucKeyButton> listStructKeyButton;  //Lista de cada caracteristicas dos botões que serão salvos do teclado
}

public class StrucKeyButton
{
    public bool active;                         //Variavel para verificar se o objeto está salvo
    public Vector2 anchored;                    //Represeta o valor anchored do botão
    public Vector2 sizedelta;                   //Representa o valor sizedelta do botão
    public string capital;                      //Valor que vai representar a string do botão Maiusculo
    public string tiny;                         //Valor que vai representar a string do botão Minusculo
}



