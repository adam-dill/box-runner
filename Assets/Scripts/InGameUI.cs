using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public GameObject StartGameCanvas;
    public GameObject EndGameCanvas;

    // shortcuts
    [SerializeField] private Text FinalScoreText;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.UI = this;

        // I guess the UI can kick this off
        GameManager.Reset();
    }

    public void Reset()
    {
        ShowState(GameManager.CurrentState);
    }
    

    public void ShowState(GameManager.GameState state)
    {
        StartGameCanvas.SetActive(false);
        EndGameCanvas.SetActive(false);
        switch (state)
        {
            case GameManager.GameState.PREGAME:
                StartGameCanvas.SetActive(true);
                break;

            case GameManager.GameState.ENDGAME:
                EndGameCanvas.SetActive(true);
                FinalScoreText.text = DisplayableScore + " meters.";
                break;

            default:
                break;
        }
    }



    //
    // UI Events
    //

    public void StartGameButtonPress()
    {
        GameManager.StartGame();
    }

    public void ReplayButtonPress()
    {
        GameManager.Reset();
    }


    private string DisplayableScore
    {
        get
        {
            // 0.3048 is meters in a foot
            return System.Math.Round((GameManager.Distance * 0.3048), 2).ToString(); 
        }
    }


}
