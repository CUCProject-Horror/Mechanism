Shader "Unlit/S_UIUI"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScanLineJitter("ScanLineJitter", Range(0, 1)) = 0
        _HorizontalShake("HorizontalShake", float) = 0
        _ColorDrift("ColorDrift", float) = 0
    }
    SubShader
    {
        Tags{"Queue" = "Transparent" "RenderType" = "Transparent"}
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag            

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST; 
            fixed _ScanLineJitter;
            float _HorizontalShake;
            float _ColorDrift;

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

            float SimpleNoise(float x, float y)
            {
                return frac(sin(dot(float2(x, y), float2(12.9898, 78.2333))) * 43758.5453);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float shake = (SimpleNoise(_Time.x, 2) - 0.5) * _HorizontalShake;
                float jitter = SimpleNoise(i.uv.y, _Time.x) * 2 - 1;
                jitter *= step(1 - _ScanLineJitter, abs(jitter));

                float drift = sin(_Time.y * 606.11) * _ColorDrift;

                //float4 col = tex2D(_MainTex, frac(float2(i.uv.x + jitter + shake, i.uv.y)));
                //float4 col = tex2D(_MainTex, frac(float2(i.uv.x + shake, i.uv.y)));
                half4 src1 = tex2D(_MainTex, frac(float2(i.uv.x + jitter + shake, i.uv.y)));
                half4 src2 = tex2D(_MainTex, frac(float2(i.uv.x + jitter + shake + drift, i.uv.y)));                
                return half4(src1.r, src2.g, src1.b, src1.a);
            }
            ENDCG
        }
    }
}
