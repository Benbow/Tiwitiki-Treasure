using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LocalManager : MonoBehaviour {

    public Text WorldNameReference;
    public ScrollSnapRect ScrollSnapReference;
    public GameObject Ui;
    public SpriteRenderer FrontScreen;
    public GameObject PlayButton;
    public Image WorldCompletionBar;
    public Text WorldCompletionText;
    public TutoManager TutoObject;

    public ParticleSystem NewIllusParticles;

    [SerializeField]
    private bool _isPopup;
    [SerializeField]
    private bool _isNew;

    public static LocalManager instance;


    private LocalManager() { }

    public static LocalManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LocalManager();
            }
            return instance;
        }
    }

    public bool IsPopup
    {
        get
        {
            return _isPopup;
        }

        set
        {
            _isPopup = value;
        }
    }

    public bool IsNew
    {
        get
        {
            return _isNew;
        }

        set
        {
            _isNew = value;
        }
    }

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        DOTween.Init();
        ScrollSnapReference.startingPage = GlobalManager.instance.PlayerConf.ActualWorld - 1;
        WorldNameReference.text = GlobalManager.instance.PlayerConf.GetActualWorldData().WorldName;
        UpdateWorldCompletion();
        Appear();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Appear()
    {
        Ui.SetActive(true);
        FrontScreen.DOFade(0, 2f).SetEase(Ease.InQuint).OnComplete(CheckStart);

    }

    public void Disappear()
    {
        if(!GlobalManager.instance.PlayerConf.Tuto && !IsPopup && !IsNew)
            FrontScreen.DOFade(1, 2f).SetEase(Ease.OutQuint).OnComplete(Play);
    }

    public void CheckStart()
    {
        CheckTuto();
        //CheckNew();
    }

    public void CheckTuto()
    {
        if (GlobalManager.instance.PlayerConf.Tuto)
        {
            TutoObject.LaunchTuto();
        }
    }

    /*public void CheckNew()
    {
        WorldGameData data = GlobalManager.instance.PlayerConf.GetActualWorldData();
        for (int i = 0; i > -data.LayersCount; i--)
        {
            foreach (CollectionItemData item in data.Collections)
            {
                if (item.IsNew && item.Owned)
                {
                    foreach (ItemPlacementData placement in item.PlacementData)
                    {
                        if (i == placement.Position.z)
                        {
                            GameObject illus = Instantiate(NewIllusPrefab);
                            illus.transform.SetParent(transform);
                            Image illusImg = illus.GetComponent<Image>();
                            illusImg.sprite = item.CollectionSprite;
                            illusImg.preserveAspect = true;
                            illusImg.SetNativeSize();

                            RectTransform illusTransform = illus.GetComponent<RectTransform>();
                            illusTransform.localPosition = placement.Position;
                            illusTransform.sizeDelta = placement.Size;
                            illusTransform.localScale = placement.Scaling;
                            illus.name = item.Name;

                            illus.AddComponent<NewItemAppear>();
                        }
                    }
                }
            }
        }
    }*/

    public void ChangeWorld(int worldId)
    {
        GlobalManager.instance.PlayerConf.ActualWorld = worldId;
        if (GlobalManager.instance.PlayerConf.GetActualWorldData() != null)
        {
            WorldGameData data = GlobalManager.instance.PlayerConf.GetActualWorldData();
            WorldNameReference.text = data.WorldName;
            if (data.IsUnlocked)
            {
                PlayButton.GetComponent<Image>().DOFade(1f, 0.5f);
                PlayButton.GetComponent<Button>().enabled = true;
                PlayButton.GetComponentInChildren<Text>().text = data.AttemptsMaxAdder + data.MapConfig.maxAttemptsBase + " Attempts";
            }
            else
            {
                PlayButton.GetComponent<Image>().DOFade(0.5f, 0.5f);
                PlayButton.GetComponent<Button>().enabled = false;
                PlayButton.GetComponentInChildren<Text>().text = " ";
            }
        }
        else
        {
            WorldNameReference.text = "Unknown";
            PlayButton.GetComponent<Image>().DOFade(0.5f, 0.5f);
            PlayButton.GetComponent<Button>().enabled = false;
            PlayButton.GetComponentInChildren<Text>().text = " ";
        }
        UpdateWorldCompletion();
    }

    public void Play()
    {
        WorldGameData data = GlobalManager.instance.PlayerConf.GetActualWorldData();
        data.AttemptsActual = data.AttemptsMaxAdder + data.MapConfig.maxAttemptsBase;
        SceneManager.LoadScene(1);
    }

    public void UpdateWorldCompletion()
    {
       
        if (GlobalManager.instance.PlayerConf.GetActualWorldData() != null)
        {
            float value = GlobalManager.instance.PlayerConf.GetActualWorldData().GetPercentageCollection();
            //if(value != )
            WorldCompletionBar.DOFillAmount(GlobalManager.instance.PlayerConf.GetActualWorldData().GetPercentageCollection(), 0.5f);
            WorldCompletionText.text = Mathf.Floor(GlobalManager.instance.PlayerConf.GetActualWorldData().GetPercentageCollection() * 100f) + "% Completed";
        }
        else
        {
            WorldCompletionBar.DOFillAmount(0, 0.5f);
            WorldCompletionText.text = 0 + "% Completed";
        }
    }
}
