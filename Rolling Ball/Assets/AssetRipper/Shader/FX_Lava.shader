Shader "FX/Lava" {
	Properties {
		_WaveScale ("Wave scale", Range(0.02, 0.15)) = 0.063
		WaveSpeed ("Wave speed (map1 x,y; map2 x,y)", Vector) = (19,9,-16,-7)
		_HorizonColor ("Simple water horizon color", Vector) = (0.172,0.463,0.435,1)
		[NoScaleOffset] _ReflectiveColor ("Reflective color (RGB) fresnel (A) ", 2D) = "" {}
		[HideInInspector] _ReflectionTex ("Internal Reflection", 2D) = "" {}
		[HideInInspector] _RefractionTex ("Internal Refraction", 2D) = "" {}
		[NoScaleOffset] _BumpMap ("Normalmap ", 2D) = "bump" {}
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