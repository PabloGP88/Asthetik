using TMPro;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Movment playerMovement;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayeCollisionManager player;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject FinishUI;
    [SerializeField] private GameObject GameUI;
    public int goalsAmount;
    public bool haveWon = false;
    private bool havePlayedMusic = false;
    private const string TEXT_FADE_STRING = "isOn";

    private void OnEnable() => player.OnPlayerHitGoal += HandlePlayerHitGoal;
    private void OnDisable() => player.OnPlayerHitGoal -= HandlePlayerHitGoal;

    void Start()
    {
        goalsAmount = PlayerPrefs.GetInt("AmountStrokes", 8);
    }

    private void Update()
    {
        HandlePlayerInput();
        timerText.text = goalsAmount.ToString();

        if (goalsAmount <= 0)
        {
            haveWon = true;
        }

        if (haveWon && !havePlayedMusic)
        {
            audioSource.Play();

            FinishUI.SetActive(true);
            GameUI.SetActive(false);

            havePlayedMusic = true;
        }

    }

    private void HandlePlayerInput()
    {
        if (playerMovement.inputActive)
        {   
            animator.SetBool(TEXT_FADE_STRING,true);
        } else animator.SetBool(TEXT_FADE_STRING,false);
        
    }

    private void HandlePlayerHitGoal()
    {
        goalsAmount--;
    }
}
