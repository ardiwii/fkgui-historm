using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] protected GameObject HintButton;
    [SerializeField] protected TutorialUI Tutorial;
    [SerializeField] ResultManager Result;

    public string StageName;
    public int StageMission;
    public int StageId;
    public TutorialImageSO TutorialAsset;
    public Difficulty CurrentDifficulty { get; private set; }

    public int Star;
    public float time;
    public int Mistake;

    public AvatarView avatar;

    protected float hintTimer;
    protected bool isHintActive;
    private bool isPaused;

    public Criteria[] StarCriteria = new Criteria[3];

    public static StageManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SoundManager.PlayMusic(SoundManager.Asset.StageMusic);
        isPaused = false;
        avatar.SetExpression(Expression.thinking);
        DisableHint();
        Initialize();
    }

    protected virtual void Initialize()
    {

    }

    public void Finished()
    {
        isPaused = true;
        StageFinished();
        Result.Setup();
    }

    protected virtual void StageFinished()
    {

    }

    protected virtual void SetDifficulty()
    {
        CurrentDifficulty = Difficulty.normal;
    }

    public virtual void MakeMistake()
    {
        SoundManager.PlaySound(SoundManager.Asset.Mistake);
        Mistake++;
        avatar.SetTimedExpression(Expression.sad, 2f, Expression.thinking);
    }

    public virtual void GetStar(int Number)
    {
        Star+=Number;
    }

    public bool CheckCriteria(Criteria crit)
    {
        if(time<=crit.time && Mistake <= crit.Mistake)
        {
            Debug.Log("true");
            return true;
        }
        else
        {
            Debug.Log("false");
            return false;
        }
    }

    public int CheckAllCriteria()
    {
        int crownAchieved = 0;
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("checking criteria " + i);
            if (CheckCriteria(StarCriteria[i]))
            {
                crownAchieved++;
            }
        }
        return crownAchieved;
    }

    protected virtual void Update()
    {
        if(CheatEnabled) EnableCheat();
        if (!isPaused)
        {
            time += Time.deltaTime;

            if (hintTimer > 0)
            {
                hintTimer -= Time.deltaTime;
                if (hintTimer <= 0)
                {
                    HintButton.SetActive(true); //activate hintbutton but not set isHintActive to true
                }
            }
        }

    }

    public void SetGamePaused(bool _isPaused)
    {
        isPaused = _isPaused;
    }

    bool CheatEnabled = false;

    public virtual void EnableCheat()
    {

    }

    public void QuitGame()
    {
        SceneSwitcher.instance.GoToMenu();
    }

    public virtual void ActivateHint()
    {
        isHintActive = true;
        HintButton.SetActive(false);
    }

    protected virtual void DisableHint()
    {
        hintTimer = 60;
        isHintActive = false;
        HintButton.SetActive(false);
    }

    public void ButtonClickSound()
    {
        SoundManager.PlaySound(SoundManager.Asset.ButtonTap);
    }
}

[System.Serializable]
public struct Criteria
{
    public int Mistake;
    public float time;
}

