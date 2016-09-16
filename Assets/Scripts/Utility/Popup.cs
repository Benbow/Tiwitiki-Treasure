using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class Popup : MonoBehaviour {

    [SerializeField]
    private bool _open;
    [SerializeField]
    private Vector3 _popupOpenPosition;
    [SerializeField]
    private Vector3 _popupClosePosition;

    void Start()
    {
        _popupClosePosition = transform.localPosition;
    }

    public void OnClickPopup()
    {
        if (_open)
            Close();
        else
            Open();
    }

    public void Open()
    {
        if(LocalManager.instance != null)
            LocalManager.instance.IsPopup = true;
        transform.DOLocalMoveY(_popupOpenPosition.y, 1f).SetEase(Ease.OutBack);
        _open = true;
    }
    public void Close()
    {
        if (LocalManager.instance != null)
            LocalManager.instance.IsPopup = false;
        transform.DOLocalMoveY(_popupClosePosition.y, 1f).SetEase(Ease.InBack);
        _open = false;
    }

}
