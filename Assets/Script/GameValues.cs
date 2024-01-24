using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameValues : MonoBehaviour
{
    public int playerLife;
    private int Score;
    public float reactTime;
    private float leftTime;
    private bool canReact;

    private void Update()
    {
        if (canReact)
        {
            leftTime -= Time.deltaTime;
            if (leftTime <= 0)
            {
                OnLoseLife();
            }
        }
    }

    public void StartTimer()
    {
        leftTime = reactTime;
        canReact = true;
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
        if (playerLife <= 0)
        {
            //Die
        }
    }
}
