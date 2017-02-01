using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class FileBrowser : MonoBehaviour {

    DirectoryInfo currentDirectory;
    DirectoryInfo parentDirectory;

    public Transform scrollViewContent;
    public Transform hierarchyScrollViewContent;
    public GameObject directoryInformationPrefab;
    public GameObject fileInformationPrefab;
    public GameObject specialFolderInformationPrefab;
    public FileBrowserBackButton backButton;

    List<DirectoryInformation> directoryInformations = new List<DirectoryInformation>();
    List<FileInformation> fileInformations = new List<FileInformation>();

    // Use this for initialization
    void Start()
    {
        // Hierarchy view
        // 1. Local folders
        string[] drvs = System.IO.Directory.GetLogicalDrives();
        for (int i = 0; i < drvs.Length; i++)
        {
            GameObject specialFolderInformation = Instantiate(specialFolderInformationPrefab, hierarchyScrollViewContent) as GameObject;
            specialFolderInformation.transform.localPosition = Vector3.zero;

            SpecialFolderInformation specialFolderInfo = specialFolderInformation.GetComponent<SpecialFolderInformation>();
            specialFolderInfo.InitItem(this);
            specialFolderInfo.SetName(drvs[i]);
            specialFolderInfo.SetDir(drvs[i]);
        }

		// For Windows
		/*
        // 2. Special folders
        string specialFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        if (specialFolderPath != "")
        {
            GameObject specialFolderInformation = Instantiate(specialFolderInformationPrefab, hierarchyScrollViewContent) as GameObject;
            specialFolderInformation.transform.localPosition = Vector3.zero;

            SpecialFolderInformation specialFolderInfo = specialFolderInformation.GetComponent<SpecialFolderInformation>();
            specialFolderInfo.InitItem(this);
            specialFolderInfo.SetName("Desktop");
            specialFolderInfo.SetDir(specialFolderPath);
        }

        specialFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        if (specialFolderPath != "")
        {
            GameObject specialFolderInformation = Instantiate(specialFolderInformationPrefab, hierarchyScrollViewContent) as GameObject;
            specialFolderInformation.transform.localPosition = Vector3.zero;

            SpecialFolderInformation specialFolderInfo = specialFolderInformation.GetComponent<SpecialFolderInformation>();
            specialFolderInfo.InitItem(this);
            specialFolderInfo.SetName("Documents");
            specialFolderInfo.SetDir(specialFolderPath);
        }

        specialFolderPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
        if (specialFolderPath != "")
        {
            GameObject specialFolderInformation = Instantiate(specialFolderInformationPrefab, hierarchyScrollViewContent) as GameObject;
            specialFolderInformation.transform.localPosition = Vector3.zero;

            SpecialFolderInformation specialFolderInfo = specialFolderInformation.GetComponent<SpecialFolderInformation>();
            specialFolderInfo.InitItem(this);
            specialFolderInfo.SetName("Downloads");
            specialFolderInfo.SetDir(specialFolderPath);
        }

        // Populate download directory
        GetFileList(new DirectoryInfo(specialFolderPath));
		*/
    }

    public void GetFileList(DirectoryInfo di)
    {
        // Clear directories
        for(int i = directoryInformations.Count - 1; i >= 0; i--)
        {
            Destroy(directoryInformations[i].gameObject);
        }

        directoryInformations.Clear();

        for (int i = fileInformations.Count - 1; i >= 0; i--)
        {
            Destroy(fileInformations[i].gameObject);
        }

        fileInformations.Clear();

        // Set directories
        currentDirectory = di;
        parentDirectory = currentDirectory.Parent;

        if (parentDirectory == null)
            backButton.SetDisable();
        else
            backButton.SetEnable();

        // Create directories
        DirectoryInfo[] dia = di.GetDirectories();
        for (int i = 0; i < dia.Length; i++)
        {
            if ((dia[i].Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                continue;

            GameObject directoryInformation = Instantiate(directoryInformationPrefab, scrollViewContent) as GameObject;
            directoryInformation.transform.localPosition = Vector3.zero;

            DirectoryInformation dirInfo = directoryInformation.GetComponent<DirectoryInformation>();
            dirInfo.InitItem(this);
            dirInfo.SetName(dia[i].Name);

            directoryInformations.Add(dirInfo);
        }

        // Create files
        FileInfo[] fia = di.GetFiles("*");
        for (int i = 0; i < fia.Length; i++)
        {
            if (CheckFileExtension(fia[i].Name))
            {
                GameObject fileInformation = Instantiate(fileInformationPrefab, scrollViewContent) as GameObject;
                fileInformation.transform.localPosition = Vector3.zero;

                FileInformation fileInfo = fileInformation.GetComponent<FileInformation>();
                fileInfo.InitItem(this);
                fileInfo.SetName(fia[i].Name);

                fileInformations.Add(fileInfo);
            }
        }
    }

    public DirectoryInfo GetCurrentDirectory ()
    {
        return currentDirectory;
    }

    public void GoBack ()
    {
        GetFileList(parentDirectory);
    }

    bool CheckFileExtension (string fileName)
    {
        if (fileName.Contains(".3gp") ||
            fileName.Contains(".avi") ||
            fileName.Contains(".flv") ||
            fileName.Contains(".swf") ||
            fileName.Contains(".m4v") ||
            fileName.Contains(".mkv") ||
            fileName.Contains(".ogg") ||
            fileName.Contains(".mov") ||
            fileName.Contains(".qt") ||
            fileName.Contains(".webm") ||
            fileName.Contains(".wmv"))
            return true;
        else
            return false;
    }
}
