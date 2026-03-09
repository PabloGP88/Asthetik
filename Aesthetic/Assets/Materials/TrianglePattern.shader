Shader "Custom/TrianglePattern"
{
    Properties
    {
        _Color1("Color 1", Color) = (1,1,1,1)
        _Color2("Color 2", Color) = (0.9,0.9,0.9,1)
        _Speed("Scroll Speed", Float) = 0.5
        _Size("Triangle Size", Float) = 10
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
            float _Speed;
            float _Size;

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

            // Helper function to tile triangles
            float triangleTile(float2 uv)
            {
                // Skew for triangular pattern
                uv.x += uv.y * 0.5;
                uv *= _Size;

                // Scroll diagonally
                uv += _Time.y * _Speed;

                float2 id = floor(uv);
                float2 local = frac(uv);

                // Flip every other row - Branch-free version for better compatibility
                float modY = id.y - floor(id.y / 2.0) * 2.0;
                float flip = step(0.5, modY);
                local.x = lerp(local.x, 1.0 - local.x, flip);

                // Inside triangle condition
                float inside = step(local.x, 1.0 - local.y);
                return inside;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float t = triangleTile(i.uv);
                return lerp(_Color2, _Color1, t);
            }
            ENDCG
        }
    }
}