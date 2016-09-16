using UnityEngine;
using DG.Tweening;
using System.Collections;

public class TutoManager : MonoBehaviour {

    public GameObject Tuto1;
    public GameObject Tuto2;
    public GameObject Tuto3;

    public void LaunchTuto()
    {
        Tuto1.transform.DOLocalMoveY(0, 1f).SetEase(Ease.InOutElastic);
    }

    public void Phase2()
    {
        Tuto1.transform.DOLocalMoveY(-50, 1f).SetEase(Ease.InOutElastic);

        Tuto2.transform.DOLocalMoveY(0, 1f).SetEase(Ease.InOutElastic);
    }

    public void Phase3()
    {
        Tuto2.transform.DOLocalMoveY(-50, 1f).SetEase(Ease.InOutElastic);

        Tuto3.transform.DOLocalMoveY(0, 1f).SetEase(Ease.InOutElastic);
    }

    public void EndTuto()
    {
        Tuto3.transform.DOLocalMoveY(-50, 1f).SetEase(Ease.InOutElastic);
        GlobalManager.instance.PlayerConf.Tuto = false;
    }
}
