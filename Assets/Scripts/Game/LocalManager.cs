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

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        DOTween.Init();
        ScrollSnapReference.startingPage = GlobalManager.instance.PlayerConf.ActualWorld - 1;
        WorldNameReference.text = GlobalManager.instance.PlayerConf.GetActualWorldData().WorldName;

        Charged();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Charged()
    {
        Ui.SetActive(true);
        FrontScreen.DOFade(0, 2f).SetDelay(0.8f);
    }

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
    }

    public void Play()
    {
        WorldGameData data = GlobalManager.instance.PlayerConf.GetActualWorldData();
        data.AttemptsActual = data.AttemptsMaxAdder + data.MapConfig.maxAttemptsBase;
        SceneManager.LoadScene(1);
    }
}
