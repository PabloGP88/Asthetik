Shader "Unlit/FloatingDots"
{
    Properties
    {
        _Color1("Color 1", Color) = (0.2,0.3,0.8,1)
        _Color2("Color 2", Color) = (0.1,0.1,0.1,1)
        _DotSize("Dot Size", Float) = 0.05
        _Spacing("Dot Spacing", Float) = 8.0
        _Speed("Float Speed", Float) = 0.5
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

            fixed4 _Color1;
            fixed4 _Color2;
            float _DotSize;
            float _Spacing;
            float _Speed;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Scroll vertically
                uv.y += _Time.y * _Speed;

                // Dot grid position
                float2 gridUV = uv * _Spacing;
                float2 id = floor(gridUV);
                float2 local = frac(gridUV) - 0.5;

                // Dot "pulse" effect
                float r = length(local);
                float pulse = 0.5 + 0.5 * sin(_Time.y * 2.0 + id.x * 0.5 + id.y);

                // Create dot mask: 1 = dot area, 0 = background
                float dotMask = 1.0 - smoothstep(_DotSize * pulse * 0.8, _DotSize * pulse, r);

                // FIXED: Properly blend between background and dot colors
                // When dotMask = 0: show background (_Color1)
                // When dotMask = 1: show dot color (_Color2)
                return lerp(_Color1, _Color2, dotMask);
            }
            ENDCG
        }
    }
}