using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Text questionText;
    public Text scoreDisplayText;
    public Text timeRemainingDisplayText;
    public SimpleObjectPool answerButtonObjectPool;
    public Transform answerButtonParent;

    public GameObject questionDisplay;
    public GameObject roundEndDisplay;
    public Text highScoreDisplay;

    public Animator playerAnimator;
    public Animator enemyAnimator;

    public Slider playerHealth;
    public Slider enemyHealth;
    public Slider enemyTime;

    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;

    private bool isRoundActive;
    private float timeRemaining;
    private float playerHealthRemaining;
    private float enemyHealthRemaining;
    private int questionIndex;
    private int playerScore;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
        dataController = FindObjectOfType<DataController>();
        currentRoundData = dataController.GetCurrentRoundData();
        questionPool = currentRoundData.questions;

        timeRemaining = currentRoundData.timeLimitInSeconds;
        enemyTime.maxValue = timeRemaining;
        enemyTime.minValue = 0f;
        UpdateTimeRemainingDisplay();

        playerHealthRemaining = 30f;
        playerHealth.maxValue = playerHealthRemaining;
        playerHealth.minValue = 0f;
        UpdatePlayerHealthDisplay();

        enemyHealthRemaining = 30f;
        enemyHealth.maxValue = enemyHealthRemaining;
        enemyHealth.minValue = 0f;
        UpdateEnemyHealthDisplay();

        playerScore = 0;
        questionIndex = 0;

        ShowQuestion();
        isRoundActive = true;
	}

    private void ShowQuestion()
    {
        RemoveAnswerButtons();
        QuestionData questionData = questionPool[questionIndex];
        questionText.text = questionData.questionText;

        for (int i=0; i < questionData.answers.Length; i++)
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
            answerButtonGameObjects.Add(answerButtonGameObject);
            answerButtonGameObject.transform.SetParent(answerButtonParent);

            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
            answerButton.Setup(questionData.answers[i]);
        }
    }

    private void RemoveAnswerButtons()
    {
        while (answerButtonGameObjects.Count > 0)
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
            answerButtonGameObjects.RemoveAt(0);
        }
    }

    public void AnswerButtonClicked(bool isCorrect)
    {
        if (isCorrect)
        {
            playerAnimator.SetTrigger("attack");
            enemyAnimator.SetTrigger("attacked");

            playerScore += currentRoundData.pointsAddedForCorrectAnswer;
            scoreDisplayText.text = "Score: " + playerScore.ToString();

            enemyHealthRemaining -= currentRoundData.pointsAddedForCorrectAnswer;
            UpdateEnemyHealthDisplay();

            timeRemaining = currentRoundData.timeLimitInSeconds;
            UpdateTimeRemainingDisplay();

        } else
        {
            enemyAnimator.SetTrigger("attack");
            playerAnimator.SetTrigger("attacked");

            playerHealthRemaining -= currentRoundData.pointsAddedForCorrectAnswer;
            UpdatePlayerHealthDisplay();

            timeRemaining = currentRoundData.timeLimitInSeconds;
            UpdateTimeRemainingDisplay();
        }

        if (questionPool.Length > questionIndex + 1)
        {
            questionIndex++;
            ShowQuestion();
        } else
        {
            EndRound();
        }
    }

    public void EndRound()
    {
        isRoundActive = false;
        dataController.SubmitNewPlayerScore(playerScore);
        highScoreDisplay.text = "High Score: " + dataController.getHighestPlayerScore().ToString();

        questionDisplay.SetActive(false);
        roundEndDisplay.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("opening");
    }

    private void UpdateTimeRemainingDisplay()
    {
        //timeRemainingDisplayText.text = "Time: " + Mathf.Round(timeRemaining).ToString();
        enemyTime.value = timeRemaining;
    }

    private void UpdatePlayerHealthDisplay()
    {
        playerHealth.value = playerHealthRemaining;
    }

    private void UpdateEnemyHealthDisplay()
    {
        enemyHealth.value = enemyHealthRemaining;
    }

    // Update is called once per frame
    void Update ()
    {
		if (isRoundActive)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay();

            if (timeRemaining <= 0f)
            {
                enemyAnimator.SetTrigger("attack");
                playerAnimator.SetTrigger("attacked");

                playerHealthRemaining -= currentRoundData.pointsAddedForCorrectAnswer;
                UpdatePlayerHealthDisplay();

                timeRemaining = currentRoundData.timeLimitInSeconds;
                UpdateTimeRemainingDisplay();

                if (questionPool.Length > questionIndex + 1)
                {
                    questionIndex++;
                    ShowQuestion();
                }
                else
                {
                    EndRound();
                }
            }
        }
	}
}
