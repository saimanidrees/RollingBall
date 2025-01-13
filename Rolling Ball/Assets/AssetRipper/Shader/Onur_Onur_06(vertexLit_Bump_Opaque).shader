Shader "Onur/Onur_06(vertexLit_Bump_Opaque)" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Main (RGB)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		[PowerSlider(5.0)] _Specular ("Specular", Range(0.03, 5)) = 0.5
		[PowerSlider(5.0)] _Glossiness ("Glossiness", Range(0, 5)) = 0.25
		[Space(10)] [Toggle(_BUMP_OFF)] _BUMP_OFF ("Dont Use Bump", Float) = 0
		[Space(10)] [Toggle(_SPECULAR_OFF)] _SPECULAR_OFF ("Dont Use Specular", Float) = 0
		[Space(10)] [Toggle(_SHADOW_OFF)] _SHADOW_OFF ("Dont Use Shadow", Float) = 0
		[Space(10)] [Toggle(_EMISSION_ON)] _EMISSION_ON ("Use Emission", Float) = 0
		[NoScaleOffset] _Emission ("Emission", 2D) = "black" {}
		_EmissionColor1 ("Emission Color1", Vector) = (1,1,1,1)
		_EmissionColor2 ("Emission Color2", Vector) = (1,1,1,1)
		_EmissionSpeed ("Emission Speed", Float) = 2
		_EmissionTime ("Emission Start Time", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Legacy Shaders/Transparent/Cutout/VertexLit"
}