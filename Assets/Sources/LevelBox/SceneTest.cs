using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTest : MonoBehaviour
{
    int sceneNum = 1;
    public int cutsceneNum = 1;
    public GameObject scene01;
    public GameObject scene02;
    public GameObject scene03;
    public GameObject scene04;

    public GameObject hCutscene01;
    public GameObject hCutscene02;
    public GameObject hCutscene03;
    public GameObject sCutscene01;
    public GameObject sCutscene02;
    public GameObject sCutscene03;
    public void OnChangeScene()
    {
        if (sceneNum < 4)
        {
            sceneNum++;
        }
        else
        {
            sceneNum = 1;
        }

    }

    public void OnChangecutscene()
    {
        if (cutsceneNum < 3)
        {
            cutsceneNum++;
        }
        else
        {
            cutsceneNum = 1;
        }
    }

    public void OnRestart()
    {
        SceneManager.LoadScene("Level1WhiteBox");
    }

    void Update()
    {
        switch(sceneNum)
        {
            case 1:
                scene01.SetActive(true);
                scene02.SetActive(false);
                scene03.SetActive(false);
                scene04.SetActive(false);
                break;
            case 2:
                scene01.SetActive(false);
                scene02.SetActive(true);
                scene03.SetActive(false);
                scene04.SetActive(false);
                break;
            case 3:
                scene01.SetActive(false);
                scene02.SetActive(false);
                scene03.SetActive(true);
                scene04.SetActive(false);
                break;
            case 4:
                scene01.SetActive(false);
                scene02.SetActive(false);
                scene03.SetActive(false);
                scene04.SetActive(true);
                break;
        }

        switch(cutsceneNum)
        {
            case 1:
                hCutscene01.SetActive(true);
                sCutscene01.SetActive(true);
                hCutscene02.SetActive(false);
                sCutscene02.SetActive(false);
                hCutscene03.SetActive(false);
                sCutscene03.SetActive(false);
                break;
            case 2:
                hCutscene01.SetActive(false);
                sCutscene01.SetActive(false);
                hCutscene02.SetActive(true);
                sCutscene02.SetActive(true);
                hCutscene03.SetActive(false);
                sCutscene03.SetActive(false);
                break;
            case 3:
                hCutscene01.SetActive(false);
                sCutscene01.SetActive(false);
                hCutscene02.SetActive(false);
                sCutscene02.SetActive(false);
                hCutscene03.SetActive(true);
                sCutscene03.SetActive(true);
                break;
        }

    }
}
