using UnityEngine;
using System.Collections;

public class GlobalManager : MonoBehaviour {

    public PlayerConfig PlayerConf;

    public static GlobalManager instance;

    private GlobalManager() { }

    public static GlobalManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GlobalManager();
            }
            return instance;
        }
    }

    void Awake()
    {
        // Set framerate

        QualitySettings.vSyncCount = 0;
        TargetFrameRate(30); // NOTES : Keep this number at 30 max to avoid the phone over heating during game sessions

        instance = this;
        DontDestroyOnLoad(transform.gameObject);
    }


    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TargetFrameRate(int customFps)
    {
        // Make the game run at the specified fps
        if (Application.targetFrameRate != customFps)
            Application.targetFrameRate = customFps;
    }

}
