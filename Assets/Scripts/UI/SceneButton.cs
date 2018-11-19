using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using Event data.

[RequireComponent(typeof(Image))]
public class SceneButton : Selectable
{

    [SerializeField] private string sceneName;
    [SerializeField] private LoadSceneMode loadMode;

    protected Image img;

    [SerializeField] protected Sprite normal;
    [SerializeField] protected Sprite hover;

    // Use this for initialization
    protected override void Awake()
    {
        img = GetComponent<Image>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        img.sprite = hover;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        img.sprite = normal;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        if (sceneName == "Exit")
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(sceneName, loadMode);
        }
    }
}
