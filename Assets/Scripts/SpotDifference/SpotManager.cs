using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SpotManager : StageManager
{
    [SerializeField] private GameObject foundSpotPrefab;
    [SerializeField] private SpotsToFindSO spotsData;
    [SerializeField] private FtsImageController leftImageController;
    [SerializeField] private FtsImageController rightImageController;
    [SerializeField] private TextMeshProUGUI spotCountText;
    [SerializeField] private float spotTolerance;
    [SerializeField] private PostGameQuizManager postQuizManager;
    [SerializeField] private GameObject postQuizCanvas;
    [SerializeField] private GameObject mainGameCanvas;

    private HashSet<int> foundSpotIdxs;
    private int foundSpotCount;
    private int spotCount;

    protected override void Initialize()
    {
        base.Initialize();
        leftImageController.SetImage(spotsData.fakeImage);
        rightImageController.SetImage(spotsData.realImage);
        foundSpotCount = 0;
        spotCount = spotsData.spotsPositions.Count;
        spotCountText.text = foundSpotCount + "/" + spotCount;
        foundSpotIdxs = new HashSet<int>();
        if (!PlayerPrefs.HasKey("ftsTutorialDone"))
        {
            Tutorial.Setup();
            PlayerPrefs.SetInt("ftsTutorialDone", 1);
        }
    }

    public void CheckSpot(Vector2 spotPos)
    {
        bool spotFound = false;
        for (int i = 0; i < spotsData.spotsPositions.Count && !spotFound; i++)
        {
            Debug.Log("distance: " + Vector2.Distance(spotPos, spotsData.spotsPositions[i]));
            if (Vector2.Distance(spotPos, spotsData.spotsPositions[i]) < spotTolerance)
            {
                spotFound = true;
                if (!foundSpotIdxs.Contains(i)) //spot is found for the first time, if spot is already found, dont add mistake but also dont do anything
                {
                    SoundManager.PlaySound(SoundManager.Asset.GetStar);
                    leftImageController.SpawnSpottedMarker(spotsData.spotsPositions[i]);
                    rightImageController.SpawnSpottedMarker(spotsData.spotsPositions[i]);
                    foundSpotIdxs.Add(i);
                    foundSpotCount++;
                    spotCountText.text = foundSpotCount + "/" + spotCount;
                    if (isHintActive)
                    {
                        DisableHint();
                        leftImageController.HideHint();
                    }
                }
            }
        }
        if (!spotFound)
        {
            Debug.Log("mistake");
            MakeMistake();
        }
        if (foundSpotCount == spotCount)
        {
            isHintActive = false;
            HintButton.SetActive(false);
            hintTimer = -1;
            DOVirtual.DelayedCall(1f, delegate
            {
                mainGameCanvas.SetActive(false);
                postQuizCanvas.SetActive(true);
                postQuizManager.InitializeQuiz();
            });
        }
    }

    public override void ActivateHint()
    {
        base.ActivateHint();
        bool spotFound = false;
        Vector2 bottomLeftLimit = new Vector2(-230, -182);
        Vector2 topRightLimit = new Vector2(230, 182);
        for (int i = 0; i < spotsData.spotsPositions.Count && !spotFound; i++)
        {
            if (!foundSpotIdxs.Contains(i))
            {
                spotFound = true;
                Vector2 spot = spotsData.spotsPositions[i];
                Vector2 hintPosition = new Vector2(spot.x - 80 + Random.Range(0, 160), spot.y - 40 + Random.Range(0, 80));
                if (hintPosition.x < bottomLeftLimit.x) hintPosition.x = bottomLeftLimit.x;
                else if (hintPosition.x > topRightLimit.x) hintPosition.x = topRightLimit.x;
                if (hintPosition.y < bottomLeftLimit.y) hintPosition.y = bottomLeftLimit.y;
                else if (hintPosition.y > topRightLimit.y) hintPosition.y = topRightLimit.y;
                leftImageController.ShowHint(hintPosition);
            }
        }
    }
}
