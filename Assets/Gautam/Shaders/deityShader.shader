Shader "Custom/ShimmerSprite"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _ShimmerColor ("Shimmer Color", Color) = (1,1,1,0.5)
        _ShimmerWidth ("Shimmer Width", Range(0, 1)) = 0.1
        _ShimmerSpeed ("Shimmer Speed", Range(0, 5)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend One OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _ShimmerColor;
            float _ShimmerWidth;
            float _ShimmerSpeed;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 texColor = tex2D(_MainTex, i.uv) * i.color;

                // Calculate the shimmer position over time
                float time = _Time.y * _ShimmerSpeed;
                float shimmerPos = fmod(time, 1.2 + _ShimmerWidth) - _ShimmerWidth;

                // Check if the current pixel is within the shimmer band
                if (i.uv.x > shimmerPos && i.uv.x < shimmerPos + _ShimmerWidth) {
                    // Add the shimmer color to the texture color
                    texColor.rgb += _ShimmerColor.rgb * _ShimmerColor.a;
                }

                // Make sure the alpha from the original texture is respected
                if (texColor.a < 0.1) discard;

                return texColor;
            }
            ENDCG
        }
    }
}