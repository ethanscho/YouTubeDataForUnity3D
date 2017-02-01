using UnityEngine;
using System.Collections;

public class MainCanvas : MonoBehaviour {

    CanvasGroup canvasGroup;

    private static MainCanvas instance;
    public static MainCanvas GetInstance()
    {
        if (!instance)
        {
            instance = (MainCanvas)GameObject.FindObjectOfType(typeof(MainCanvas));
            if (!instance)
                Debug.LogError("There needs to be one active MyClass script on a GameObject in your scene.");
        }

        return instance;
    }

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn ()
    {
        gameObject.SetActive(true);

        Hashtable ht = new Hashtable();
        ht.Add("from", canvasGroup.alpha);
        ht.Add("to", 1f);
        ht.Add("time", .3f);
        ht.Add("onupdate", "TweenOpacity");

        iTween.ValueTo(gameObject, ht);
    }

    public void FadeOut ()
    {
        Hashtable ht = new Hashtable();
        ht.Add("from", canvasGroup.alpha);
        ht.Add("to", 0f);
        ht.Add("time", .3f);
        ht.Add("onupdate", "TweenOpacity");
        ht.Add("oncomplete", "SetInactiveCanvas");

        iTween.ValueTo(gameObject, ht);
    }

    void TweenOpacity (float val)
    {
        canvasGroup.alpha = val;
    }

    void SetInactiveCanvas ()
    {
        gameObject.SetActive(false);
    }
}
