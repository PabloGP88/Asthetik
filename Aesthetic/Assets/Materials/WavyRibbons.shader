Shader "Custom/WavyRibbons"
{
    Properties
    {
        _Color1("Color 1", Color) = (1,1,1,1)
        _Color2("Color 2", Color) = (0.9,0.9,0.9,1)
        _Speed("Wave Speed", Float) = 1.0
        _Frequency("Wave Frequency", Float) = 5.0
        _Amplitude("Wave Amplitude", Float) = 0.05
        _BandHeight("Band Height", Float) = 0.2
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
            float _Frequency;
            float _Amplitude;
            float _BandHeight;

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
                float time = _Time.y * _Speed;

                // Scroll UV vertically
                float2 uv = i.uv;
                uv.y += time;

                // Wave offset
                float offset = sin(uv.y * _Frequency + uv.x * 4.0 + time * 0.5) * _Amplitude;

                // Determine band index
                float bandIndex = floor((uv.y + offset) / _BandHeight);

                // Alternate color based on even/odd bands
                float t = fmod(bandIndex, 2.0);
                return (t < 1.0) ? _Color1 : _Color2;
            }
            ENDCG
        }
    }
}
