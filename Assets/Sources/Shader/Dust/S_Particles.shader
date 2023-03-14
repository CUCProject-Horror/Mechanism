Shader "Hidden/S_Particles"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ParticleColor("ParticleColor", Color) = (0, 0, 0, 0)
        _AlphaOffset("AlphaOffset", Range(0, 0.5)) = 0
    }
    SubShader
    {
        Tags{"Queue" = "Transparent"}
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _ParticleColor;
            fixed4 _AlphaOffset;

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
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed SinAlpha(fixed a)
            {
                fixed output;
                output = a * 3/2 * sin( UNITY_PI * _Time.y + 3 * UNITY_PI / 2) + 1.5 * a;
                return clamp(output,0,a);            
            } 
            
            int Random(float d)
            {
                return floor(sin(dot(float2(d,42.87863), float2(42.357968, d))) * 43.757) % 4;
            }
            
            
            

            fixed4 frag (v2f i) : SV_Target
            {
                //fixed4 col = tex2D(_MainTex, i.uv /2);
                //fixed4 col = tex2D(_MainTex, i.uv / 2 + 0.5);
                //fixed4 col = tex2D(_MainTex, i.uv / 2 + fixed2(0.5, 0));
                fixed4 col = tex2D(_MainTex, i.uv / 2);
                fixed alpha = lerp(0, 1, col); 
                //fixed OutputAlpha = min(alpha + _AlphaOffset, 1);
                return fixed4(_ParticleColor.rgb, alpha);
                //return fixed4(_ParticleColor);
            }
            ENDCG
        }
    }
}
