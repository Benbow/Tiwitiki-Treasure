using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class NewItemAppear : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GetComponent<Image>().DOFade(1, 0.5f).SetDelay(3f).OnComplete(UnlockGame);
        GameObject obj = Instantiate(LocalManager.instance.NewIllusParticles.gameObject);
        obj.transform.position = transform.position;
        ParticleSystem part = obj.GetComponent<ParticleSystem>();
        part.Play();
        Destroy(obj, part.startDelay + part.duration + 0.5f);
	}

    public void UnlockGame()
    {
        LocalManager.instance.IsNew = false;
    }
}
