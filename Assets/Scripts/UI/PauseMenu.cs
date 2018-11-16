using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    /// <summary>
    /// controls the pause menu
    /// </summary>

    protected bool paused = false;

    [SerializeField] private RectTransform topSection;
    [SerializeField] private RectTransform bottomSection;

    private Vector2 topSectionHidden = new Vector2(-250f, 0);
    private Vector2 bottomSectionHidden = new Vector2(0, -800);

	// Use this for initialization
	void Start () {
        topSection.anchoredPosition = topSectionHidden;
        bottomSection.anchoredPosition = bottomSectionHidden;
	}
	
	// Update is called once per frame
	void Update () {

        //triggers for toggling pause
        bool pauseInput = Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape);
        if (pauseInput) { paused = !paused; }

        if (paused && topSection.anchoredPosition != Vector2.zero && bottomSection.anchoredPosition != Vector2.zero)
        {
            topSection.anchoredPosition = Vector2.Lerp(topSection.anchoredPosition, Vector2.zero, 0.3f);
            bottomSection.anchoredPosition = Vector2.Lerp(bottomSection.anchoredPosition, Vector2.zero, 0.2f);
        }
        else if (!paused && topSection.anchoredPosition != topSectionHidden && bottomSection.anchoredPosition != bottomSectionHidden)
        {
            topSection.anchoredPosition = Vector2.Lerp(topSection.anchoredPosition, topSectionHidden, 0.3f);
            bottomSection.anchoredPosition = Vector2.Lerp(bottomSection.anchoredPosition, bottomSectionHidden, 0.2f);
        }
    }

    public void TogglePause()
    {
        paused = !paused;
    }
}
