using System;
using System.IO;
using System.Net;
using UnityEngine;
using Assets.Scripts;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;


public static class Service
{
    private static string API = "http://games.adamdill.com/leaderboards/";
    private static int GAME_ID = 1;



    public static void GetHighScores()
    {
        string uri = String.Format(API + "?projectid={0}", GAME_ID);
        StaticCoroutine.Start(GetRequest(uri));
    }


    public static void AddHighScore(UserScore score)
    {
        string uri = string.Format(API + "?projectid={0}&name={1}&score={2}", GAME_ID, score.name, score.score);
        StaticCoroutine.Start(GetRequest(uri));
    }


    public static void GetRanking(float score)
    {
        string uri = String.Format(API + "?projectid={0}&score={1}", GAME_ID, score);
        StaticCoroutine.Start(GetRequest(uri));
    }


    private static IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                ServiceResponse response = JsonUtility.FromJson<ServiceResponse>(webRequest.downloadHandler.text);
                if (response.ranking > 0)
                    GameManager.Ranking = response.ranking;
                if (response.scores.Count > 0)
                    GameManager.Scores = response.scores;
                //Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }

}
