using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class DirectoryInformation : MonoBehaviour {

    public Text dirName;
    FileBrowser fileBrowser;

    private bool m_GazeOver;
    
    void Start ()
    {

    }

    public void InitItem (FileBrowser fb)
    {
        fileBrowser = fb;
    }
	
	public void SetName (string val)
    {
        dirName.text = val;
    }

	public void HandleDown () {
		fileBrowser.GetFileList(new DirectoryInfo(fileBrowser.GetCurrentDirectory() + "/" + dirName.text));
	}
}
