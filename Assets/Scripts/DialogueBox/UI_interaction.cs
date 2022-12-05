using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_interaction : MonoBehaviour
{
    [SerializeField] protected movement Player;

    [SerializeField] protected TMP_Text nameText;
    [SerializeField] protected TMP_Text dialogueText;
    [SerializeField] protected GameObject actionButton;
    [SerializeField] protected TMP_Text dialogueButton;
    [SerializeField] protected GameObject dialogueBox;//vai mudar para animator no fim

    [SerializeField] protected Dialogue _dialogue;
    [SerializeField] protected Sentence _sentence;
    [SerializeField] protected Quest _quest;
    [SerializeField] protected Item _item; 
    [SerializeField] protected Door _door;

    [SerializeField] protected bool _setDoor;
    [SerializeField] protected bool _unlocking;
    [SerializeField] protected bool _endDialogue;

    protected void Start()
    {
        _unlocking = _setDoor = false;
        _item.Initialize();
        _quest.Initialize();
        _dialogue.Initialize();
        dialogueBox.SetActive(false);
    }

    void ButtonSetUp()
    {
        actionButton.GetComponent<Button>().onClick.AddListener(DisplayNextSentence);
    }

    void ButtonSetDown()
    {
        actionButton.GetComponent<Button>().onClick.RemoveListener(DisplayNextSentence);
    }

    public void StartDialogue(Dialogue dialogue)
    {        
        _dialogue = dialogue;
        _sentence = _dialogue.sentences[0];
        ButtonSetUp();

        DisplayName(_sentence.name);
    
        DisplayFirstSentence();
    }

    public void EndDialogue()
    {
        if(actionButton != null)
            ButtonSetDown();

        _setDoor = false;
        _dialogue = null;
        _sentence = null;
        _item = null;
        _door = null;
    }

    public void SetItem(Item item)
    {
        _item = null;
        _item = item;
    }

    public void SetQuest(Quest quest)
    {
        _quest = null;
        _quest = quest;
    }

    public void SetDoor(Door door)
    {
        _door = null;
        _door = door;
        _setDoor = true;
    }

    public void CheckDoor(string buttonText)
    {
        if (!_door.locked)
        {
            if (buttonText == "Entrar" || buttonText == "Sair")
            {
                actionButton.GetComponent<Button>().onClick.AddListener(NextScene);
                ButtonSetDown();
            }
        }
        else
        {
            if (buttonText == "Usar")
            {
                actionButton.GetComponent<Button>().onClick.AddListener(UnlockingDoor);
                _unlocking = true;
            }
        }
    }

    public void NextScene()
    {
        actionButton.SetActive(false);
        SceneManager.LoadScene(_door.NextScene);
        EndDialogue();
    }

    public void UnlockingDoor()
    {
        if (Player.Search(_door.KeyItemName))
        {
            Player.UseItem(_door.KeyItemName);

            Sentence s = new Sentence();

            s.name = "";
            s.sentence = "Você conseguiu desbloquear a passagem...";
            s.buttonText = "Entrar";

            _dialogue.sentences.Add(s);
            
            s = new Sentence();
            s.name = "";
            s.sentence = "Entrando...";
            s.buttonText = "";
            
            _dialogue.sentences.Add(s);

            _dialogue.sentences.RemoveAt(0);
            _sentence = _dialogue.sentences[0];

            string sentence = _sentence.sentence;
            _door.locked = false;
            actionButton.GetComponent<Button>().onClick.AddListener(NextScene);
            StartCoroutine(TypeSentence(sentence));            
        }
        else
        {
            Debug.Log("Não tenho");
            string lockedsentence = "Não tenho o que é preciso para passar...";

            StartCoroutine(LockedDoor(lockedsentence));
        }

        actionButton.GetComponent<Button>().onClick.RemoveListener(UnlockingDoor);
        Debug.Log("unlocking");
        _unlocking = false;
    }

    void DisplayName(string name)
    {
        StartCoroutine(TypeName(name));
    }

    IEnumerator TypeName(string name)
    {
        nameText.text = "";
        foreach (char letter in name.ToCharArray())
        {
            nameText.text += letter;
            yield return null;
        }
    }

    void DisplayFirstSentence()
    {
        Debug.Log("primeira");
        string sentence = _sentence.sentence;

        StartCoroutine(TypeSentence(sentence));

        if (_setDoor)
            CheckDoor(_sentence.buttonText);
        else
        {
            if (_dialogue.sentences.Count == 1)
            {
                actionButton.GetComponent<Button>().onClick.AddListener(HidenText);
                if (!_quest.questDelivered && _quest.quest != "")
                {
                    Player.setQuest(_quest.quest);
                    _quest.questDelivered = true;
                }
            }
        }
    }

    public void DisplayNextSentence()
    {
        Debug.Log("segunda");

        if (_unlocking)
            return;

        StopAllCoroutines();
        if (_dialogue.sentences.Count > 1)
        {
            _dialogue.sentences.RemoveAt(0);
            _sentence = _dialogue.sentences[0];
        }
        string sentence = _sentence.sentence;

        if (_sentence.name != nameText.text)
            DisplayName(_sentence.name);

        if (_setDoor)
            CheckDoor(_sentence.buttonText);
        else if (_dialogue.sentences.Count == 1)
        {
            if (!_quest.questDelivered && _quest.quest != "")
            { 
                Player.setQuest(_quest.quest); 
                _quest.questDelivered = true;
            }
            if (dialogueButton.text == "Pegar")
            {
                Player.setItem(_item);
               _item.itemDelivered = true;
            }

            actionButton.GetComponent<Button>().onClick.AddListener(HidenText);
        }             
        
        

        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        actionButton.SetActive(false);

        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }

        dialogueButton.text = _sentence.buttonText;
        if(dialogueButton.text != "")
            actionButton.SetActive(true);
    }

    IEnumerator LockedDoor(string sentence)
    {
        actionButton.SetActive(false);
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        HidenText();
    }

    public void Texting(string text)
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(text));
    }

    public void AppearText()
    {
        dialogueBox.SetActive(true);
    }

    public void HidenText()
    {
        actionButton.GetComponent<Button>().onClick.RemoveAllListeners();
        StopAllCoroutines();
        dialogueText.text = "";
        dialogueBox.SetActive(false);
        actionButton.SetActive(true);
    }

    
}
