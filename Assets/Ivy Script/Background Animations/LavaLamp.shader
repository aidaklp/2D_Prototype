Shader "Custom/LavaLamp"
{
    Properties
    {
        _Speed ("Blob Speed", Float) = 0.4
        _Scale ("Blob Scale", Float) = 3.0
        _Smoothness ("Edge Smoothness", Float) = 6.0
        _ColorBottom ("Color Bottom", Color) = (1.0, 0.2, 0.0, 1.0)
        _ColorMid ("Color Mid", Color) = (1.0, 0.05, 0.1, 1.0)
        _ColorTop ("Color Top", Color) = (0.9, 0.0, 0.6, 1.0)
        _BgColor ("Background Color", Color) = (0.0, 0.0, 0.0, 1.0)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; };
            struct Varyings { float2 uv : TEXCOORD0; float4 positionHCS : SV_POSITION; };

            float _Speed, _Scale, _Smoothness;
            float4 _ColorBottom, _ColorMid, _ColorTop, _BgColor;

            float2 hash2(float2 p)
            {
                p = float2(dot(p, float2(127.1, 311.7)),
                           dot(p, float2(269.5, 183.3)));
                return frac(sin(p) * 43758.5453);
            }

            float metaball(float2 uv, float2 center, float radius)
            {
                float d = length(uv - center);
                return radius / (d * d + 0.001);
            }

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float2 uv = i.uv;
                float t = _Time.y * _Speed;

                float field = 0.0;

                for (int b = 0; b < 10; b++)
                {
                    float fb = float(b);
                    float2 seed = hash2(float2(fb, fb * 1.3 + 7.0));

                    float xPos = seed.x;
                    float speed = 0.03 + seed.y * 0.06;
                    float size = 0.04 + seed.x * 0.04;
                    float yPos = frac(seed.y + t * speed);
                    float sway = sin(t * 0.5 + fb * 2.0) * 0.04;

                    float2 blobCenter = float2(xPos + sway, yPos);
                    field += metaball(uv * _Scale, blobCenter * _Scale, size);
                }

               float edgeFade = smoothstep(0.0, 0.08, i.uv.y);
float blob = smoothstep(1.0, 1.02, field) * edgeFade;

                float3 blobColor;
                if (i.uv.y < 0.4)
                    blobColor = lerp(_ColorBottom.rgb, _ColorMid.rgb, i.uv.y / 0.4);
                else
                    blobColor = lerp(_ColorMid.rgb, _ColorTop.rgb, (i.uv.y - 0.4) / 0.6);

                float3 bgColor = lerp(float3(0.4, 0.05, 0.0), _BgColor.rgb, i.uv.y);
                float3 finalColor = lerp(bgColor, blobColor, blob);
                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}