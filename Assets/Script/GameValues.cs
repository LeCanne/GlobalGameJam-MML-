using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameValues : MonoBehaviour
{
    public int playerLife;
    private int Score;
    public float reactTime;
    private bool canReact;

    public void OnAddScore()
    {
        Score++;
    }

    public void OnLoseLife()
    {
        playerLife--;
        if (playerLife <= 0)
        {
            //Die
        }
    }
}
