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
    private bool isDead;
    #endregion

    #region Autres
    [SerializeField] private Animator bossAnims;
    [SerializeField] private Animator playerAnims;
    [SerializeField] private AudioSource VoiceBoss;
    [SerializeField] private Vector2 bossPitchRange;
    #endregion

    private void Start()
    {
        for (int i = 1; i < dialogues.Length - 2; i++)
        {
            availableNumbers.Add(i);
        }
        dialogueId = 0;
        line = -1;
        AddTextBox();
        bossAnims.SetTrigger("To_Idle");
        bossAnims.SetTrigger("To_Not_Turn");
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
        }
    }

    #region func

    public void ProgressInText ()
    {
        if (actualCharacter < dialogues[dialogueId].dial[line].text.Length)
        {
            finishedTalk = false;
            actualCharacter++;
            textBox.text += dialogues[dialogueId].dial[line].text[actualCharacter - 1].ToString();
            typeLeftTime = TypingSpeed;
            VoiceBoss.pitch = UnityEngine.Random.Range(bossPitchRange.x, bossPitchRange.y);
            VoiceBoss.Play();

        } else if (!finishedTalk)
        {
            finishedTalk = true;
            SendMessage("StartTimer");
        }
    }

    public void OnExpression(bool expression)
    {
        {
            if (actualCharacter == dialogues[dialogueId].dial[line].text.Length)
            {
                CheckExpression(expression);
                AddTextBox();
            } else if (!expression)
            {
                textBox.text = dialogues[dialogueId].dial[line].text;
                actualCharacter = dialogues[dialogueId].dial[line].text.Length;
            }
        }
    }

    public void CheckExpression(bool expression)
    {
        {
            if (expression == dialogues[dialogueId].dial[line].mustLaugh)
            {
                SendMessage("OnAddScore");
                bossAnims.SetTrigger("To_Laugh");
                bossAnims.SetTrigger("To_Win");
            } else
            {
                SendMessage("OnLoseLife");
                bossAnims.SetTrigger("To_Cringed");
                playerAnims.SetTrigger("To_Lose");
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
        if(line >= dialogues[dialogueId].dial.Length)
        {
            if (dialogueId >= dialogues.Length - 2)
            {
                EndGame();
                return;

            }
            line = 0;
            if (availableNumbers.Count > 0)
            {
                int newIndex = availableNumbers[UnityEngine.Random.Range(0, availableNumbers.Count)];
                dialogueId = newIndex;
                availableNumbers.Remove(newIndex);
                availableNumbers.TrimExcess();
            } else
            {
                if (isDead)
                {
                    dialogueId = dialogues.Length - 2;
                } else
                {
                    dialogueId = dialogues.Length - 1;
                }
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

    public void DieNow()
    {
        isDead = true;
        availableNumbers.Clear();
    }

    public void EndGame()
    {
        print("EndGame");
        enabled = false;
    }
    #endregion
}
