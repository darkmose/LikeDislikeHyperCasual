Shader "HC Team/Skybox/Gradient"
{
    Properties
    {
        _MinColor ("Min Color", Color) = (0, 0, 0, 1)
        _MinLocation("Min Location", Range(-1, 1)) = 0
    
        _MaxColor ("Max Color", Color) = (1, 1, 1, 1)
        _MaxLocation("Max Location", Range(-1, 1)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _MinColor;
            fixed _MinLocation;
            
            fixed4 _MaxColor;
            fixed _MaxLocation;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed t = smoothstep(_MinLocation, _MaxLocation, i.uv.y);
                return lerp(_MinColor, _MaxColor, t);
            }
            ENDCG
        }
    }
}
