using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class YouTubeSearch : MonoBehaviour
{
    const string baseURL = "https://www.googleapis.com/youtube/v3";
    const string apiKey = "AIzaSyA6PUfGOuecty0nxJNBC3xGDg-8EUM6waE";
    private string pageToken = "";
    private string keywords = "360+VR";

    List<VideoItem> videoItems = new List<VideoItem>();
    public GameObject videoItemPrefab;
    public GameObject videoItemCanvas;
    RectTransform scrollViewContent;
    CanvasGroup canvasGroup;
    public CanvasGroup titleBarCanvasGroup;

    private int rowIndex = 0;
    private int videoIndex = 0;

    [SerializeField] private InputField inputField;

    private bool m_GazeOver;
    
    void Start()
    {
        scrollViewContent = videoItemCanvas.GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(QuerySearchList(keywords));
    }

    IEnumerator QuerySearchList (string keywords)
    {
        // Create 12 video items
        int maxRow = rowIndex + 4;

        for (; rowIndex < maxRow; rowIndex++)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject videoItem = Instantiate(videoItemPrefab, videoItemCanvas.transform) as GameObject;
                videoItem.transform.localPosition = Vector3.zero;
                videoItem.transform.localEulerAngles = Vector3.zero;

                VideoItem videoItemScript = videoItem.GetComponent<VideoItem>();
                videoItems.Add(videoItemScript);
            }
        }

        // Pull down the JSON from YouTube
        string query = "";

        if (pageToken == "")
        {
            query = baseURL + "/search?part=snippet&maxResults=12&order=relevance&q=" + keywords + "&type=video&videoDefinition=high&key=" + apiKey;
        }
        else
        {
            query = baseURL + "/search?pageToken=" + pageToken + "&part=snippet&maxResults=12&order=relevance&q=" + keywords + "&type=video&videoDefinition=high&key=" + apiKey;
        }

        WWW w = new WWW(query);
        yield return w;

        ExtractSearchList(w.text);
    }

    void ExtractSearchList (string json)
    {
        print(json);

        // Create a JSON object from the text stream
        JSONObject jo = new JSONObject(json);

        pageToken = jo.list[2].str;

        // Go through the list of objects in our array
        foreach (JSONObject item in jo.list)
        {
            if (item.type == JSONObject.Type.ARRAY)
            {
                for (int i = 0; i < item.list.Count; i++)
                {
                    JSONObject videoInfo = item.list[i];

                    // Thumbnail
                    videoItems[videoIndex + i].SetThumbnail(videoInfo.list[3].list[4].list[2].list[0].str);

                    // Title
                    videoItems[videoIndex + i].SetTitle(videoInfo.list[3].list[2].str);

                    // Date
                    videoItems[videoIndex + i].SetDate(videoInfo.list[3].list[0].str);

                    // URL
                    videoItems[videoIndex + i].SetURL(videoInfo.list[2].list[1].str);
                }
            }
        }

        videoIndex += 12;
    }

    public void OnKeywordsEntered ()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {

            keywords = inputField.text;
            keywords = keywords.Replace(" ", "+");

            videoIndex = 0;
            rowIndex = 0;
            pageToken = "";

            for (int i = videoItems.Count - 1; i >= 0; i--)
            {
                Destroy(videoItems[i].gameObject);
            }

            videoItems.Clear();

            HideSearchBar();

            StartCoroutine(QuerySearchList(keywords));
        }
        
    }

    public void ReachedScrollbarEndpoint ()
    {
        StartCoroutine(QuerySearchList(keywords));
    }

    public void ShowSearchBar ()
    {
        Hashtable ht = new Hashtable();
        ht.Add("from", canvasGroup.alpha);
        ht.Add("to", 1f);
        ht.Add("time", .3f);
        ht.Add("onupdate", "TweenOpacity");

        iTween.ValueTo(gameObject, ht);

        inputField.Select();
        inputField.ActivateInputField();
    }

    public void HideSearchBar ()
    {
        Hashtable ht = new Hashtable();
        ht.Add("from", canvasGroup.alpha);
        ht.Add("to", 0f);
        ht.Add("time", .3f);
        ht.Add("onupdate", "TweenOpacity");

        iTween.ValueTo(gameObject, ht);
    }

    void TweenOpacity (float val)
    {
        canvasGroup.alpha = val;
        titleBarCanvasGroup.alpha = 1f - val;
    }
}