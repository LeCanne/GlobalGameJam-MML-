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
            timeSlider.GetComponent<RectTransform>().parent.parent.gameObject.SetActive(false);
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
            timeSlider.GetComponent<RectTransform>().parent.parent.gameObject.SetActive(true);
        }
    }

    public void OnAddScore()
    {
        playerScore++;
        canReact = false;
        if (!ThereIsNoTimer)
        {
            timeSlider.GetComponent<RectTransform>().parent.parent.gameObject.SetActive(false);
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
            timeSlider.GetComponent<RectTransform>().parent.parent.gameObject.SetActive(false);
        }
        if (playerLife <= 0)
        {
            SendMessage("DieNow");
        }
    }
}
