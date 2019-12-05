Shader "Custom/PlayerBallShader"
{
    Properties
    {
        [PerRendererData] _Color ("Color", Color) = (1,1,1,1)
		_MainTex("MainTex", 2D) = "red"{}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;
        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutput o)
        {
			o.Albedo = float4(lerp(_Color.rgb, _Color.bgr, IN.uv_MainTex.x),1);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
