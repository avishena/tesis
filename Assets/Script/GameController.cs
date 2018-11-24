using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Text questionText;
    public Text questionTextImage;
    public Text scoreDisplayText;
    public Text timeRemainingDisplayText;
    public Image questionImageDisplay;
    public SimpleObjectPool answerButtonObjectPool;
    public Transform answerButtonParent;

    public GameObject questionDisplay;
    public GameObject roundEndDisplay;
    public Text highScoreDisplay;
    public Text playerScoreDisplay;

    public Animator playerAnimator;
    public Animator enemyAnimator;

    public Text playerHealth;
    public Text enemyTime;
    public Slider enemyHealth;

    public AudioClip attackSound;
    public AudioClip dragonSound;
    public AudioClip rightHookSound;
    public AudioClip leftHookSound;
    public AudioClip clickSound;

    private LevelScreenController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;

    private bool isRoundActive;
    private bool playerAttackStat = false;
    private bool playerDieStat = false;
    private bool enemyAttackStat = false;
    private float timeRemaining;
    private float playerHealthRemaining;
    private float enemyHealthRemaining;
    private int questionIndex;
    private int playerScore;
    private Sprite questionImage;
    private AudioSource source;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();

    float counter = 0;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start ()
    {
        questionText.enabled = false;
        questionTextImage.enabled = false;
        questionImageDisplay.enabled = false;
        dataController = FindObjectOfType<LevelScreenController>();
        currentRoundData = dataController.GetCurrentRoundData();
        questionPool = currentRoundData.questions;

        timeRemaining = currentRoundData.timeLimitInSeconds;
        // enemyTime.maxValue = timeRemaining;
        // enemyTime.minValue = 0f;
        UpdateTimeRemainingDisplay();
        playerHealthRemaining = 30f;
        playerHealth.text = playerHealthRemaining.ToString();
        // playerHealth.maxValue = playerHealthRemaining;
        // playerHealth.minValue = 0f;
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
        if (questionData.hasImage)
        {
            questionText.enabled = false;
            questionTextImage.enabled = true;
            questionImageDisplay.enabled = true;
            questionImage = Resources.Load<Sprite>(questionData.imagePath);
            questionImageDisplay.sprite = questionImage;
            questionTextImage.text = questionData.questionText;
            
        } else
        {
            questionText.enabled = true;
            questionTextImage.enabled = false;
            questionImageDisplay.enabled = false;
            questionText.text = questionData.questionText;
        }

        for (int i=0; i < questionData.answers.Length; i++)
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
            answerButtonGameObjects.Add(answerButtonGameObject);
            answerButtonGameObject.transform.SetParent(answerButtonParent, false);

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
        source.PlayOneShot(clickSound, 1f);
        if (isCorrect)
        {
            playerAttackStat = true;
            playerAnimator.SetTrigger("attack");
            source.PlayOneShot(attackSound, 1f);

            playerScore += currentRoundData.pointsAddedForCorrectAnswer;
            scoreDisplayText.text = playerScore.ToString();

            // enemyHealthRemaining -= currentRoundData.pointsAddedForCorrectAnswer;
            // UpdateEnemyHealthDisplay();

            timeRemaining = currentRoundData.timeLimitInSeconds;
            UpdateTimeRemainingDisplay();

        } else
        {
            enemyAttackStat = true;
            enemyAnimator.SetTrigger("attack");
            source.PlayOneShot(dragonSound, 1f);

            playerHealthRemaining -= currentRoundData.pointsAddedForCorrectAnswer;
            UpdatePlayerHealthDisplay();

            timeRemaining = currentRoundData.timeLimitInSeconds;
            UpdateTimeRemainingDisplay();

            if (playerHealthRemaining <= 0)
            {
                PlayerDead();
            }
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
        playerScoreDisplay.text = "" + playerScore;
        dataController.SubmitNewPlayerScore(playerScore, "highestScore" + currentRoundData.subject);
        highScoreDisplay.text = "" + dataController.getHighestPlayerScore("highestScore" + currentRoundData.subject).ToString();

        questionDisplay.SetActive(false);
        roundEndDisplay.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("opening");
    }

    private void UpdateTimeRemainingDisplay()
    {
        // timeRemainingDisplayText.text = "Time: " + Mathf.Round(timeRemaining).ToString();
        // enemyTime.value = timeRemaining;
        enemyTime.text = Mathf.Round(timeRemaining).ToString();
    }

    private void UpdatePlayerHealthDisplay()
    {
        // playerHealth.value = playerHealthRemaining;
        playerHealth.text = playerHealthRemaining.ToString();
    }

    private void UpdateEnemyHealthDisplay()
    {
        enemyHealth.value = enemyHealthRemaining;
    }

    private void PlayerDead()
    {
        playerDieStat = true;
        playerAnimator.SetTrigger("die");
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
                enemyAttackStat = true;
                enemyAnimator.SetTrigger("attack");
                source.PlayOneShot(dragonSound, 1f);

                playerHealthRemaining -= currentRoundData.pointsAddedForCorrectAnswer;
                UpdatePlayerHealthDisplay();

                timeRemaining = currentRoundData.timeLimitInSeconds;
                UpdateTimeRemainingDisplay();

                if (playerHealthRemaining > 0 && questionPool.Length > questionIndex + 1)
                {
                    questionIndex++;
                    ShowQuestion();
                }
                else
                {
                    PlayerDead();
                }
            }

            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack")) {

                if (playerAttackStat == true && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
                {
                    enemyAnimator.SetTrigger("attacked");
                    source.PlayOneShot(leftHookSound, 1f);
                    playerAttackStat = false;
                }
            }

            if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                if (enemyAttackStat == true && enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
                {
                    playerAnimator.SetTrigger("attacked");
                    source.PlayOneShot(rightHookSound, 1f);
                    enemyAttackStat = false;
                }
            }

            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("die"))
            {
                if (playerDieStat == true && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
                {
                    playerDieStat = false;
                    EndRound();
                }
            }
        }
	}
}
