using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using LeaderboardCreatorDemo;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance;
    public int currentScore;
    public LeaderboardManager leaderboard;
    public GameObject inputOverlay;
    public GameObject keyboard;
    public TextMeshProUGUI scoreText;
    public InputField nameInput;
    public Image blackOverlay;
    public bool clear;
    public bool usingKeyboard;
    public bool caps;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        currentScore = PlayerPrefs.GetInt("score");
        blackOverlay.DOColor(new Color(0, 0, 0, 1), .5f).OnComplete(()=> { blackOverlay.DOColor(new Color(0, 0, 0, 0), .5f); });
    }
    private void Update()
    {
        scoreText.text = currentScore.ToString();
    }
    
    public void CloseOverlay()
    {
        inputOverlay.gameObject.SetActive(false);
    }

    public void SubmitScore()
    {
        leaderboard.UploadEntry(currentScore);
        CloseOverlay();
    }
    public void ToggleKeyboard()
    {
       usingKeyboard = !usingKeyboard;
        if (usingKeyboard)
        {
            nameInput.gameObject.transform.parent.DOLocalMoveY(120, 0.25f).SetEase(Ease.OutBack);
            keyboard.transform.DOLocalMoveY(-95, 0.25f).SetEase(Ease.OutBack);
        }
        else
        {
            nameInput.gameObject.transform.parent.DOLocalMoveY(0, 0.25f).SetEase(Ease.OutBack);
            keyboard.transform.DOLocalMoveY(-400, 0.25f).SetEase(Ease.OutBack);
        }
    }    
}
