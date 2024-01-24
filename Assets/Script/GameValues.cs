using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameValues : MonoBehaviour
{
    [Header("References")]
    public Slider timeSlider;

    [Header ("NumericalValues")]
    public int playerLife;
    private int Score;
    public float reactTime;
    private float leftTime;
    private bool canReact;

    private void Update()
    {
        if (canReact)
        {
            timeSlider.value = 1 - leftTime / reactTime;
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
        timeSlider.SetEnabled(true);
    }

    public void OnAddScore()
    {
        Score++;
        canReact = false;
    }

    public void OnLoseLife()
    {
        playerLife--;
        canReact = false;
        timeSlider.SetEnabled(false);
        if (playerLife <= 0)
        {
            //Die
        }
    }
}
