using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static BlaBlaScript;

public class BlaBlaScript : MonoBehaviour
{
    #region groupeDeTexte
    public GameObject textObject;
    public Transform textSpawn;
    public ScrollRect scroller;
    public float scrollSpeed;
    private readonly List<Transform> InstanciatedTexts = new();
    private readonly List<int> availableNumbers = new();
    public float messagesSpacing;
    private bool finishedTalk;
    #endregion

    #region TextRegion
    
    [Header ("Dialogues")]
    public Dialogue[] dialogues;
    public Dialogue lastDialogue;

    [System.Serializable]
    public class Dialogue
    {
        public SingleDial[] dial;
    }

    [System.Serializable]
    public class SingleDial
    {
        public string text;
        public bool mustLaugh;
    }

    [Header("DialoguesVares")]
    private int dialogueId;
    private TextMeshProUGUI textBox;
    private int line;
    private int actualCharacter;
    [SerializeField] private float TypingSpeed;
    private float typeLeftTime;
    private bool isFinished;
    //private bool buttonPressed;
    [SerializeField] private AudioSource VoiceBoss;
    #endregion

    private void Start()
    {
        for (int i = 1; i < dialogues.Length; i++)
        {
            availableNumbers.Add(i);
        }
        dialogueId = 0;
        line = -1;
        AddTextBox();
    }

    private void Update()
    {
        if (!isFinished)
        {
            if (typeLeftTime <= 0)
            {
                ProgressInText();
            } else
            {
                typeLeftTime -= Time.deltaTime;
            }
            /*if (Input.GetKey(KeyCode.Space))
            {
                if (!buttonPressed)
                {
                    OnExpression(false);
                }
            } else
            {
                buttonPressed = false;
            }*/
        }
    }

    #region func

    public void ProgressInText ()
    {
        if (dialogueId == dialogues.Length)
        {
            if (actualCharacter < lastDialogue.dial.Length)
            {
                finishedTalk = false;
                actualCharacter++;
                textBox.text += lastDialogue.dial[line].text[actualCharacter - 1].ToString();
                typeLeftTime = TypingSpeed;
                VoiceBoss.Play();
            } else if (!finishedTalk)
            {
                finishedTalk = true;
                SendMessage("StartTimer");
            }
        } else if (actualCharacter < dialogues[dialogueId].dial[line].text.Length)
        {
            finishedTalk = false;
            actualCharacter++;
            textBox.text += dialogues[dialogueId].dial[line].text[actualCharacter - 1].ToString();
            typeLeftTime = TypingSpeed;
            VoiceBoss.Play();

        } else if (!finishedTalk)
        {
            finishedTalk = true;
            SendMessage("StartTimer");
        }
    }

    public void OnExpression(bool expression)
    {
        //buttonPressed = true;
        if (dialogueId == dialogues.Length)
        {
            if (actualCharacter == lastDialogue.dial[line].text.Length)
            {
                CheckExpression(expression);
                AddTextBox();
            } else
            {
                textBox.text = lastDialogue.dial[line].text;
                actualCharacter = lastDialogue.dial[line].text.Length;
            }
        } else
        {
            if (actualCharacter == dialogues[dialogueId].dial[line].text.Length)
            {
                CheckExpression(expression);
                AddTextBox();
            } else
            {
                textBox.text = dialogues[dialogueId].dial[line].text;
                actualCharacter = dialogues[dialogueId].dial[line].text.Length;
            }
        }
    }

    public void CheckExpression(bool expression)
    {
        if (dialogueId == dialogues.Length)
        {
            if (expression == lastDialogue.dial[line].mustLaugh)
            {
                SendMessage("OnAddScore");
            } else
            {
                SendMessage("OnLoseLife");
            }
        } else
        {
            if (expression == dialogues[dialogueId].dial[line].mustLaugh)
            {
                SendMessage("OnAddScore");
            } else
            {
                SendMessage("OnLoseLife");
            }
        }
        
    }

    public void NoExpression()
    {
        AddTextBox();
    }

    public void AddTextBox()
    {
        actualCharacter = 0;
        
        line++;
        if (dialogueId == dialogues.Length)
        {
            if (line >= lastDialogue.dial.Length)
            {
                EndGame();
                return;

            }
        } else if(line >= dialogues[dialogueId].dial.Length)
        {
            line = 0;
            if (availableNumbers.Count > 0)
            {
                int newIndex = availableNumbers[UnityEngine.Random.Range(0, availableNumbers.Count)];
                dialogueId = newIndex;
                availableNumbers.Remove(newIndex);
                availableNumbers.TrimExcess();
            } else
            {
                dialogueId = dialogues.Length;
            }
            isFinished = false;
        }
        InstanciatedTexts.Add(Instantiate(textObject, textSpawn.position, textSpawn.rotation, textSpawn).transform);
        textBox = InstanciatedTexts[^1].GetChild(0).GetComponent<TextMeshProUGUI>();
        textBox.text = null;
        Canvas.ForceUpdateCanvases();
        scroller.normalizedPosition = new Vector2(0, 0);
        Canvas.ForceUpdateCanvases();
    }

    public void EndGame()
    {
        print("EndGame");
        enabled = false;
    }
    #endregion
}
