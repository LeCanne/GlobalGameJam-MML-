using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
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
    #endregion

    #region TextRegion
    [System.Serializable]
    public class Dialogue
    {
        public string[] texts;
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

            CheckInputs();
        }
        //Scroll();
    }

    public void ProgressInText ()
    {
        if (actualCharacter != dialogues[dialogueId].texts[line].Length)
        {
            actualCharacter++;
            textBox.text += dialogues[dialogueId].texts[line][actualCharacter - 1].ToString();
            typeLeftTime = TypingSpeed;
        }
    }

    public void CheckInputs()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!buttonPressed)
            {
                buttonPressed = true;
                if (actualCharacter == dialogues[dialogueId].texts[line].Length)
                {
                    AddTextBox();
                } else
                {
                    textBox.text = dialogues[dialogueId].texts[line];
                    actualCharacter = dialogues[dialogueId].texts[line].Length;
                }

            }
        } else
        {
            buttonPressed = false;
        }
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
        
        /*if (InstanciatedTexts.Count > 0)
        {
            for (int i = 0; i < InstanciatedTexts.Count; i++)
            {
                InstanciatedTexts[i].position += Vector3.up * messagesSpacing;
            }
        }*/
        
        line++;
        if (line >= dialogues[dialogueId].texts.Length)
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
