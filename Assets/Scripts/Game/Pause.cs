using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {
    [SerializeField]
    private GameObject PauseOverlay;
    [SerializeField]
    private Sprite _pause;
    [SerializeField]
    private Sprite _play;
    private SpriteRenderer _mySpriteRenderer;


    public void Start()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        PauseOverlay.SetActive(!GameManager.instance.IsPaused);
        _mySpriteRenderer.sprite = (GameManager.instance.IsPaused) ? _pause : _play;
        GameManager.instance.IsPaused = !GameManager.instance.IsPaused;
    }
}
