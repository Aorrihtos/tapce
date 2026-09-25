
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System;

[System.Serializable]
public class RankData
{
    public string username;
    public int score;

    public RankData(string username, int score)
    {
        this.username = username;
        this.score = score;
    }
}

public class FinalScoreController : MonoBehaviour
{
    const string API_URL = "https://tapce-server-aorrihtos-aorrihtos-projects.vercel.app/api/rank";
    const string API_KEY = "N2E5NDFiZGUtY2RiNy00NTM4LWJkMTEtZGNmZWUzMzNhNTFh";

    [SerializeField]
    private GameObject _scoreRankTitle;

    [SerializeField]
    private GameObject _scoreErrorTitle;

    [SerializeField]
    private GameObject _loadingPanel;

    [SerializeField]
    private GameObject _errorPanel;

    private bool _canContinue = false;

    private TMP_Text _scoreTitleText;
    private TMP_Text _scoreErrorText;
    private int _finalScore = 0000;
    private string _username = "AAA";

    void Awake()
    {
        // Set the title of score
        _finalScore = PlayerPrefs.GetInt("lastScore");
        _username = PlayerPrefs.GetString("playerID");

        // Set score text for both error and rank panels
        _scoreTitleText = _scoreRankTitle.GetComponent<TMP_Text>();
        _scoreErrorText = _scoreErrorTitle.GetComponent<TMP_Text>();
        _scoreTitleText.SetText(_scoreTitleText.text + _finalScore.ToString().PadLeft(4, '0'));
        _scoreErrorText.SetText(_scoreErrorText.text + _finalScore.ToString().PadLeft(4, '0'));

        _errorPanel.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(PostScore());
    }

    private void Update()
    {
        if (_loadingPanel.activeSelf || _canContinue) return;
        StartCoroutine(EnableCanContinue());
    }

    private IEnumerator EnableCanContinue()
    {
        yield return new WaitForSeconds(1);
        _canContinue = true;
    }

    // Publish the player last score to the server
    private IEnumerator PostScore()
    {
        string data = "{ \"username\": \"" + _username + "\", \"score\": \"" + _finalScore + "\"}";
        using (UnityWebRequest request = UnityWebRequest.Post(API_URL, data, "application/json"))
        {
            request.SetRequestHeader("Authorization", API_KEY);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Se ha guardado OK");
                StartCoroutine(GetRank());
            }
            else
            {
                Debug.Log("Se ha producido un error: " + request.error);
                _errorPanel.SetActive(true);
                _loadingPanel.SetActive(false);
            }
        }
    }

    // Retrieves the rank ordered DESC
    private IEnumerator GetRank()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(API_URL))
        {
            request.SetRequestHeader("Authorization", API_KEY);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Set Rank Data
                List<RankData> jsonData = JsonConvert.DeserializeObject<List<RankData>>(request.downloadHandler.text);
                SetRankTableData(jsonData);
            }
            else
            {
                // Activate error panel
                _errorPanel.SetActive(true);
            }

            // hide loading panel
            _loadingPanel.SetActive(false);
        }
    }

    void SetRankTableData(List<RankData> rankData)
    {
        // Set the scores on the rank table
        GameObject.Find("First").GetComponent<TMP_Text>().SetText(rankData[0].username + "  " + rankData[0].score);
        GameObject.Find("Second").GetComponent<TMP_Text>().SetText(rankData[1].username + "  " + rankData[1].score);
        GameObject.Find("Third").GetComponent<TMP_Text>().SetText(rankData[2].username + "  " + rankData[2].score);
        GameObject.Find("Fourth").GetComponent<TMP_Text>().SetText(rankData[3].username + "  " + rankData[3].score);
        GameObject.Find("Fifth").GetComponent<TMP_Text>().SetText(rankData[4].username + "  " + rankData[4].score);
    }


    public void RetryGame(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && _canContinue)
        {
            SceneManager.LoadScene("StartScreen");
        }
    }

}
