using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResetSave : MonoBehaviour {


    public void ResetSaveApply()
    {
        GlobalManager.instance.PlayerConf.ResetSave();
        SceneManager.LoadScene(0);
    }
}

