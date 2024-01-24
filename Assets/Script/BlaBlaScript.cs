using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BlaBlaScript : MonoBehaviour
{
    #region groupeDeTexte
    public GameObject textObject;
    public Transform textSpawn;
    public Transform textUpLimit;
    public Transform textDownLimit;
    public ScrollRect scroller;
    public float scrollSpeed;
    private readonly List<Transform> InstanciatedTexts = new();
    public float messagesSpacing;
    private bool finishedTalk;
    #endregion

    #region TextRegion
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

    public Dialogue[] dialogues;
    private int dialogueId;
    private TextMeshProUGUI textBox;
    private int line;
    private int actualCharacter;
    [SerializeField] private float TypingSpeed;
    private float typeLeftTime;
    private bool isFinished;
    private bool buttonPressed;
    [SerializeField] private AudioSource VoiceBoss;
    #endregion

    private void Start()
    {
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
            if (Input.GetKey(KeyCode.Space))
            {
                if (!buttonPressed)
                {
                    OnExpression(false);
                }
            } else
            {
                buttonPressed = false;
            }
            
        }
        //Scroll();
    }

    public void ProgressInText ()
    {
        if (actualCharacter < dialogues[dialogueId].dial[line].text.Length)
        {
            finishedTalk = false;
            actualCharacter++;
            textBox.text += dialogues[dialogueId].dial[line].text[actualCharacter - 1].ToString();
            typeLeftTime = TypingSpeed;
            //VoiceBoss.Play();
            
        } else if (!finishedTalk)
        {
            finishedTalk = true;
            //SendMessage("StartTimer");
        }
    }

    public void OnExpression(bool expression)
    {
        buttonPressed = true;
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

    public void CheckExpression(bool expression)
    {
        if (expression == dialogues[dialogueId].dial[line].mustLaugh)
        {
            //SendMessage("OnAddScore");
        } else
        {
            //SendMessage("OnLoseLife");
        }
    }

    public void NoExpression()
    {
        AddTextBox();
    }

    public void Scroll()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            textSpawn.GetComponent<Rigidbody2D>().AddForce(Input.mouseScrollDelta.y * scrollSpeed * Vector2.up, ForceMode2D.Impulse);
        }
        if (textSpawn.position.y > textDownLimit.position.y)
        {
            textSpawn.GetComponent<Rigidbody2D>().AddForce(scrollSpeed * Time.deltaTime * Vector2.Distance(textSpawn.position, textDownLimit.position) * -Vector2.up, ForceMode2D.Force);
        } else if (InstanciatedTexts[0].position.y < textUpLimit.position.y)
        {
            textSpawn.GetComponent<Rigidbody2D>().AddForce(scrollSpeed * Time.deltaTime * Vector2.Distance(InstanciatedTexts[0].position, textUpLimit.position) * Vector2.up, ForceMode2D.Force);
        }

        
    }

    public void AddTextBox()
    {
        actualCharacter = 0;
        
        line++;
        if (line >= dialogues[dialogueId].dial.Length)
        {
            line = 0;
            dialogueId = UnityEngine.Random.Range(1, dialogues.Length);
            isFinished = false;
        }
        InstanciatedTexts.Add(Instantiate(textObject, textSpawn.position, textSpawn.rotation, textSpawn).transform);
        textBox = InstanciatedTexts[^1].GetChild(0).GetComponent<TextMeshProUGUI>();
        textBox.text = null;
        Canvas.ForceUpdateCanvases();
        scroller.normalizedPosition = new Vector2(0, 0);
        Canvas.ForceUpdateCanvases();
    }
}
