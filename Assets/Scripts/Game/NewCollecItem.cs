using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewCollecItem : MonoBehaviour
{
    public Sprite SpriteBack;
    public Sprite SpriteFront;
    public Text TitleText;
    public Text ButtonText;
    public Button MyButton;
    public SpriteRenderer Illus;
    public ParticleSystem Particles;
    private CollectionItemData _selectedItem;

    private bool _isCliked;

    public void Start()
    {
        SpriteBack = GetComponent<SpriteRenderer>().sprite;
    }

    public void OnMouseDown()
    {
        if (!_isCliked)
        {
            _isCliked = true;
            //first, choose the sprite
            List<CollectionItemData> actualCollection = GameManager.instance.MyPlayerConfig.GetActualWorldData().Collections;
            _selectedItem = actualCollection[Random.Range(0, actualCollection.Count)];
            Illus.sprite = _selectedItem.CollectionSprite;

            GetComponent<SpriteRenderer>().sprite = SpriteFront;
            Particles.Play();
            Illus.DOFade(1, 1.5f).OnComplete(ItemAppear);
        }
    }

    public void ItemAppear()
    {
        if (_selectedItem.Owned)
        {
            TitleText.text = "Duplicata : " + _selectedItem.Name;
            ButtonText.text = "Continue";
            WorldGameData data = GameManager.instance.MyPlayerConfig.GetActualWorldData();
            data.AttemptsActual = data.AttemptsMaxAdder + data.MapConfig.maxAttemptsBase;
        }
        else
        {
            TitleText.text = "NEW Card : " + _selectedItem.Name;
            ButtonText.text = "Add";

            WorldGameData data = GameManager.instance.MyPlayerConfig.GetActualWorldData();
            data.AttemptsMaxAdder += 1;
        }

        MyButton.image.DOFade(1f, 0.6f);
        ButtonText.DOFade(1f, 0.6f);
        TitleText.DOFade(1f, 0.6f);
    }

    public void ButtonClick()
    {
        if (!_selectedItem.Owned)
        {
            _selectedItem.Owned = true;
            GameManager.instance.AfterPopupReady(false);
            SceneManager.LoadScene(0);
        }

        MyButton.image.DOFade(0f, 0.6f);
        ButtonText.DOFade(0f, 0.6f);
        TitleText.DOFade(0f, 0.6f).OnComplete(Activate);
    }

    public void Activate()
    {
        _isCliked = false;
        GameManager.instance.ContinueAftertPopupAnim();
    }


}
