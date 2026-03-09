Shader "Custom/ScrollingSquares"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (0.9, 0.9, 0.9, 1)
        _Color2 ("Color 2", Color) = (0.8, 0.8, 0.8, 1)
        _Speed ("Scroll Speed", Float) = 1
        _TileSize ("Tile Size (X = width, Y = height)", Vector) = (8, 8, 0, 0)
        _Direction ("Direction (X=1,Y=1 for diagonal)", Vector) = (1, 0, 0, 0)
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
            float4 _TileSize;
            float4 _Direction;

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
                // Scroll UVs based on direction and time
                float2 scrollUV = i.uv + _Direction.xy * _Time.y * _Speed;

                // Scale UVs by custom tile size (stretchable)
                float2 tile = floor(scrollUV * _TileSize.xy);

                // Checker pattern
                float checker = fmod(tile.x + tile.y, 2.0);

                return lerp(_Color1, _Color2, checker);
            }
            ENDCG
        }
    }
}
