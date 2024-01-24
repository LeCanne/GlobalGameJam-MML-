using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameValues : MonoBehaviour
{
    [Header("References")]
    public Slider timeSlider;

    [Header ("NumericalValues")]
    public int playerLife;
    public float reactTime;
    private float leftTime;
    private bool canReact;
    private bool ThereIsNoTimer;

    private void Start()
    {
        if (timeSlider == null)
        {
            ThereIsNoTimer = true;
        }
    }

    private void Update()
    {
        if (canReact)
        {
            if (!ThereIsNoTimer)
            {
                timeSlider.value = 1 - leftTime / reactTime;
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
            timeSlider.gameObject.SetActive(true);
        }
    }

    public void OnAddScore()
    {
        canReact = false;
        if (!ThereIsNoTimer)
        {
            timeSlider.gameObject.SetActive(false);
        }
    }

    public void OnLoseLife()
    {
        playerLife--;
        canReact = false;
        if (!ThereIsNoTimer)
        {
            timeSlider.gameObject.SetActive(false);
        }
        if (playerLife <= 0)
        {
            print("Death");
            //Die
        }
    }
}
