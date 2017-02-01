using UnityEngine;
using System.Collections;

public class FileBrowserCanvas : MonoBehaviour {

    [SerializeField] private CanvasGroup canvasGroup;

    // Use this for initialization
    void Start () {
        
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);

        Hashtable ht = new Hashtable();
        ht.Add("from", canvasGroup.alpha);
        ht.Add("to", 1f);
        ht.Add("time", .3f);
        ht.Add("onupdate", "TweenOpacity");

        iTween.ValueTo(gameObject, ht);
    }

    public void FadeOut()
    {
        Hashtable ht = new Hashtable();
        ht.Add("from", canvasGroup.alpha);
        ht.Add("to", 0f);
        ht.Add("time", .3f);
        ht.Add("onupdate", "TweenOpacity");
        ht.Add("oncomplete", "SetInactiveCanvas");

        iTween.ValueTo(gameObject, ht);
    }

    void TweenOpacity(float val)
    {
        canvasGroup.alpha = val;
    }

    void SetInactiveCanvas()
    {
        gameObject.SetActive(false);
    }
}
