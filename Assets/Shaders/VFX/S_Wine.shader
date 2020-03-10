// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Wine"
{
	Properties
	{
		[Normal]_NoiseA("NoiseA", 2D) = "bump" {}
		[Normal]_NoiseB("NoiseB", 2D) = "bump" {}
		_Smoothness("Smoothness", Float) = 0
		_ColorA("ColorA", Color) = (1,1,1,0)
		_ColorB("ColorB", Color) = (1,1,1,0)
		[HDR]_Emission("Emission", Color) = (1,1,1,0)
		_ColorMin("ColorMin", Float) = 0
		_ColorMax("ColorMax", Float) = 1
		_Displacement("Displacement", Float) = 0
		_DispMin("DispMin", Float) = 0
		_DispMax("DispMax", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _DispMin;
		uniform float _DispMax;
		uniform sampler2D _NoiseA;
		uniform sampler2D _Sampler03;
		uniform float4 _NoiseA_ST;
		uniform float _Displacement;
		uniform sampler2D _NoiseB;
		uniform sampler2D _Sampler07;
		uniform float4 _NoiseB_ST;
		uniform float4 _ColorA;
		uniform float4 _ColorB;
		uniform float _ColorMin;
		uniform float _ColorMax;
		uniform float4 _Emission;
		uniform float _Smoothness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_TexCoord6 = v.texcoord.xy * _NoiseA_ST.xy;
			float2 panner5 = ( 1.0 * _Time.y * _NoiseA_ST.zw + uv_TexCoord6);
			float3 tex2DNode1 = UnpackNormal( tex2Dlod( _NoiseA, float4( panner5, 0, 0.0) ) );
			float temp_output_14_0 = ( tex2DNode1.g * 1.0 );
			float smoothstepResult26 = smoothstep( _DispMin , _DispMax , temp_output_14_0);
			v.vertex.xyz += ( smoothstepResult26 * float3(0,0,1) * _Displacement );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord6 = i.uv_texcoord * _NoiseA_ST.xy;
			float2 panner5 = ( 1.0 * _Time.y * _NoiseA_ST.zw + uv_TexCoord6);
			float3 tex2DNode1 = UnpackNormal( tex2D( _NoiseA, panner5 ) );
			float2 uv_TexCoord8 = i.uv_texcoord * _NoiseB_ST.xy;
			float2 panner9 = ( 1.0 * _Time.y * _NoiseB_ST.zw + uv_TexCoord8);
			o.Normal = max( tex2DNode1 , UnpackNormal( tex2D( _NoiseB, panner9 ) ) );
			float temp_output_14_0 = ( tex2DNode1.g * 1.0 );
			float smoothstepResult20 = smoothstep( _ColorMin , _ColorMax , temp_output_14_0);
			float4 lerpResult15 = lerp( _ColorA , _ColorB , smoothstepResult20);
			o.Albedo = lerpResult15.rgb;
			o.Emission = _Emission.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15900
-1920;75;1758;947;70.74017;493.4128;1;True;True
Node;AmplifyShaderEditor.TextureTransformNode;3;-1350.087,-134.482;Float;False;1;1;0;SAMPLER2D;_Sampler03;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1077.087,-197.482;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureTransformNode;7;-1249.442,82.60599;Float;False;2;1;0;SAMPLER2D;_Sampler07;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.PannerNode;5;-856.9868,-163.482;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-979.2141,24.70065;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-519.5413,-172.4189;Float;True;Property;_NoiseA;NoiseA;0;1;[Normal];Create;True;0;0;False;0;e6fd03e3048f75b4e902f1dcc5e59bff;e6fd03e3048f75b4e902f1dcc5e59bff;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;22;116.7543,-150.7629;Float;False;Property;_ColorMax;ColorMax;7;0;Create;True;0;0;False;0;1;0.29;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;109.7543,-237.7629;Float;False;Property;_ColorMin;ColorMin;6;0;Create;True;0;0;False;0;0;-0.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;100.0062,244.2215;Float;False;Property;_DispMin;DispMin;9;0;Create;True;0;0;False;0;0;-0.33;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;107.0062,331.2215;Float;False;Property;_DispMax;DispMax;10;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-118.2269,-283.7118;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;9;-744.2141,49.70065;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector3Node;23;488.5821,307.8683;Float;False;Constant;_Vector2;Vector 2;7;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SmoothstepOpNode;26;264.0062,251.2215;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;20;273.7543,-230.7629;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;12;-105.3787,-666.4625;Float;False;Property;_ColorA;ColorA;3;0;Create;True;0;0;False;0;1,1,1,0;0.4622641,0.1548149,0.3623432,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;654.0714,460.5796;Float;False;Property;_Displacement;Displacement;8;0;Create;True;0;0;False;0;0;-0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;-93.22693,-484.7118;Float;False;Property;_ColorB;ColorB;4;0;Create;True;0;0;False;0;1,1,1,0;0.3584905,0.09976856,0.3584905,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-532.1221,48.95807;Float;True;Property;_NoiseB;NoiseB;1;1;[Normal];Create;True;0;0;False;0;e6fd03e3048f75b4e902f1dcc5e59bff;e6fd03e3048f75b4e902f1dcc5e59bff;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMaxOpNode;11;-30.57923,-0.1628611;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;29;800.8894,-180.9406;Float;False;Property;_Emission;Emission;5;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0.03251034,0.02223584,0.03251034,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;182.6213,119.5374;Float;False;Property;_Smoothness;Smoothness;2;0;Create;True;0;0;False;0;0;0.95;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;15;495.7731,-351.7118;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;776.8874,249.866;Float;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1336.522,-71.57112;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Wine;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;3;0
WireConnection;5;0;6;0
WireConnection;5;2;3;1
WireConnection;8;0;7;0
WireConnection;1;1;5;0
WireConnection;14;0;1;2
WireConnection;9;0;8;0
WireConnection;9;2;7;1
WireConnection;26;0;14;0
WireConnection;26;1;27;0
WireConnection;26;2;28;0
WireConnection;20;0;14;0
WireConnection;20;1;21;0
WireConnection;20;2;22;0
WireConnection;2;1;9;0
WireConnection;11;0;1;0
WireConnection;11;1;2;0
WireConnection;15;0;12;0
WireConnection;15;1;16;0
WireConnection;15;2;20;0
WireConnection;24;0;26;0
WireConnection;24;1;23;0
WireConnection;24;2;25;0
WireConnection;0;0;15;0
WireConnection;0;1;11;0
WireConnection;0;2;29;0
WireConnection;0;4;13;0
WireConnection;0;11;24;0
ASEEND*/
//CHKSM=42715B2C2BBD1482B65E995DCD8BA5CFC82FFE39