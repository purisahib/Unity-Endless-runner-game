using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PS_Score : MonoBehaviour
{
    private float score = 0.0f;

    private int difficultyLevel = 1;
    private int maxDifficultyLevel = 10;
    private int scoreToNextLevel = 10;

    private bool is_death = false;

    public Text scoreText;
   
    private void Update() {
        if(is_death)
            return;

        if(score >= scoreToNextLevel)
            levelUp();

        score += Time.deltaTime;
        scoreText.text = ((int)score).ToString();
    }

    private void levelUp(){
        if(difficultyLevel == maxDifficultyLevel)
            return;

        scoreToNextLevel *= 2;
        difficultyLevel++;

        GetComponent<PS_CharacterController>().SetSpeed(difficultyLevel);
    }
    public void OnDeath(){
        is_death = true;
    }
}
