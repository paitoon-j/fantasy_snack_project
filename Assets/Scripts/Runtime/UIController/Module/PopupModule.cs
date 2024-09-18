using System;
using UnityEngine;

public class PopupModule : MonoBehaviour
{
    private Action _onOpen;
    private Action _onClose;

    public void Open()
    {
        gameObject.SetActive(true);
        _onOpen?.Invoke();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        _onClose?.Invoke();
    }

    public void OnOpen(Action callback)
    {
        _onOpen = callback;
    }

    public void OnClose(Action callback)
    {
        _onClose = callback;
    }
}
