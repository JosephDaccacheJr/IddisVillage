using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BlackFade : MonoBehaviour
{
    public bool fadeOut;
    public float fadeSpeed = 1f;
    Image _fade;

    private void Awake()
    {
        _fade = GetComponent<Image>();
    }

    void Update()
    {
        if (fadeOut)
        {
            _fade.color = new Color(0f, 0f, 0f, Mathf.MoveTowards(_fade.color.a, 1f, Time.deltaTime * fadeSpeed));
        }
        else 
        {
            _fade.color = new Color(0f, 0f, 0f, Mathf.MoveTowards(_fade.color.a, 0f, Time.deltaTime * fadeSpeed));
        }
    }
}
