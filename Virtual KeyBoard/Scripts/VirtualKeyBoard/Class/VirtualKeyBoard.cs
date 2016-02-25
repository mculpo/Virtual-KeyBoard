using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;


public class VirtualKeyBoard : MonoBehaviour
{
    public string nameVirtualKeyBoard { set; get; }                 //Nome do Teclado atual
    public List<StructKeyBoard> listKeyBoard { set; get; }          //Lista dos teclados que existem

    /// <summary>
    /// Carreagar teclados virtuais para configurar e criar no Editor
    /// </summary>
    public void loadKeyBoards()
    {
#if UNITY_EDITOR
        
        ///Busca o caminho do Json no Editor
        string path = Application.dataPath + "/Virtual KeyBoard/KeyBoard/virtualkeyboard.txt";
        //Usa o FILE para buscar a informação dentro do caminhos passado por parametro
        string _text = File.ReadAllText(path);

        if (_text != null)
        {
            //Transforma a informação do json e alimenta a lista de teclados virtuais configurados
            listKeyBoard = new List<StructKeyBoard>(JsonMapper.ToObject<List<StructKeyBoard>>(_text));
        }
#endif
    }
    /// <summary>
    /// Carregar Teclado em tempo de execução usando recurso Resources, usando o nome para fazer a busca do teclado que quer ser usado
    /// </summary>
    /// <param name="_keyBoard">< Nome do Teclado  /param>
    public void loadKeyBoards(string _keyBoard)
    {
        ///Verifica se a lista é nula ou se ela está vazia
        if (this.listKeyBoard == null || this.listKeyBoard.Count == 0)
            this.listKeyBoard = new List<StructKeyBoard>();             //Cria uma nova lista

        /// Usando Resource para pegar as informaçções dos teclados dentro do json usando para salver TextAssets
        TextAsset _json = VirtualKeyboadManager.instance.json;

        //Debug.Log(_json);

        ///Se não for nulo
        if (_json != null)
        {
            //Transforma a informação do json e alimenta a lista de teclados virtuais configurados
            listKeyBoard = new List<StructKeyBoard>(JsonMapper.ToObject<List<StructKeyBoard>>(_json.text));
        }
        ///Chama o metodov"useVirtualKeyBoard" para configurar o teclado de acordo com o nome do teclado passado por parametro
        ///"getKeyboard" retorna o index do teclado de acordo com o nome do teclado na busca
        useVirtualKeyBoard(getKeyboard(_keyBoard));
    }
    /// <summary>
    /// Deletar Teclado Virtual
    /// </summary>
    /// <param name="_index"></param>
    public void deleteKeyBoard(int _index)
    {
        ///Deleta o teclado de acordo com o index do mesmo
        ///Salva as novas configurações
        this.listKeyBoard.RemoveAt(_index);
        SaveKeyBoard();
    }

    /// <summary>
    /// Salvar um Teclado Virtual novo criado no Editor (EDITOR)
    /// </summary>
    public void saveVirtualKeyBoard()
    {
        ///Verifica se a lista é nula
        if (listKeyBoard == null)
            /// Inicializa a lista
            this.listKeyBoard = new List<StructKeyBoard>();

        ///Cria um objeto keyboard
        StructKeyBoard _keyboard = new StructKeyBoard();
        ///Inicializa a lista das configurações do botão
        _keyboard.listStructKeyButton = new List<StrucKeyButton>();

        ///Atribui o nome do Teclado
        _keyboard.namePainel = nameVirtualKeyBoard;

        ///Pega a referencia do Filho(que contem a estrutura do teclado)
        GameObject _child = gameObject.transform.GetChild(0).gameObject;
        ///Salva as configurações das valores anchored do painel
        ///Salva asw connfigurações dos valores do sizedelta do painel
        _keyboard.anchored = _child.GetComponent<RectTransform>().anchoredPosition;
        _keyboard.sizedelta = _child.GetComponent<RectTransform>().sizeDelta;

        ///Pega a quantidade de filhos
        int count = _child.transform.childCount;

        ///Cria um laço de repetição para percorrer todos os filhos existentes dentro do Teclado virtual
        for (int i = 0; i < count; i++)
        {
            ///Cria um objeto botão
            StrucKeyButton _keyButton = new StrucKeyButton();
            ///Salva o estado do botão "i" (Ligado/Desligado).
            _keyButton.active = _child.transform.GetChild(i).gameObject.activeSelf;
            ///Salva as configurações dos valores do Anchored do botão.
            _keyButton.anchored = _child.transform.GetChild(i).gameObject.GetComponent<RectTransform>().anchoredPosition;
            ///Salva as configurações dos valores do sizedelta do botão.
            _keyButton.sizedelta = _child.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta;
            ///Salva as informações do texto MAIUSCULO que tem configura no botão (VirtualKeyboardBehaviour)
            _keyButton.capital = _child.transform.GetChild(i).gameObject.GetComponent<VirtualKeyBoardBehaviour>().capital;
            ///Salva as informações do texto MINUSCULO que tem configura no botão (VirtualKeyboardBehaviour)
            _keyButton.tiny = _child.transform.GetChild(i).gameObject.GetComponent<VirtualKeyBoardBehaviour>().tiny;
            ///Salva o botão em uma lista de copnfigurações de botões
            _keyboard.listStructKeyButton.Add(_keyButton);
        }

        ///Adiciona o este teclado virtual criado detro da lista de teclados criados
        ///Salva o teclado virtual dentro do Json
        this.listKeyBoard.Add(_keyboard);
        SaveKeyBoard();
    }

