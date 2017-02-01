using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FileBrowserBackButton : MonoBehaviour {

    public FileBrowser fileBrowser;

    private bool m_GazeOver;
    private bool m_Enabled = true;

    [SerializeField] private Image buttonImage;

	public void GoBack()
    {
		fileBrowser.GoBack();
    }

    public void SetDisable ()
    {
        m_Enabled = false;
        buttonImage.color = new Color(1, 1, 1, .2f);
    }

    public void SetEnable ()
    {
        m_Enabled = true;
        buttonImage.color = new Color(1, 1, 1, .7f);
    }
}
