using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Text questionText;
    public Text questionTextImage;
    public Text questionTextImageDiagram;
    public Text scoreDisplayText;
    public Text timeRemainingDisplayText;
    public Image questionImageDisplay;
    public Image questionImageDiagramDisplay;
    public SimpleObjectPool answerButtonObjectPool;
    public Transform answerButtonParent;

    public GameObject questionDisplay;
    public GameObject roundEndDisplay;
    public Text highScoreDisplay;
    public Text playerScoreDisplay;
    public Text bestTimeDisplay;
    public Text playerTimeDisplay;

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
    public AudioClip loseSound;
    public AudioClip winSound;

    private LevelScreenController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;

    private bool isRoundActive;
    private bool playerAttackStat = false;
    private bool playerDieStat = false;
    private bool enemyAttackStat = false;
    private float timeRemaining;
    private float timeRemainingStart;
    private float playerSpeed;
    private float playerHealthRemaining;
    private float enemyHealthRemaining;
    private int questionIndex;
    private int playerScore;
    private int playerScoreNow;
    private int playerTime;
    private int attackValue;
    private int attackValueNow;
    private Sprite questionImage;
    private AudioSource source;
    private AudioSource openingSource;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();

    float counter = 0;

    // DDA attributes
    private float scoreMax = 100f;
    private float healthMax = 100f;
    private float powerMax = 10f;
    private float actionMax = 210f;
    private float speedMax = 210f;
    private float scoreMin = 56f;
    private float healthMin = 0f;
    private float actionMin = 0f;
    private float powerMin = 0f;
    private float speedMin = 60f;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        openingSource = GameObject.FindWithTag("opening").GetComponent<AudioSource>();
        openingSource.Stop();
        source.Play();

        //Destroy(openingSource);
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
        timeRemainingStart = currentRoundData.timeLimitInSeconds;
        attackValue = currentRoundData.pointsAddedForCorrectAnswer;
        attackValueNow = attackValue;
        // enemyTime.maxValue = timeRemaining;
        // enemyTime.minValue = 0f;
        UpdateTimeRemainingDisplay(timeRemaining);
        playerHealthRemaining = 100f;
        playerHealth.text = playerHealthRemaining.ToString();
        // playerHealth.maxValue = playerHealthRemaining;
        // playerHealth.minValue = 0f;
        UpdatePlayerHealthDisplay();

        enemyHealthRemaining = 30f;
        enemyHealth.maxValue = enemyHealthRemaining;

        enemyHealth.minValue = 0f;
        UpdateEnemyHealthDisplay();

        playerScore = 0;
        playerTime = 0;
        playerScoreNow = 0;
        questionIndex = 0;
        playerSpeed = 0f;

        ShowQuestion();
        isRoundActive = true;
	}

    private void ShowQuestion()
    {
        source.Play();
        RemoveAnswerButtons();
        QuestionData questionData = questionPool[questionIndex];
        if (questionData.hasImage)
        {
            if (questionData.diagram)
            {
                questionText.enabled = false;
                questionTextImage.enabled = false;
                questionImageDisplay.enabled = false;
                questionTextImageDiagram.enabled = true;
                questionImageDiagramDisplay.enabled = true;
                questionImage = Resources.Load<Sprite>(questionData.imagePath);
                questionImageDiagramDisplay.sprite = questionImage;
                questionTextImageDiagram.text = questionData.questionText;
            } else
            {
                questionText.enabled = false;
                questionTextImageDiagram.enabled = false;
                questionImageDiagramDisplay.enabled = false;
                questionTextImage.enabled = true;
                questionImageDisplay.enabled = true;
                questionImage = Resources.Load<Sprite>(questionData.imagePath);
                questionImageDisplay.sprite = questionImage;
                questionTextImage.text = questionData.questionText;
            }
            
        } else
        {
            questionText.enabled = true;
            questionTextImage.enabled = false;
            questionImageDisplay.enabled = false;
            questionTextImageDiagram.enabled = false;
            questionImageDiagramDisplay.enabled = false;
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
            playerSpeed = timeRemainingStart - timeRemaining;
            playerTime += Mathf.RoundToInt(playerSpeed);
            playerAttackStat = true;
            playerAnimator.SetTrigger("attack");
            source.PlayOneShot(attackSound, 1f);

            playerScore += currentRoundData.pointsAddedForCorrectAnswer;
            playerScoreNow = currentRoundData.pointsAddedForCorrectAnswer;
            scoreDisplayText.text = playerScore.ToString();
            attackValue = 0;

            // enemyHealthRemaining -= currentRoundData.pointsAddedForCorrectAnswer;
            // UpdateEnemyHealthDisplay();

        } else
        {
            playerSpeed = timeRemainingStart;
            playerTime += Mathf.RoundToInt(timeRemainingStart - timeRemaining);
            enemyAttackStat = true;
            enemyAnimator.SetTrigger("attack");
            source.PlayOneShot(dragonSound, 1f);

            // attackValue = currentRoundData.pointsAddedForCorrectAnswer;
            playerHealthRemaining -= attackValue;
            UpdatePlayerHealthDisplay();

            // timeRemaining = currentRoundData.timeLimitInSeconds;
            // UpdateTimeRemainingDisplay();

            if (playerHealthRemaining <= 0)
            {
                PlayerDead();
            }
        }

        if (questionPool.Length > questionIndex + 1)
        {
            /// ADJUSTMENT
            adjustSvbpAttribute();
            adjustBvbpAttribute();
            playerSpeed = 0f;
            playerScoreNow = 0;
            // attackValue = currentRoundData.pointsAddedForCorrectAnswer;

            questionIndex++;
            ShowQuestion();

            // timeRemaining = currentRoundData.timeLimitInSeconds;
            // UpdateTimeRemainingDisplay();
        } else
        {
            EndRound();
            source.Stop();
            source.PlayOneShot(winSound, 1f);
        }
    }

    public void EndRound()
    {
        isRoundActive = false;
        playerScoreDisplay.text = "" + playerScore;
        playerTimeDisplay.text = "" + playerTime;
        dataController.SubmitNewPlayerScore(playerScore, "highestScore" + currentRoundData.subject);
        dataController.SubmitNewPlayerTime(playerTime, "bestTime" + currentRoundData.subject);
        highScoreDisplay.text = "" + dataController.getHighestPlayerScore("highestScore" + currentRoundData.subject).ToString();
        bestTimeDisplay.text = "" + dataController.getBestPlayerTime("bestTime" + currentRoundData.subject).ToString();

        questionDisplay.SetActive(false);
        roundEndDisplay.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("opening");
        GameObject[] objs = GameObject.FindGameObjectsWithTag("opening");

        if (objs.Length > 0)
        {
            Destroy(objs[0]);
        }
    }

    private void UpdateTimeRemainingDisplay(float time)
    {
        // timeRemainingDisplayText.text = "Time: " + Mathf.Round(timeRemaining).ToString();
        // enemyTime.value = timeRemaining;
        enemyTime.text = Mathf.Round(time).ToString();
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

    private float countTotalEf(float playerScore, float playerSpeed, float enemyPower, float enemySpeed)
    {
        float totalEf;
        float weightBvbp = 0.8176f;
        float weightSvbp = 0.1824f;

        totalEf = (playerScore * weightBvbp) + (playerSpeed * weightSvbp) + (enemyPower * weightBvbp) + (enemySpeed + weightSvbp);

        return totalEf;
    }

    private float countTotalEfSvbp(float playerSpeed, float enemySpeed)
    {
        float totalEf;
        float weightSvbp = 0.1824f;

        totalEf = (playerSpeed * weightSvbp) + (enemySpeed + weightSvbp);

        return totalEf;
    }

    // check perkiraan kemampuan pemain
    private void adjustBvbpAttribute()
    {
        float averageScore = Mathf.Round(playerScore / (questionIndex + 1)) * 10;

        //Debug.Log("score : " + averageScore);
        //Debug.Log("attack : " + attackValue);

        float efPlayerScore = (averageScore - scoreMin) / (scoreMax - scoreMin);
        float efEnemyPower = (attackValue - powerMin) / (powerMax - powerMin);

        //Debug.Log("ef score : " + efPlayerScore);
        //Debug.Log("ef attack : " + efEnemyPower);

        float totalEf = countTotalEf(averageScore, playerSpeed, attackValue, timeRemaining);
        float abilities = totalEf / 1;

        float diffef = Mathf.Abs(efPlayerScore - efEnemyPower);
        bool decision = false;
        decision = diffef > (0.05 * efPlayerScore);

        //Debug.Log("diffef : " + diffef);

        float adj = diffef * (powerMax - powerMin);

        //Debug.Log("adj : " + adj);
        //Debug.Log("attack before : " + attackValueNow);

        if (efPlayerScore > efEnemyPower)
        {
            attackValue = Mathf.RoundToInt(Mathf.Round(attackValueNow) + adj);
            attackValueNow = attackValue;
        }
        else
        {
            if ((Mathf.RoundToInt(Mathf.Round(attackValueNow) - adj)) > 0) {
                attackValue = Mathf.RoundToInt(Mathf.Round(attackValueNow) - adj);
                attackValueNow = attackValue;
            }
        }

        //Debug.Log("attack after : " + attackValueNow);
    }

    private void adjustSvbpAttribute()
    {

        //Debug.Log("player : " + playerSpeed);
        //Debug.Log("enemy : " + timeRemaining);

        float efPlayerSpeed = (actionMax - playerSpeed) / (actionMax - actionMin);
        float efEnemySpeed = (speedMax - timeRemaining) / (speedMax - speedMin);

        float averageScore = Mathf.Round(playerScore / (questionIndex + 1)) * 10;

        //Debug.Log("speed player : " + efPlayerSpeed);
        //Debug.Log("speed enemy : " + efEnemySpeed);

        float totalEf = countTotalEf(averageScore, playerSpeed, attackValue, timeRemaining);
        // float totalEf = countTotalEfSvbp(playerSpeed, timeRemaining);
        float abilities = totalEf / 1;

        float diffef = Mathf.Abs(efPlayerSpeed - efEnemySpeed);
        bool decision = false;
        decision = diffef > (0.05 * efPlayerSpeed);

        float adj = -diffef * (speedMax - speedMin);

        //Debug.Log("speed before : " + timeRemainingStart);

        if (efPlayerSpeed > efEnemySpeed)
        {
            timeRemaining = Mathf.RoundToInt(Mathf.Round(timeRemainingStart) + adj);
            timeRemainingStart = timeRemaining;
            UpdateTimeRemainingDisplay(timeRemaining);
        }
        else
        {
            timeRemaining = Mathf.RoundToInt(Mathf.Round(timeRemainingStart) - adj);
            timeRemainingStart = timeRemaining;
            UpdateTimeRemainingDisplay(timeRemaining);
        }

        //Debug.Log("speed after : " + timeRemainingStart);
    }

    // Update is called once per frame
    void Update ()
    {
		if (isRoundActive)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay(timeRemaining);

            if (timeRemaining <= 0f)
            {
                enemyAttackStat = true;
                enemyAnimator.SetTrigger("attack");
                source.PlayOneShot(dragonSound, 1f);

                playerHealthRemaining -= currentRoundData.pointsAddedForCorrectAnswer;
                UpdatePlayerHealthDisplay();

                adjustSvbpAttribute();
                // timeRemaining = currentRoundData.timeLimitInSeconds;
                // UpdateTimeRemainingDisplay(timeRemaining);

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
                    source.Stop();
                    source.PlayOneShot(loseSound, 1f);
                }
            }
        }
	}
}
