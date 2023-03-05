Shader "FullScreen/S_GlitchAndNoise"
{
    Properties
    {       
        _NoiseAmount("NoiseAmout", Range(0, 100)) = 50
        _GlitchStrength("GlitchStrength", Range(0, 10)) = 1       
        _GlitchNoiseIntensity("GlitchNoiseIntensity", Range(0, 5)) = 1
        _ScanLineIntensity("ScanLineIntensity", Range(0, 1)) = 0
        //_MaskTex("MaskTex", 2D) = "white" {}
        _NoiseIntensity("NoiseIntensity", Range(0, 1)) = 1
    }
    HLSLINCLUDE

    #pragma vertex Vert

    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"

    // The PositionInputs struct allow you to retrieve a lot of useful information for your fullScreenShader:
    // struct PositionInputs
    // {
    //     float3 positionWS;  // World space position (could be camera-relative)
    //     float2 positionNDC; // Normalized screen coordinates within the viewport    : [0, 1) (with the half-pixel offset)
    //     uint2  positionSS;  // Screen space pixel coordinates                       : [0, NumPixels)
    //     uint2  tileCoord;   // Screen tile coordinates                              : [0, NumTiles)
    //     float  deviceDepth; // Depth from the depth buffer                          : [0, 1] (typically reversed)
    //     float  linearDepth; // View space Z coordinate                              : [Near, Far]
    // };

    // To sample custom buffers, you have access to these functions:
    // But be careful, on most platforms you can't sample to the bound color buffer. It means that you
    // can't use the SampleCustomColor when the pass color buffer is set to custom (and same for camera the buffer).
    // float4 SampleCustomColor(float2 uv);
    // float4 LoadCustomColor(uint2 pixelCoords);
    // float LoadCustomDepth(uint2 pixelCoords);
    // float SampleCustomDepth(float2 uv);

    // There are also a lot of utility function you can use inside Common.hlsl and Color.hlsl,
    // you can check them out in the source code of the core SRP package.

     float2 unity_gradientNoise_dir(float2 p)
    {
        p = p % 289;
        float x = (34 * p.x + 1) * p.x % 289 + p.y;
        x = (34 * x + 1) * x % 289;
        x = frac(x / 41) * 2 - 1;
        return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
    }

    float unity_gradientNoise(float2 p)
    {
        float2 ip = floor(p);
        float2 fp = frac(p);
        float d00 = dot(unity_gradientNoise_dir(ip), fp);
        float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
        float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
        float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
        fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
        return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
    }

    float Unity_GradientNoise_float(float2 UV, float Scale)
    {
        float n = unity_gradientNoise(UV * Scale) + 0.5;
        return n;
    }

    half remap(half x, half t1, half t2, half s1, half s2)
    {
        return (x - t1) / (t2 - t1) * (s2 - s1) + s1;
    }

    float SimpleNoise(float2 uv)
    {
        return frac(sin(dot(uv,float2(12.9898, 78.233))) * 43758.5453);
    }

       
    float _GlitchStrength; 
    float _NoiseAmount;     
    float _GlitchNoiseIntensity;
    float _ScanLineIntensity;
    float _NoiseIntensity;
    //sampler2D _MaskTex;    

    float4 FullScreenPass(Varyings varyings) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(varyings);
        float depth = LoadCameraDepth(varyings.positionCS.xy);
        PositionInputs posInput = GetPositionInput(varyings.positionCS.xy, _ScreenSize.zw, depth, UNITY_MATRIX_I_VP, UNITY_MATRIX_V);

        float4 c = LoadCustomColor(posInput.positionSS);
        
        float3 viewDirection = GetWorldSpaceNormalizeViewDir(posInput.positionWS);
        float4 color = float4(0.0, 0.0, 0.0, 0.0);

        // Load the camera color buffer at the mip 0 if we're not at the before rendering injection point
        if (_CustomPassInjectionPoint != CUSTOMPASSINJECTIONPOINT_BEFORE_RENDERING)
				color = float4(CustomPassSampleCameraColor(posInput.positionNDC.xy, 0), 1);	

        // Add your custom pass code here        
        float2 noiseUV = float2(1, 1) * (_Time.y * _GlitchStrength + posInput.positionNDC.y);
        float GNoise = Unity_GradientNoise_float(noiseUV, _NoiseAmount);
        float Noise = remap(GNoise, 0, 1.0f, -1.0f, 1.0f);        

        float2 glitchUV = float2(1, 1) * (_Time.y * _GlitchStrength);
        float GradientNoise = Unity_GradientNoise_float(glitchUV, 10);
        float Flickering = pow(GradientNoise, 4) * 0.1f;

        float2 UVOffset = float2(Flickering * Noise * _GlitchNoiseIntensity, 0);

        float GenerateScanLine = sin(posInput.positionNDC.y * 600 + _Time.y * _GlitchStrength);
        float ClampSL = clamp(GenerateScanLine, 1 - _ScanLineIntensity, 1);
        float ScanLine = remap(ClampSL, -1, 1, 0.2, 1);

        //float4 MaskColor = step(tex2D(_MaskTex, posInput.positionNDC), 0.4);

        //float3 finalColor = CustomPassSampleCameraColor(posInput.positionNDC.xy + UVOffset, 0).rgb * MaskColor.x * ScanLine +
                            //CustomPassSampleCameraColor(posInput.positionNDC.xy, 0).rgb; 
        float3 finalColor = CustomPassSampleCameraColor(posInput.positionNDC.xy + UVOffset, 0).rgb * ScanLine;

        //float3 result = lerp(CustomPassSampleCameraColor(posInput.positionNDC.xy, 0).rgb, finalColor, c.a);
        float3 AppendColor = finalColor * (1 - _NoiseIntensity) + SimpleNoise(posInput.positionNDC.xy * _Time.x) * _NoiseIntensity;
        float3 Output = lerp(CustomPassSampleCameraColor(posInput.positionNDC.xy, 0).rgb, AppendColor, c.a);
        //Fade value allow you to increase the strength of the effect while the camera gets closer to the custom pass volume
        //float f = 1 - abs(_FadeValue * 2 - 1);
        //return float4((color.rgb + f) * ScanLine, color.a);
        return float4(Output, 1);
    }

    ENDHLSL

    SubShader
    {
        Tags{ "RenderPipeline" = "HDRenderPipeline" }
        Pass
        {
            Name "Custom Pass 0"

            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            HLSLPROGRAM
                #pragma fragment FullScreenPass
            ENDHLSL
        }
    }
    Fallback Off
}
