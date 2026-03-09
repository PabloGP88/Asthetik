Shader "Custom/SegmentedGlowTrail"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _GlowStrength ("Glow Strength", Range(0.5, 3)) = 1.5
        _Segments ("Color Segments", Range(2, 8)) = 4
        _Sharpness ("Segment Sharpness", Range(0.1, 1)) = 0.8
    }
    
    SubShader
    {
        Tags { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "IgnoreProjector"="True"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        Lighting Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_particles
            
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed _GlowStrength;
            fixed _Segments;
            fixed _Sharpness;
            
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample texture
                fixed4 tex = tex2D(_MainTex, i.texcoord);
                
                // Create segmented effect along the trail length
                // Using V coordinate (along trail) to create bands
                float trailPosition = i.texcoord.y;
                
                // Create stepped gradient instead of smooth
                float segmented = floor(trailPosition * _Segments) / _Segments;
                
                // Add some sharpness to the transitions
                float sharp = smoothstep(segmented - (1.0 - _Sharpness) * 0.5, 
                                       segmented + (1.0 - _Sharpness) * 0.5, 
                                       trailPosition);
                
                // Mix between segmented and original for control
                float finalGradient = lerp(segmented, sharp, _Sharpness);
                
                // Apply colors and glow
                fixed4 col = tex * _Color * i.color;
                
                // Modulate brightness based on segment position
                col.rgb *= _GlowStrength * (0.5 + finalGradient * 0.5);
                
                // Keep alpha as is for proper fading
                return col;
            }
            ENDCG
        }
    }
    
    FallBack "Mobile/Particles/Alpha Blended"
}