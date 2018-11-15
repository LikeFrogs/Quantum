using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using Event data.

[RequireComponent(typeof(Reset))]
public class ResetButton : SceneButton {

    private Reset reset;

    protected override void Awake()
    {
        base.Awake();
        reset = GetComponent<Reset>();
    }


    public override void OnSelect(BaseEventData eventData)
    {
        reset.ResetScene();
    }
}
