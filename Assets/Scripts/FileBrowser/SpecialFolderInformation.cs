using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class SpecialFolderInformation : MonoBehaviour
{
    public Text dirName;
    string dir;
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
        dirName.text = val;
    }

    public void SetDir(string val)
    {
        dir = val;
    }

    private void HandleDown()
    {
        if (m_GazeOver)
        {
            fileBrowser.GetFileList(new DirectoryInfo(dir));
        }
    }

    private void HandleUp()
    {

    }
}
