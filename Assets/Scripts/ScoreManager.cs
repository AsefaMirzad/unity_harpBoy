using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ScoreManager Instance;

    public int score = 0;
   

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Perfect()
    {
        score += 100;
        //(UpdateScore();
        Debug.Log("Perfect!");
    }

    public void Good()
    {
        score += 50;
        //UpdateScore();
        Debug.Log("Good!");
    }

    public void Miss()
    {
        score -= 50;
        //UpdateScore();
        Debug.Log("Miss!");
    }

    //private void UpdateScore()
    //{
    //    scoreText.text = "Score: " + score.ToString();
    //}
}
