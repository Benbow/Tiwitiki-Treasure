using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;

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

    public void Start()
    {
        SpriteBack = GetComponent<SpriteRenderer>().sprite;
        //MyButton.onClick += ButtonClick;
    }

    public void OnMouseDown()
    {
        Debug.Log("Sprite Mouse Down");

        //first, choose the sprite
        List<CollectionItemData> actualCollection = GameManager.instance.MyPlayerConfig.GetActualWorldData().Collections;
        _selectedItem = actualCollection[Random.Range(0, actualCollection.Count)];
        Illus.sprite = _selectedItem.CollectionSprite;

        GetComponent<SpriteRenderer>().sprite = SpriteFront;
        Particles.Play();
        Illus.DOFade(1, 1.5f).OnComplete(ItemAppear);
    }

    public void ItemAppear()
    {
        if (_selectedItem.Owned)
        {
            TitleText.text = "Duplicata : " + _selectedItem.Name;
            ButtonText.text = "Continue";
            Debug.Log("Well, you already have it !");
        }
        else
        {
            _selectedItem.Owned = true;
            TitleText.text = "NEW Card : " + _selectedItem.Name;
            Debug.Log("HURRAY ! A NEW ONE !");
            ButtonText.text = "Add (for now, continue)";
        }

        MyButton.image.DOFade(1f, 0.6f);
        ButtonText.DOFade(1f, 0.6f);
        TitleText.DOFade(1f, 0.6f);

    }

    public void ButtonClick()
    {
        MyButton.image.DOFade(0f, 0.6f);
        ButtonText.DOFade(0f, 0.6f);
        TitleText.DOFade(0f, 0.6f).OnComplete(GameManager.instance.ContinueAftertPopupAnim);
    }


}
