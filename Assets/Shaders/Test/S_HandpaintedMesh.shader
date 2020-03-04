// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "HandpaintedMesh"
{
	Properties
	{
		_Displacement("Displacement", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,0)
		_Displace("Displace", Float) = 0.05
		_Shake("Shake", Float) = 0.25
		_Speed("Speed", Float) = 2
		_Normal("Normal", Float) = 0.1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			half filler;
		};

		uniform sampler2D _Displacement;
		uniform float _Speed;
		uniform sampler2D _Sampler013;
		uniform float4 _Displacement_ST;
		uniform float _Displace;
		uniform sampler2D _TextureSample0;
		uniform float _Shake;
		uniform float _Normal;
		uniform float4 _Color;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float mulTime5 = _Time.y * _Speed;
			float temp_output_9_0 = ( floor( mulTime5 ) * 0.1 );
			float4 appendResult22 = (float4(ase_vertex3Pos.x , ase_vertex3Pos.y , 0.0 , 0.0));
			float4 tex2DNode4 = tex2Dlod( _Displacement, float4( ( temp_output_9_0 + ( appendResult22 * float4( _Displacement_ST.xy, 0.0 , 0.0 ) ) ).xy, 0, 0.0) );
			float4 appendResult24 = (float4(( sin( ase_vertex3Pos.y ) * tex2DNode4.r * _Displace ) , 0.0 , 0.0 , 0.0));
			float2 temp_cast_2 = (temp_output_9_0).xx;
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( appendResult24 + ( tex2Dlod( _TextureSample0, float4( temp_cast_2, 0, 0.0) ).r * _Shake ) + float4( ( ( tex2DNode4.r - 0.5 ) * _Normal * ase_vertexNormal ) , 0.0 ) ).xyz;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Emission = _Color.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15900
-1871;16;1758;953;613.4794;257.3856;1.469195;True;True
Node;AmplifyShaderEditor.RangedFloatNode;25;-1537.829,248.7128;Float;False;Property;_Speed;Speed;5;0;Create;True;0;0;False;0;2;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;5;-1267.687,245.0031;Float;False;1;0;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;21;-1061.755,-185.167;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;22;-825.7555,-115.167;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureTransformNode;13;-1107.028,-7.571506;Float;False;4;1;0;SAMPLER2D;_Sampler013;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.FloorOpNode;6;-1044.087,261.903;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-639.7555,-38.16699;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-885.0254,256.4051;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-593.0871,177.703;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PosVertexDataNode;16;-40.81518,35.63254;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-428.4261,254.2876;Float;True;Property;_Displacement;Displacement;0;0;Create;True;0;0;False;0;cbe94f53fe50e83498d80e0fccd1d85e;8031679cb224dd346ab13f403adac6dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;15;175.2048,142.0995;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-42.92839,254.9876;Float;False;Property;_Displace;Displace;3;0;Create;True;0;0;False;0;0.05;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-393.9723,643.7932;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;cbe94f53fe50e83498d80e0fccd1d85e;8031679cb224dd346ab13f403adac6dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-177.7555,510.333;Float;False;Property;_Shake;Shake;4;0;Create;True;0;0;False;0;0.25;0.005;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;28;-6.558105,395.0391;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;164.4033,251.6525;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;26;216.4644,818.2474;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;31;273.9283,680.9788;Float;False;Property;_Normal;Normal;6;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-25.45959,495.8157;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;24;444.1234,225.0332;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;447.4232,649.2099;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0.2;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;3;-388.9866,-135.597;Float;False;Property;_Color;Color;2;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;17;632.6974,506.8039;Float;False;3;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;2;1313.465,126.2161;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;HandpaintedMesh;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;25;0
WireConnection;22;0;21;1
WireConnection;22;1;21;2
WireConnection;6;0;5;0
WireConnection;23;0;22;0
WireConnection;23;1;13;0
WireConnection;9;0;6;0
WireConnection;8;0;9;0
WireConnection;8;1;23;0
WireConnection;4;1;8;0
WireConnection;15;0;16;2
WireConnection;19;1;9;0
WireConnection;28;0;4;1
WireConnection;11;0;15;0
WireConnection;11;1;4;1
WireConnection;11;2;12;0
WireConnection;18;0;19;1
WireConnection;18;1;20;0
WireConnection;24;0;11;0
WireConnection;30;0;28;0
WireConnection;30;1;31;0
WireConnection;30;2;26;0
WireConnection;17;0;24;0
WireConnection;17;1;18;0
WireConnection;17;2;30;0
WireConnection;2;2;3;0
WireConnection;2;11;17;0
ASEEND*/
//CHKSM=24C76C81B7E01CFE0B4094A485056C83E0FBD725