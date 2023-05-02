using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class LensRotateAnimation : MonoBehaviour
{
    [Header("Static")]
    private float timer;
    private float[] EndTimeArray;

    [Header("Reference")]
    public Volume volume;
    public Material LensRotateMat;
    private ChromaticAberration _CA;
    private DepthOfField _DOF;
    private ColorAdjustments _CAS;

    [Space(20f)]
    [Header("Switch")]
    public bool Begin = false;    

    [Space(40f)]
    [Header("LensRotate")]
    public float BeginTime_LR;
    public float EndTime_LR;
    public AnimationCurve AC_LR;
    public float BeginValue_LR;
    public float EndValue_LR;

    [Space(30f)]
    [Header("ColorAdjustments")]
    public float BeginTime_PostExposure_CAS;
    public float EndTime_PostExposure_CAS;
    public AnimationCurve AC_PostExposure_CAS;
    public float BeginValue_PostExposure_CAS;
    public float EndValue_PostExposure_CAS;

    [Space(20f)]
    public float BeginTime_ColorFilter_CAS;
    public float EndTime_ColorFilter_CAS;
    public AnimationCurve AC_ColorFilter_CAS;
    public Color BeginColor_ColorFilter_CAS;
    public Color EndColor_ColorFilter_CAS;
    

    [Space(30f)]
    [Header("DepthOfField")]
    public float BeginTime_DOF;
    public float EndTime_DOF;
    public AnimationCurve AC_DOF;
    public float BeginValue_FocusDist_DOF;
    public float EndValue_FocusDist_DOF;
    [Space(10f)]
    public float BeginValue_FarBlur_SampleCount_DOF;
    public float EndValue_FarBlur_SampleCount_DOF;
    [Space(10f)]
    public float BeginValue_FarBlur_MaxRadius_DOF;
    public float EndValue_FarBlur_MaxRadius_DOF;
    

    [Space(30f)]
    [Header("ChromaticAberration")]
    public float BeginTime_CA;
    public float EndTime_CA;
    public AnimationCurve AC_CA;
    public float BeginValue_CA;
    public float EndValue_CA;
    

    // Start is called before the first frame update
    void Start()
    {
        Begin = false;
        if(volume?.profile) {
            volume.profile.TryGet<ChromaticAberration>(out _CA);
            volume.profile.TryGet<ColorAdjustments>(out _CAS);
            volume.profile.TryGet<DepthOfField>(out _DOF);
        }
    }

    // Update is called once per frame
    void Update()
    {
        EndTimeArray = new float[]
        {
            EndTime_LR, EndTime_PostExposure_CAS, EndTime_ColorFilter_CAS, EndTime_DOF, EndTime_CA
        };
        if (Begin)
        {
            timer += Time.deltaTime;
            StartCoroutine(Begin_LR());
            StartCoroutine(Begin_PostExposure_CAS());
            StartCoroutine(Begin_ColorFilter_CAS());
            StartCoroutine(Begin_CA());
            StartCoroutine(Begin_DOF());
            StartCoroutine(EndEffect());
        }
        else
        {
            timer = 0;
            StopAllCoroutines();
        }          
    }

    private IEnumerator Begin_LR()
    {
        yield return new WaitForSeconds(BeginTime_LR);
        if(timer > BeginTime_LR)
        {
            float rotateScale = BeginValue_LR + (EndValue_LR - BeginValue_LR) *
                AC_LR.Evaluate((timer - BeginTime_LR) / (EndTime_LR - BeginTime_LR));
            LensRotateMat.SetFloat("_RotateScale", rotateScale);
        }                
    }

    private IEnumerator Begin_PostExposure_CAS()
    {
        yield return new WaitForSeconds(BeginTime_PostExposure_CAS);
        if(timer > BeginTime_PostExposure_CAS)
        {
            float postExposure = BeginValue_PostExposure_CAS + (EndValue_PostExposure_CAS - BeginValue_PostExposure_CAS) *
                AC_PostExposure_CAS.Evaluate((timer - BeginTime_PostExposure_CAS) / (EndTime_PostExposure_CAS - BeginTime_PostExposure_CAS));
            _CAS.postExposure.SetValue(new FloatParameter(postExposure));            
        }
    }

    private IEnumerator Begin_ColorFilter_CAS()
    {
        yield return new WaitForSeconds(BeginTime_ColorFilter_CAS);
        if (timer > BeginTime_ColorFilter_CAS)
        {
            float colorFilter_r = BeginColor_ColorFilter_CAS.r + (EndColor_ColorFilter_CAS.r - BeginColor_ColorFilter_CAS.r) *
                AC_ColorFilter_CAS.Evaluate((timer - BeginTime_ColorFilter_CAS) / (EndTime_ColorFilter_CAS - BeginTime_ColorFilter_CAS));
            float colorFilter_g = BeginColor_ColorFilter_CAS.g + (EndColor_ColorFilter_CAS.g - BeginColor_ColorFilter_CAS.g) *
                AC_ColorFilter_CAS.Evaluate((timer - BeginTime_ColorFilter_CAS) / (EndTime_ColorFilter_CAS - BeginTime_ColorFilter_CAS));
            float colorFilter_b = BeginColor_ColorFilter_CAS.b + (EndColor_ColorFilter_CAS.b - BeginColor_ColorFilter_CAS.b) *
                AC_ColorFilter_CAS.Evaluate((timer - BeginTime_ColorFilter_CAS) / (EndTime_ColorFilter_CAS - BeginTime_ColorFilter_CAS));
            float colorFilter_a = BeginColor_ColorFilter_CAS.a + (EndColor_ColorFilter_CAS.a - BeginColor_ColorFilter_CAS.a) *
                AC_ColorFilter_CAS.Evaluate((timer - BeginTime_ColorFilter_CAS) / (EndTime_ColorFilter_CAS - BeginTime_ColorFilter_CAS));
            _CAS.colorFilter.SetValue(new ColorParameter(new Color(colorFilter_r, colorFilter_g, colorFilter_b, colorFilter_a)));
        }
    }

    private IEnumerator Begin_DOF()
    {
        yield return new WaitForSeconds(BeginTime_DOF);
        if(timer > BeginTime_DOF)
        {
            float focusDist = BeginValue_FocusDist_DOF + (EndValue_FocusDist_DOF - BeginValue_FocusDist_DOF) *
                AC_DOF.Evaluate((timer - BeginTime_DOF) / (EndTime_DOF - BeginTime_DOF));
            float sampleCount = BeginValue_FarBlur_SampleCount_DOF + (EndValue_FarBlur_SampleCount_DOF - BeginValue_FarBlur_SampleCount_DOF) *
                AC_DOF.Evaluate((timer - BeginTime_DOF) / (EndTime_DOF - BeginTime_DOF));
            float maxRadius = BeginValue_FarBlur_MaxRadius_DOF + (EndValue_FarBlur_MaxRadius_DOF - BeginValue_FarBlur_MaxRadius_DOF) *
                AC_DOF.Evaluate((timer - BeginTime_DOF) / (EndTime_DOF - BeginTime_DOF));
            _DOF.focusDistance.SetValue(new FloatParameter(focusDist));
            _DOF.farSampleCount = (int)sampleCount;
            _DOF.farMaxBlur = (int)maxRadius;
        }
    }

    private IEnumerator Begin_CA()
    {
        yield return new WaitForSeconds(BeginTime_CA);
        if (timer > BeginTime_CA)
        {
            float intensity = BeginValue_CA + (EndValue_CA - BeginValue_CA) *
                AC_CA.Evaluate((timer - BeginTime_CA) / (EndTime_CA - BeginTime_CA));
            _CA.intensity = new ClampedFloatParameter(intensity, 0, 1);
        }
    }

    private IEnumerator EndEffect()
    {
        yield return new WaitForSeconds(CalculateLength(EndTimeArray));
        Begin = false;        
    }

    private float CalculateLength(float[] EndTimeArray)
    {
        float Max = 0;
        for (int i = 0; i < EndTimeArray.Length; i++)
        {
            Max = EndTimeArray[i] > Max ? EndTimeArray[i] : Max;
        }
        return Max;       
    }
}
