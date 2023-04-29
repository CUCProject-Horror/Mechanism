using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINoise : MonoBehaviour
{
    public bool Begin;
    [Space(15f)]
    [SerializeField] private Material mat;
    [Space(15f)]
    [SerializeField] private float GlitchTime;

    [Space(15f)]
    [SerializeField] private float InitScanLineJitter;
    [SerializeField] private float GlitchScanLineJitter;

    [Space(15f)]
    [SerializeField] private float InitHorizontalShake;
    [SerializeField] private float GlitchHorizontalShake;
    [Space(15f)]
    [SerializeField] private float InitColorDrift;
    [SerializeField] private float GlitchColorDrift;
    [SerializeField] private AnimationCurve AC;
    private float timer = 0;

    private void OnEnable()
    {
        timer = 0;
        Begin = false;
        mat.SetFloat("_ScanLineJitter", InitScanLineJitter);
        mat.SetFloat("_HorizontalShake", InitHorizontalShake);
        mat.SetFloat("_ColorDrift", InitColorDrift);
    }

    private void Update()
    {
        if(Begin)
        {
            timer += Time.unscaledDeltaTime;
            StartCoroutine(Counter());
            Glitch();
        }
        else
        {
            StopAllCoroutines();
            timer = 0;
        }

        mat.SetFloat("_ShakeTime", Time.unscaledDeltaTime);
    }

    public void Glitch()
    {
        if (timer <= GlitchTime)
        {
            float _ScanLineJitter = InitScanLineJitter + (GlitchScanLineJitter - InitScanLineJitter) *
                AC.Evaluate(timer / GlitchTime);
            float _ColorDrift = InitColorDrift + (GlitchColorDrift - InitColorDrift) *
                AC.Evaluate(timer / GlitchTime);
            float _HorizontalShake = InitHorizontalShake + (GlitchHorizontalShake - InitHorizontalShake) *
                AC.Evaluate(timer / GlitchTime);
            Debug.Log(timer);
            mat.SetFloat("_ScanLineJitter", _ScanLineJitter);
            mat.SetFloat("_HorizontalShake", _HorizontalShake);
            mat.SetFloat("_ColorDrift", _ColorDrift);
        }         
    }

    private IEnumerator Counter()
    {
        yield return new WaitForSecondsRealtime(GlitchTime);
        Begin = false;
        mat.SetFloat("_ScanLineJitter", InitScanLineJitter);
        mat.SetFloat("_HorizontalShake", InitHorizontalShake);
        mat.SetFloat("_ColorDrift", InitColorDrift);
    }
}
