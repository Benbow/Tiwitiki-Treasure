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
        instance = this;
        DontDestroyOnLoad(transform.gameObject);
    }


    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
