using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloatingEffect : MonoBehaviour {

    [SerializeField]
    [Range(-1, 1)]
    private int _xDirection = 0;
    [SerializeField]
    [Range(-1, 1)]
    private int _yDirection = 0;

    [SerializeField]
    private float _delay = 0;
    [SerializeField]
    private float _speed = 0;

    [SerializeField]
    private float _delayFade = 0;

    [SerializeField]
    private bool _destroy = true;
    [SerializeField]
    private bool _fade = true;

    //Game Object to destroy
    [SerializeField]
    private GameObject _parent;


    private float _timer = 0;
    // Use this for initialization
    void Start ()
    {
        if (_parent == null)
            _parent = gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        _timer += Time.deltaTime;
        if ((_timer < _delay && _delay > 0) || _fade)
        {
            transform.Translate(new Vector3(_xDirection, _yDirection, 0) * _speed * Time.deltaTime);
        }
        else if (_delay > 0 && _destroy)
        {
            Destroy(_parent);
        }


        if (_timer < _delayFade && _delayFade > 0)
        {
            foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
            {
                if (sprite != null)
                    sprite.color = Fade(sprite.color);
            }
            foreach(Image sprite in GetComponentsInChildren<Image>())
            {
                if (sprite != null)
                    sprite.color = Fade(sprite.color);
            }
            foreach (TextMesh text in GetComponentsInChildren<TextMesh>())
            {
                if (text != null)
                    text.color = Fade(text.color);
            }
        }


    }

    Color Fade(Color color)
    {
        color = new Color(color.r, color.g, color.b, color.a - (Time.deltaTime / _delayFade));
        if (color.a <= 0)
            Destroy(_parent);

        return color;
    }
}
