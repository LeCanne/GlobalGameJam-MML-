using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameValues : MonoBehaviour
{
    [Header("References")]
    public Slider timeSlider;
    public TextMeshProUGUI scoreText;
    public Animator[] lifeAnims;

    [Header ("NumericalValues")]
    public int playerLife;
    private int playerScore;
    public float reactTime;
    private float leftTime;
    private bool canReact;
    private bool ThereIsNoTimer;
    private bool ThereIsNoScore;

    private void Start()
    {
        if (timeSlider == null)
        {
            ThereIsNoTimer = true;
        } else
        {
            foreach(RectTransform child in timeSlider.GetComponent<RectTransform>().parent.parent)
            {
                if (child.TryGetComponent<Animator> (out Animator anim))
                {
                    anim.SetTrigger("Closed");
                }
            }
        }

        if (scoreText == null)
        {
            ThereIsNoScore = true;
        }
    }

    private void Update()
    {
        if (canReact)
        {
            if (!ThereIsNoTimer)
            {
                timeSlider.value = leftTime / reactTime;
            }
            leftTime -= Time.deltaTime;
            if (leftTime <= 0)
            {
                SendMessage("NoExpression");
                OnLoseLife();
            }
        }
    }

    public void StartTimer()
    {
        leftTime = reactTime;
        canReact = true;
        if (!ThereIsNoTimer)
        {
            foreach (RectTransform child in timeSlider.GetComponent<RectTransform>().parent.parent)
            {
                if (child.TryGetComponent<Animator>(out Animator anim))
                {
                    anim.SetTrigger("Spawn");
                }
            }
        }
    }

    public void OnAddScore()
    {
        playerScore++;
        canReact = false;
        if (!ThereIsNoTimer)
        {
            foreach (RectTransform child in timeSlider.GetComponent<RectTransform>().parent.parent)
            {
                if (child.TryGetComponent<Animator>(out Animator anim))
                {
                    anim.SetTrigger("Closed");
                }
            }
        }
        if(!ThereIsNoScore)
        {
            scoreText.text = (playerScore * 1000).ToString();
        }
    }

    public void OnLoseLife()
    {
        playerLife--;
        canReact = false;
        if (!ThereIsNoTimer)
        {
            foreach (RectTransform child in timeSlider.GetComponent<RectTransform>().parent.parent)
            {
                if (child.TryGetComponent<Animator>(out Animator anim))
                {
                    anim.SetTrigger("Closed");
                }
            }

            for (int i = 0; i < lifeAnims.Length; i++)
            {
                if (i >= playerLife)
                {
                    lifeAnims[i].SetTrigger("Lost");
                }
            }
        }
        if (playerLife <= 0)
        {
            SendMessage("DieNow");
        }
    }
}
