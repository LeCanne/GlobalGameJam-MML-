using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region gameStart
    public Image TransitionScene;
    private float timerTransition;
    private bool updateTransition;
    #endregion
    #region globalGameManager
    public float timer;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (updateTransition == true)
        {
            StartGame(0);
        }
       
    }

    public void StartGame(int sceneChosen)
    {
        updateTransition = true;
        Color alpha = TransitionScene.color;
        timerTransition += Time.deltaTime;
        
        if(timerTransition >= 0.05f)
        {
            alpha.a += 0.05f;
            TransitionScene.color = alpha;
            timerTransition = 0;
        }
        if(TransitionScene.color.a >= 1)
        {
            ChangeScene(sceneChosen);
            Debug.Log("ye");
        }
    }

    public void ChangeScene(int sceneChosen)
    {
        SceneManager.LoadScene(sceneChosen);
       
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
