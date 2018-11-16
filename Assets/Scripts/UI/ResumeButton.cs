using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using Event data.

public class ResumeButton : SceneButton {

    private PauseMenu pauseMenu;

    protected override void Awake()
    {
        base.Awake();
        pauseMenu = GetComponentInParent<PauseMenu>();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        pauseMenu.TogglePause();
    }
}
