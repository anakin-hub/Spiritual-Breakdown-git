using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_interaction : MonoBehaviour
{
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

    [SerializeField] protected bool _endDialogue;

    protected void Start()
    {
        _item.Initialize();
        _quest.Initialize();
        _dialogue.Initialize();
        dialogueBox.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {        
        _dialogue = dialogue;
        _sentence = _dialogue.sentences[0];
        
        DisplayName(_sentence.name);

        DisplayFirstSentence();
    }

    public void StartDialogue(Sentence sentence)
    {
        _sentence = sentence;
        _dialogue.sentences.Add(sentence);

        DisplayName(_sentence.name);

        StartCoroutine(TypeSentence(_sentence.sentence));

        if (_door.NextScene != "")
        {
            if (_sentence.buttonText == "Entrar" || _sentence.buttonText == "Sair")
            {
                SceneManager.LoadScene(_door.NextScene);
            }
            if (_sentence.buttonText.StartsWith("Usar"))
            {
                movement player;
                player = FindObjectOfType<movement>();
                if (player != null)
                {
                    if (player.Search(_door.KeyItemName))
                    {
                        player.UseItem(_door.KeyItemName);
                        SceneManager.LoadScene(_door.NextScene);
                    }
                    else
                    {
                        string lockedsentence = "Não tenho o que é preciso para entrar...";
                        StartCoroutine(LockedDoor(lockedsentence));
                    }
                }
            }
        }
    }

    public void EndDialogue()
    {
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
    }

    public void ActivateFunction(UnityEngine.Events.UnityAction call)
    {
        actionButton.GetComponent<Button>().onClick.AddListener(call);
    }

    public void DesactivateFunction(UnityEngine.Events.UnityAction call)
    {
        actionButton.GetComponent<Button>().onClick.RemoveListener(call);
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
        string sentence = _sentence.sentence;
        if (_sentence.name != nameText.text)
            DisplayName(_sentence.name);
        StartCoroutine(TypeSentence(sentence));
        if (_dialogue.sentences.Count == 1)
        { 
            actionButton.GetComponent<Button>().onClick.AddListener(HidenText);
            if (!_quest.questDelivered && _quest.quest != "")
            {
                FindObjectOfType<movement>().setQuest(_quest.quest);
                _quest.questDelivered = true;
            }
        }
    }

    public void DisplayNextSentence()
    {
        StopAllCoroutines();
        if (_dialogue.sentences.Count > 1)
        {
            _dialogue.sentences.RemoveAt(0);
            _sentence = _dialogue.sentences[0];
        }
        string sentence = _sentence.sentence;

        if (_sentence.name != nameText.text)
            DisplayName(_sentence.name);

        if (_dialogue.sentences.Count == 1)
        {
            StartCoroutine(TypeSentence(sentence));
            if (!_quest.questDelivered && _quest.quest != "")
            { 
                FindObjectOfType<movement>().setQuest(_quest.quest); 
                _quest.questDelivered = true;
            }
            if (_sentence.buttonText == "Pegar")
            {
                FindObjectOfType<movement>().setItem(_item);
               _item.itemDelivered = true;
            }

            actionButton.GetComponent<Button>().onClick.AddListener(HidenText);

            return;
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
        actionButton.GetComponent<Button>().onClick.RemoveListener(HidenText);
        StopAllCoroutines();
        dialogueText.text = "";
        dialogueBox.SetActive(false);
        actionButton.SetActive(true);
    }

    
}
