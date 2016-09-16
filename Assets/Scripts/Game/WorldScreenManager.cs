using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WorldScreenManager : MonoBehaviour {

    [SerializeField]
    private int _worldId;

    private WorldGameData _myWorldGameData;

    public WorldGameData MyWorldGameData
    {
        get
        {
            return _myWorldGameData;
        }

        set
        {
            _myWorldGameData = value;
        }
    }

    public int WorldId
    {
        get
        {
            return _worldId;
        }

        set
        {
            _worldId = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        WorldGameData data = GlobalManager.instance.PlayerConf.GetWorldDataById(_worldId);
        if (data.IsUnlocked)
        {
            for (int i = 0; i > -data.LayersCount; i--)
            {
                foreach (CollectionItemData item in data.Collections)
                {
                    if (item.Owned)
                    {
                        foreach (ItemPlacementData placement in item.PlacementData)
                        {
                            if (i == placement.Position.z)
                            {
                                GameObject illus = new GameObject();
                                illus.transform.SetParent(transform);
                                Image illusImg = illus.AddComponent<Image>();
                                illusImg.sprite = item.CollectionSprite;
                                illusImg.preserveAspect = true;
                                illusImg.SetNativeSize();

                                RectTransform illusTransform = illus.GetComponent<RectTransform>();
                                illusTransform.localPosition = placement.Position;
                                illusTransform.sizeDelta = placement.Size;
                                illusTransform.localScale = placement.Scaling;
                                illus.name = item.Name;

                                if (item.IsNew)
                                {
                                    illusImg.color = new Color(1, 1, 1, 0);
                                    illus.AddComponent<NewItemAppear>();
                                }
                            }
                        }
                    }
                    
                }
            }
            //uncheck isNew
            foreach (CollectionItemData item in data.Collections)
            {
                if (item.IsNew && item.Owned)
                {
                    item.IsNew = false;
                    LocalManager.instance.IsNew = true;
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
