Shader "FX/WaterGrabPass" {
	Properties {
		_WaveScale ("Wave scale", Range(0.02, 0.15)) = 0.063
		_ReflDistort ("Reflection distort", Range(0, 1.5)) = 0.44
		_RefrDistort ("Refraction distort", Range(0, 1.5)) = 0.4
		_RefrColor ("Refraction color", Vector) = (0.34,0.85,0.92,1)
		[NoScaleOffset] _Fresnel ("Fresnel (A) ", 2D) = "gray" {}
		[NoScaleOffset] _BumpMap ("Normalmap ", 2D) = "bump" {}
		WaveSpeed ("Wave speed (map1 x,y; map2 x,y)", Vector) = (19,9,-16,-7)
		[HideInInspector] _ReflectionTex ("Internal Reflection", 2D) = "" {}
		[PowerSlider(5.0)] _Specular ("Specular", Range(0.03, 1)) = 0.078125
		[PowerSlider(5.0)] _Glossiness ("Glossiness", Range(0, 5)) = 0.5
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
}