    /// <summary>
    /// Atualizar o Teclado Virtual (Editor)
    /// </summary>
    /// <param name="index"></param>
    public void updateVirtualKeyBoard(int index)
    {
        if (listKeyBoard == null)
        {
            Debug.Log("Não existe uma lista");
            return;
        }

        StructKeyBoard _keyboard = listKeyBoard[index];

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
            _keyButton.capital = _child.transform.GetChild(i).gameObject.GetComponent<VirtualKeyBoardBehaviour>().capital;
            _keyButton.tiny = _child.transform.GetChild(i).gameObject.GetComponent<VirtualKeyBoardBehaviour>().tiny;
            _keyboard.listStructKeyButton.Add(_keyButton);
        }

        SaveKeyBoard();
    }

    /// <summary>
    /// Copiar os teclado Virtual de outros existente (Editor)
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
    public bool useVirtualKeyBoard(int _index)
    {
        ///Verifica se o teclado está disativado ou se o index está dentro dos parametros
        if (!this.gameObject.transform.GetChild(0).gameObject.activeSelf || _index < 0 || _index > this.listKeyBoard.Count)
            return false;

        ///Pega a referencia 
        GameObject _painel = gameObject.transform.GetChild(0).gameObject;

        StructKeyBoard _keyBoard = listKeyBoard[_index];

        this.nameVirtualKeyBoard = _keyBoard.namePainel;

        RectTransform _painelRectTransform = _painel.transform.GetComponent<RectTransform>();
        _painelRectTransform.anchoredPosition = _keyBoard.anchored;
        _painelRectTransform.sizeDelta = _keyBoard.sizedelta;

        for (int i = 0; i < _keyBoard.listStructKeyButton.Count; i++)
        {
            RectTransform _rectTransform = _painel.transform.GetChild(i).GetComponent<RectTransform>();
            VirtualKeyBoardBehaviour _virtual = _painel.transform.GetChild(i).GetComponent<VirtualKeyBoardBehaviour>();

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
        return true;
    }


    /// <summary>
    /// Salvar as configurações que forão criadas do teclado
    /// </summary>
    void SaveKeyBoard()
    {
#if UNITY_EDITOR
        string path = Application.dataPath + "/Virtual KeyBoard/KeyBoard/virtualkeyboard.txt";
        File.WriteAllText(path, JsonMapper.ToJson(this.listKeyBoard));
#endif
    }

    /// <summary>
    /// Metédo que percorre uma lista de tecldos e retorna o INDEX com o nome passado por parametro
    /// </summary>
    /// <param name="_keyboard"></param>
    /// <returns></returns>
    int getKeyboard(string _keyboard)
    {
        for (int i = 0; i < this.listKeyBoard.Count; i++)
        {
            if (this.listKeyBoard[i].namePainel == _keyboard)
                return i;
        }
        return -1;
    }
}
/// <summary>
/// Class que tem as propriedades de um Teclado
/// </summary>
public class StructKeyBoard
{
    public string namePainel;                         //Nome do painel
    public Vector2 anchored;                          //Representa o valor anchored do painel principal
    public Vector2 sizedelta;                         //Representa o valor sizedelta do painel principal
    public List<StrucKeyButton> listStructKeyButton;  //Lista de cada caracteristicas dos botões que serão salvos do teclado
}
/// <summary>
/// Classe que tem as propriedades de um Botão do teclado
/// </summary>
public class StrucKeyButton
{
    public bool active;                         //Variavel para verificar se o objeto está salvo
    public Vector2 anchored;                    //Represeta o valor anchored do botão
    public Vector2 sizedelta;                   //Representa o valor sizedelta do botão
    public string capital;                      //Valor que vai representar a string do botão Maiusculo
    public string tiny;                         //Valor que vai representar a string do botão Minusculo
}



