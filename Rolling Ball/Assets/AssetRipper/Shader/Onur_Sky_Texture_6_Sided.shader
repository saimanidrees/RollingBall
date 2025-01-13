Shader "Onur/Sky_Texture_6_Sided" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MinDistance ("Min Distance", Float) = 0
		_MaxDistance ("Max Distance", Float) = 1
		_Rotation ("Rotation", Range(0, 360)) = 0
		[NoScaleOffset] _FrontTex ("Front [+Z] ", 2D) = "white" {}
		[NoScaleOffset] _BackTex ("Back [-Z] ", 2D) = "white" {}
		[NoScaleOffset] _LeftTex ("Left [+X] ", 2D) = "white" {}
		[NoScaleOffset] _RightTex ("Right [-X] ", 2D) = "white" {}
		[NoScaleOffset] _UpTex ("Up [+Y] ", 2D) = "white" {}
		[NoScaleOffset] _DownTex ("Down [-Y] ", 2D) = "white" {}
		_Hue ("Hue", Range(-360, 360)) = 0
		_Brightness ("Brightness", Range(-1, 1)) = 0
		_Contrast ("Contrast", Range(0, 2)) = 1
		_Saturation ("Saturation", Range(0, 2)) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
}