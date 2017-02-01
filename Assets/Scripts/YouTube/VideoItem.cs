using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class VideoItem : MonoBehaviour {

    [SerializeField] private Image thumbnail;
    [SerializeField] private Text title;
    [SerializeField] private Text date;
    private string url;

    private bool m_GazeOver;

    Color thumbnailColor = new Color(1, 1, 1, 0);

    // Use this for initialization
    void Start () {
        thumbnail.color = thumbnailColor;
    }

    public void SetThumbnail(string val)
    {
        StartCoroutine(RenderThumbnail(val));
    }

    public void SetTitle(string val)
    {
        title.text = val;
    }

    public void SetDate (string val)
    {
        char[] delimiterChars = { '-', ':', 'T', '.' };

        string[] words = val.Split(delimiterChars);
        DateTime videoDate = new DateTime(int.Parse(words[0]), int.Parse(words[1]), int.Parse(words[2]), int.Parse(words[3]), int.Parse(words[4]), int.Parse(words[5]));
        DateTime currentDate = DateTime.Now;

        TimeSpan diff = currentDate.Subtract(videoDate);

        if (diff.Days > 0)
        {
            if (diff.Days < 30)
                date.text = diff.Days + " days ago";
            else if (diff.Days < 365)
                date.text = diff.Days / 30 + " months ago";
            else
                date.text = diff.Days / 365 + " years ago";
        }
        else if (diff.Hours > 0)
        {
            date.text = diff.Hours + " hours ago";
        }
        else if (diff.Minutes > 0)
        {
            date.text = diff.Minutes + " minutes ago";
        }
        else if (diff.Seconds > 0)
        {
            date.text = diff.Minutes + " seconds ago";
        }
    }

    public void SetURL (string val)
    {
        url = "https://www.youtube.com/watch?v=" + val + "&hd=1";
    }

    IEnumerator RenderThumbnail(string url)
    {
        WWW www = new WWW(url);

        // Wait for download to complete
        yield return www;

        thumbnail.overrideSprite = Sprite.Create(www.texture, new Rect(0, 0, 480, 360), new Vector2(0.5f, 0.5f));

        Hashtable ht = new Hashtable();
        ht.Add("from", 0f);
        ht.Add("to", 1f);
        ht.Add("time", .3f);
        ht.Add("onupdate", "TweenOpacity");

        iTween.ValueTo(gameObject, ht);
    }

    void TweenOpacity (float val)
    {
        thumbnailColor.a = val;
        thumbnail.color = thumbnailColor;
    }
}
