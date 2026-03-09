Shader "Custom/BG Lines"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (0.2, 0.8, 0.2, 1)
        _Color2 ("Color 2", Color) = (0.1, 0.7, 0.1, 1)
        _Speed ("Speed", Float) = 0.5
        _Frequency ("Frequency", Float) = 10
        _Angle ("Angle", Range(0, 360)) = 45
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
            
            // Add mobile platform support
            #pragma target 2.0
            #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON
            
            #include "UnityCG.cginc"

            // Use half precision for mobile optimization
            half4 _Color1;
            half4 _Color2;
            half _Speed;
            half _Frequency;
            half _Angle;

            struct appdata 
            { 
                float4 vertex : POSITION; 
                half2 uv : TEXCOORD0; 
            };
            
            struct v2f 
            { 
                half2 uv : TEXCOORD0; 
                float4 vertex : SV_POSITION; 
            };

            v2f vert (appdata v) 
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target 
            {
                // Use half precision for mobile performance
                half t = _Time.y * _Speed;
                
                // More stable angle calculation for mobile
                half angleRad = _Angle * 0.017453292h; // Use half precision constant
                half2 dir = half2(cos(angleRad), sin(angleRad));
                
                // Ensure UV coordinates are properly normalized
                half2 uv = i.uv;
                
                // Create the stripe pattern with better precision
                half dotProduct = dot(uv, dir);
                half stripe = frac(dotProduct * _Frequency + t);
                
                // Use smoothstep for better mobile compatibility instead of step
                half linePattern = smoothstep(0.45h, 0.55h, stripe);
                
                // Explicit color blending
                return lerp(_Color1, _Color2, linePattern);
            }
            ENDCG
        }
    }
    
    SubShader
    {
        // Fallback for very old mobile devices
        Tags { "RenderType"="Opaque" }
        LOD 50
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #include "UnityCG.cginc"

            half4 _Color1;
            half4 _Color2;

            struct appdata { float4 vertex : POSITION; half2 uv : TEXCOORD0; };
            struct v2f { half2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            v2f vert (appdata v) 
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target 
            {
                // Simple fallback pattern
                half pattern = step(0.5h, frac(i.uv.x * 10.0h));
                return lerp(_Color1, _Color2, pattern);
            }
            ENDCG
        }
    }
    
    FallBack "Mobile/Unlit (Supports Lightmap)"
}