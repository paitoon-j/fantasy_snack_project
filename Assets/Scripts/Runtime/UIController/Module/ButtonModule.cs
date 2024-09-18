using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonModule : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void On(Action onClick)
    {
        _button.onClick.AddListener(() =>
        {
            onClick?.Invoke();
        });
    }

    public void Off()
    {
        _button.onClick.RemoveAllListeners();
    }

    public void SetInteractive(bool interactable)
    {
        _button.interactable = interactable;
    }

    public void SetActive(bool isActive)
    {
        _button.gameObject.SetActive(isActive);
    }
}
