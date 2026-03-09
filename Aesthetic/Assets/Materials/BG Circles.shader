Shader "Custom/BG Circles"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (0.95, 0.95, 0.95, 1)
        _Color2 ("Color 2", Color) = (0.85, 0.85, 0.85, 1)
        _Speed ("Speed", Float) = 1
        _Frequency ("Frequency", Float) = 5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _Color1;
            float4 _Color2;
            float _Speed;
            float _Frequency;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * 2 - 1; // Normalize UV from -1 to 1
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2(0.0, 0.0);
                float dist = length(i.uv - center); // Distance from center

                float t = _Time.y * _Speed;
                float ripple = frac(dist * _Frequency - t); // Creates moving concentric circles

                float pattern = step(0.5, ripple);
                return lerp(_Color1, _Color2, pattern);
            }
            ENDCG
        }
    }
}