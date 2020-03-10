// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_PBR"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset][Normal]_Normal("Normal", 2D) = "bump" {}
		[NoScaleOffset]_Height("Height", 2D) = "white" {}
		_Sub("Sub", Float) = 0
		_Displacement("Displacement", Float) = 0
		_Smoothness("Smoothness", Float) = 0
		_NormalStrenth("NormalStrenth", Float) = 1
		_OffsetStrength("OffsetStrength", Float) = 10
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Height;
		uniform sampler2D _Sampler015;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float _OffsetStrength;
		uniform float _Sub;
		uniform float _Displacement;
		uniform float _NormalStrenth;
		uniform sampler2D _Normal;
		uniform float _Smoothness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 transform13 = mul(unity_WorldToObject,float4( 0,0,0,1 ));
			float2 temp_cast_0 = (( ( transform13.x + transform13.y + transform13.z ) * _OffsetStrength )).xx;
			float2 uv_TexCoord14 = v.texcoord.xy * _Albedo_ST.xy + temp_cast_0;
			float4 temp_cast_1 = (_Sub).xxxx;
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( tex2Dlod( _Height, float4( uv_TexCoord14, 0, 0.0) ) - temp_cast_1 ) * _Displacement * float4( ase_vertexNormal , 0.0 ) ).rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 transform13 = mul(unity_WorldToObject,float4( 0,0,0,1 ));
			float2 temp_cast_0 = (( ( transform13.x + transform13.y + transform13.z ) * _OffsetStrength )).xx;
			float2 uv_TexCoord14 = i.uv_texcoord * _Albedo_ST.xy + temp_cast_0;
			o.Normal = UnpackScaleNormal( tex2D( _Normal, uv_TexCoord14 ), _NormalStrenth );
			o.Albedo = tex2D( _Albedo, uv_TexCoord14 ).rgb;
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
-1920;75;1758;947;2571.616;622.5842;1.298236;True;True
Node;AmplifyShaderEditor.WorldToObjectTransfNode;13;-2064.006,21.34059;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-1847.201,55.09481;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1734.254,195.3044;Float;False;Property;_OffsetStrength;OffsetStrength;7;0;Create;True;0;0;False;0;10;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureTransformNode;15;-1782.289,-397.9895;Float;False;10;1;0;SAMPLER2D;_Sampler015;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1460.327,-233.1135;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1346.081,-385.0073;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-950.6615,147.3716;Float;True;Property;_Height;Height;2;1;[NoScaleOffset];Create;True;0;0;False;0;cbe94f53fe50e83498d80e0fccd1d85e;20b4d0cd2efcabd4eb16a50165d86e08;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;-702.373,292.8878;Float;False;Property;_Sub;Sub;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;5;-521.373,126.8877;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.5;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-759.0818,385.994;Float;False;Property;_Displacement;Displacement;4;0;Create;True;0;0;False;0;0;0.65;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1022.652,-48.1055;Float;False;Property;_NormalStrenth;NormalStrenth;6;0;Create;True;0;0;False;0;1;5.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;12;-385.3754,513.2227;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;-841.2715,-359.9022;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;8921fb670b68070458efd8c31abec79b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-259.6362,123.7297;Float;False;Property;_Smoothness;Smoothness;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-328.0818,341.994;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;11;-839.2715,-132.9021;Float;True;Property;_Normal;Normal;1;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;826f80ee0ad07444c8558af826a4df2e;1ea94253f901d9c488f08437b3dccdd4;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;4.63;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;S_PBR;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;0;13;1
WireConnection;16;1;13;2
WireConnection;16;2;13;3
WireConnection;17;0;16;0
WireConnection;17;1;18;0
WireConnection;14;0;15;0
WireConnection;14;1;17;0
WireConnection;2;1;14;0
WireConnection;5;0;2;0
WireConnection;5;1;3;0
WireConnection;10;1;14;0
WireConnection;9;0;5;0
WireConnection;9;1;6;0
WireConnection;9;2;12;0
WireConnection;11;1;14;0
WireConnection;11;5;4;0
WireConnection;0;0;10;0
WireConnection;0;1;11;0
WireConnection;0;4;8;0
WireConnection;0;11;9;0
ASEEND*/
//CHKSM=FC2306368966721BA307AF1D4E329D5525AFA837