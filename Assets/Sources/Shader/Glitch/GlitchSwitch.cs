using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//[ExecuteInEditMode]
public class GlitchSwitch : MonoBehaviour
{
    public GameObject SwitchobejectA;
    public GameObject SwitchobejectB;
    [Range(5, 20)] public float Intensity; 
    private float IntensityA;
    private float IntensityB;   
    public bool Switch = false;
    [Range(0.1f, 3f)] public float TimeGap; 
    private float timer = 0;
    new public Transform camera;//Ö÷Ïà»ú
    //public Transform ObjPos;
    // Start is called before the first frame update
    void Start()
    {
        IntensityA = 0;
        IntensityB = 0;        
    }

    // Update is called once per frame
    void Update()
    {

        //if(Input.GetMouseButtonDown(0))
        //{
        //    Switch = true;
        //}
        if(Switch)
        {
            timer += Time.deltaTime;
            for (int i = 0; i < SwitchobejectA.GetComponentsInChildren<MeshRenderer>().Length; i++)
            {
                for (int j = 0; j < SwitchobejectA.GetComponentsInChildren<MeshRenderer>()[i].materials.Length; j++)
                {                                         
                    SwitchobejectA.GetComponentsInChildren<MeshRenderer>()[i].materials[j].SetFloat("_ZTestDepthEqualForOpaque", 0);
                    SwitchobejectB.GetComponentsInChildren<MeshRenderer>()[i].materials[j].SetFloat("_ZTestDepthEqualForOpaque", 0);
                }
            }
            StartSwitch();
        }
        for (int i = 0; i < SwitchobejectA.GetComponentsInChildren<MeshRenderer>().Length; i++)
        {
            for (int j = 0; j < SwitchobejectA.GetComponentsInChildren<MeshRenderer>()[i].materials.Length; j++)
            {
                SwitchobejectA.GetComponentsInChildren<MeshRenderer>()[i].materials[j].SetFloat("_Intensity", IntensityA);
                SwitchobejectA.GetComponentsInChildren<MeshRenderer>()[i].materials[j].SetVector("_CameraDir", camera.forward);                                               
            }
        }
        for (int i = 0; i < SwitchobejectB.GetComponentsInChildren<MeshRenderer>().Length; i++)
        {
            for (int j = 0; j < SwitchobejectB.GetComponentsInChildren<MeshRenderer>()[i].materials.Length; j++)
            {
                SwitchobejectB.GetComponentsInChildren<MeshRenderer>()[i].materials[j].SetFloat("_Intensity", IntensityB);
                SwitchobejectB.GetComponentsInChildren<MeshRenderer>()[i].materials[j].SetVector("_CameraDir", camera.forward);
            }
        }
    }

    private void StartSwitch()
    {
        
        if (timer/TimeGap < 0.5f)
        {
            IntensityA = Mathf.Clamp(timer/TimeGap * 2 * Intensity, 0, Intensity);             
            for (int i = 0; i < SwitchobejectA.GetComponentsInChildren<MeshRenderer>().Length; i++)
            {
                for (int j = 0; j < SwitchobejectA.GetComponentsInChildren<MeshRenderer>()[i].materials.Length; j++)
                {                    
                    SwitchobejectA.GetComponentsInChildren<MeshRenderer>()[i].materials[j].SetFloat("_ZTestDepthEqualForOpaque", 0);                    
                }
            }
            for (int i = 0; i < SwitchobejectB.GetComponentsInChildren<MeshRenderer>().Length; i++)
            {
                for (int j = 0; j < SwitchobejectB.GetComponentsInChildren<MeshRenderer>()[i].materials.Length; j++)
                {                    
                    SwitchobejectB.GetComponentsInChildren<MeshRenderer>()[i].materials[j].SetFloat("_ZTestDepthEqualForOpaque", 0);
                }
            }
        }
        else if (timer/TimeGap > 0.5f && timer/TimeGap < 1f)
        {
            SwitchobejectA.SetActive(false);
            SwitchobejectB.SetActive(true);
            IntensityB = Mathf.Clamp((1 - timer/TimeGap) * 2 * Intensity, 0, Intensity);            
        }
        else if (timer/TimeGap > 1)
        {
            Switch = false;
            IntensityA = 0;
            IntensityB = 0;
            timer = 0;
            for (int i = 0; i < SwitchobejectA.GetComponentsInChildren<MeshRenderer>().Length; i++)
            {
                for (int j = 0; j < SwitchobejectA.GetComponentsInChildren<MeshRenderer>()[i].materials.Length; j++)
                {                    
                    SwitchobejectA.GetComponentsInChildren<MeshRenderer>()[i].materials[j].SetFloat("_ZTestDepthEqualForOpaque", 4);                    
                }
            }
            for (int i = 0; i < SwitchobejectB.GetComponentsInChildren<MeshRenderer>().Length; i++)
            {
                for (int j = 0; j < SwitchobejectB.GetComponentsInChildren<MeshRenderer>()[i].materials.Length; j++)
                {
                    SwitchobejectB.GetComponentsInChildren<MeshRenderer>()[i].materials[j].SetFloat("_ZTestDepthEqualForOpaque", 4);
                }
            }
        }
    }
}
