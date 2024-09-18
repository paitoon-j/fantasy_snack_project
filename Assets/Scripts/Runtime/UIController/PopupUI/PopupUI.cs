using System.Collections.Generic;
using UnityEngine;

public class PopupUI : MonoBehaviour
{
    [SerializeField] private PopupPause _popupPause;
    [SerializeField] private PopupGameOver _popupGameOver;

    private List<string> _list = new List<string>();

    public bool IsShowPopup
    {
        get { return _list.Count > 0; }
    }

    ////////////////////////////////////////////////

    public void ShowPausePopup()
    {
        setIDCallback(_popupPause);
        _popupPause.Show();
    }

    public void ShowGameOverPopup()
    {
        setIDCallback(_popupGameOver);
        _popupGameOver.Show();
    }


    ////////////////////////////////////////////////

    private void setIDCallback(PopupModule popup)
    {
        popup.OnOpen(() => {
            AddPopup(popup.gameObject.name);
        });

        popup.OnClose(() => {
            RemovePopup(popup.gameObject.name);
        });
    }

    private void AddPopup(string popupName)
    {
        if (_list.Contains(popupName) == false)
        {
            _list.Add(popupName);
        }
        else
        {
            Debug.LogError("Cannot open the same popup : " + popupName);
        }
    }

    private void RemovePopup(string popupName)
    {
        if (_list.Contains(popupName))
        {
            int index = _list.FindIndex(x => x == popupName);
            _list.RemoveRange(index, 1);
        }
        else
        {
            Debug.LogError("Cannot find popup : " + popupName);
        }
    }
}
