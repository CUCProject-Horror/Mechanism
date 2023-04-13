Shader "Unlit/S_OBJGG"
{
    Properties
    {       
        _NoiseAmount("NoiseAmout", Range(0, 100)) = 50
        _GlitchStrength("GlitchStrength", Range(0, 10)) = 1       
        _GlitchNoiseIntensity("GlitchNoiseIntensity", Range(0, 5)) = 1
        _ScanLineIntensity("ScanLineIntensity", Range(0, 1)) = 0        
        _NoiseIntensity("NoiseIntensity", Range(0, 1)) = 1  
        _ColorTex("ColorTex", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }       

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag            

            #include "UnityCG.cginc"

            sampler2D _ColorTex;
            float4 _ColorTex_ST;
            float _NoiseAmount;
            float _GlitchStrength;
            float _GlitchNoiseIntensity;
            fixed _ScanLineIntensity;
            fixed _NoiseIntensity; 

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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;                
                float4 vertex : SV_POSITION;
            };            

            v2f vert (appdata v)
            {
                v2f o;
                float4 Vpos = UnityObjectToClipPos(v.vertex);
                //o.vertex = Vpos + float4(0,0,0,0) * _Offset;
                float2 noiseUV = float2(1, 1) * (_Time.y * _GlitchStrength + Vpos.y);
                float GNoise = Unity_GradientNoise_float(noiseUV, _NoiseAmount);
                float Noise = remap(GNoise, 0, 1.0f, -1.0f, 1.0f);        

                float2 glitchUV = float2(1, 1) * (_Time.y * _GlitchStrength);
                float GradientNoise = Unity_GradientNoise_float(glitchUV, 10);
                float Flickering = pow(GradientNoise, 4) * 0.1f;

                float2 UVOffset = float2(Flickering * Noise * _GlitchNoiseIntensity, 0);
                o.vertex = Vpos + float4(UVOffset.x, 0, 0, 0); 
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float GenerateScanLine = sin(i.vertex.y * 600 + _Time.y * _GlitchStrength);
                float ClampSL = clamp(GenerateScanLine, 1 - _ScanLineIntensity, 1);
                float ScanLine = remap(ClampSL, -1, 1, 0.2, 1);

                // sample the texture
                fixed4 OriginColor = tex2D(_ColorTex, i.uv);               
                float4 finalColor = lerp(OriginColor, SimpleNoise(i.vertex.xy * _Time.x), _NoiseIntensity) * ScanLine;
                return finalColor;
            }
            ENDCG
        }
    }
}
