using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackFade : MonoBehaviour
{
    Image blackScreen;
    public bool fadingIn;
    public bool fadingOut;

    void Awake()
    {
        blackScreen = GetComponent<Image>();
        Color temp = Color.black;
        temp.a = 0;
        blackScreen.color = temp;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator blackFadeIn()
    {
        fadingIn = true;
        Color temp = Color.black;
        temp.a = 0;

        while (blackScreen.color.a < 1)
        {
            temp.a = blackScreen.color.a + .1f;
            blackScreen.color = temp;
            yield return new WaitForSeconds(.1f);
        }

        fadingIn = false;
    }

    public IEnumerator blackFadeOut()
    {
        fadingOut = true;
        Color temp = Color.black;
        temp.a = 1;

        while (blackScreen.color.a > 0)
        {
            temp.a = blackScreen.color.a - .1f;
            blackScreen.color = temp;
            yield return new WaitForSeconds(.1f);
        }

        fadingOut = false;
    }
}
