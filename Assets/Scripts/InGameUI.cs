using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public GameObject StartGameCanvas;
    public GameObject EndGameCanvas;
    public GameObject InGameCanvas;

    // shortcuts
    [SerializeField] private Text DistanceText;
    [SerializeField] private InputField InitialsText;
    [SerializeField] private Button AddScoreButton;

    [SerializeField] private Text RankingText;
    [SerializeField] private Text FinalScoreText;

    [SerializeField] private Canvas AddScoreCanvas;
    [SerializeField] private Canvas FeedbackCanvas;


    private Vector3 addScoreCanvasOrig;
    private Vector3 feedbackCanvasOrig;

    private bool RankingIsSet = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.UI = this;

        addScoreCanvasOrig = AddScoreCanvas.transform.position;
        feedbackCanvasOrig = FeedbackCanvas.transform.position;

        // I guess the UI can kick this off
        GameManager.Reset();
    }

    public void Reset()
    {
        InitialsText.text = string.Empty;
        DistanceText.text = "0";
        RankingIsSet = false;
        ShowState(GameManager.CurrentState);
        AddScoreCanvas.transform.position = addScoreCanvasOrig;
        FeedbackCanvas.transform.position = new Vector3(10000, 10000);
    }

    private void Update()
    {
        if (GameManager.CurrentState == GameManager.GameState.INGAME)
        {
            DistanceText.text = DisplayableScore;
        }

        if (GameManager.CurrentState == GameManager.GameState.ENDGAME)
        {
            if (RankingIsSet == false && GameManager.Ranking != 0)
            {
                RankingIsSet = true;
                RankingText.text = "Ranking #" + GameManager.Ranking;
            }
        }

    }


    public void ShowState(GameManager.GameState state)
    {
        StartGameCanvas.SetActive(false);
        InGameCanvas.SetActive(false);
        EndGameCanvas.SetActive(false);
        switch (state)
        {
            case GameManager.GameState.PREGAME:
                StartGameCanvas.SetActive(true);
                break;

            case GameManager.GameState.INGAME:
                InGameCanvas.SetActive(true);
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


    public void AddScoreButtonPress()
    {
        GameManager.PostScore(InitialsText.text);
        AddScoreCanvas.transform.position = new Vector3(10000, 10000);
        FeedbackCanvas.transform.position = feedbackCanvasOrig; 
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
