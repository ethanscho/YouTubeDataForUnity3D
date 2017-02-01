using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class FileInformation : MonoBehaviour {

    public Text fileName;
    FileBrowser fileBrowser;

    private bool m_GazeOver;

    void Start()
    {

    }

    public void InitItem(FileBrowser fb)
    {
        fileBrowser = fb;
    }

    public void SetName(string val)
    {
        fileName.text = val;
    }

	public void PrintFilePath () {
		Debug.Log (fileBrowser.GetCurrentDirectory () + "/" + fileName.text);
	}
}
