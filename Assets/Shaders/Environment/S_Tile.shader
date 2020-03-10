// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Tile"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 50
		_TessPhongStrength( "Phong Tess Strength", Range( 0, 1 ) ) = 0.5
		_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset][Normal]_Normal("Normal", 2D) = "bump" {}
		[NoScaleOffset]_Height("Height", 2D) = "white" {}
		_Sub("Sub", Float) = 0
		_ThresholdUV("ThresholdUV", Float) = 0
		_Displacement("Displacement", Float) = 1
		_Smoothness("Smoothness", Float) = 0
		_NormalStrenth("NormalStrenth", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction tessphong:_TessPhongStrength 
		struct Input
		{
			float3 worldPos;
		};

		uniform sampler2D _Height;
		uniform sampler2D _Sampler06;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float _ThresholdUV;
		uniform float _Sub;
		uniform float _Displacement;
		uniform float _NormalStrenth;
		uniform sampler2D _Normal;
		uniform float _Smoothness;
		uniform float _EdgeLength;
		uniform float _TessPhongStrength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float4 appendResult4 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 appendResult22 = (float4(( ase_worldPos.x + ase_worldPos.z ) , ase_worldPos.y , 0.0 , 0.0));
			float3 ase_vertex3Pos = v.vertex.xyz;
			float4 lerpResult19 = lerp( ( ( appendResult4 * float4( _Albedo_ST.xy, 0.0 , 0.0 ) ) + float4( _Albedo_ST.zw, 0.0 , 0.0 ) ) , ( ( appendResult22 * float4( _Albedo_ST.xy, 0.0 , 0.0 ) ) + float4( _Albedo_ST.zw, 0.0 , 0.0 ) ) , step( ase_vertex3Pos.y , _ThresholdUV ));
			float4 UV17 = lerpResult19;
			float4 temp_cast_5 = (_Sub).xxxx;
			v.vertex.xyz += ( ( tex2Dlod( _Height, float4( UV17.xy, 0, 0.0) ) - temp_cast_5 ) * _Displacement * float4( float3(0,1,0) , 0.0 ) ).rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float4 appendResult4 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 appendResult22 = (float4(( ase_worldPos.x + ase_worldPos.z ) , ase_worldPos.y , 0.0 , 0.0));
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 lerpResult19 = lerp( ( ( appendResult4 * float4( _Albedo_ST.xy, 0.0 , 0.0 ) ) + float4( _Albedo_ST.zw, 0.0 , 0.0 ) ) , ( ( appendResult22 * float4( _Albedo_ST.xy, 0.0 , 0.0 ) ) + float4( _Albedo_ST.zw, 0.0 , 0.0 ) ) , step( ase_vertex3Pos.y , _ThresholdUV ));
			float4 UV17 = lerpResult19;
			o.Normal = UnpackScaleNormal( tex2D( _Normal, UV17.xy ), _NormalStrenth );
			o.Albedo = tex2D( _Albedo, UV17.xy ).rgb;
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
-1920;75;1758;947;1590.799;449.1222;1.440144;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;20;-3323.457,-86.05832;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;3;-2849.863,-412.6368;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-2985.242,74.87779;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;22;-2750.655,69.94167;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;4;-2583.862,-373.6368;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureTransformNode;6;-2674.163,-102.2368;Float;False;1;1;0;SAMPLER2D;_Sampler06;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.PosVertexDataNode;25;-2404.144,233.4775;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;-2115.542,375.178;Float;False;Property;_ThresholdUV;ThresholdUV;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-2461.455,75.14174;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-2364.862,-373.6368;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-2249.693,65.02195;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-2106.3,-304.4565;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StepOpNode;26;-1982.942,238.6776;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;19;-1842.542,-61.62244;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;17;-1667.487,-63.17706;Float;False;UV;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;-944.1613,-1.720413;Float;False;17;UV;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;8;-677.39,252.7737;Float;True;Property;_Height;Height;7;1;[NoScaleOffset];Create;True;0;0;False;0;cbe94f53fe50e83498d80e0fccd1d85e;20b4d0cd2efcabd4eb16a50165d86e08;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-440.1016,200.2899;Float;False;Property;_Sub;Sub;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-938.4312,171.7183;Float;False;Property;_NormalStrenth;NormalStrenth;12;0;Create;True;0;0;False;0;1;5.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;15;-353.1016,285.2899;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.5;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-590.8103,544.3961;Float;False;Property;_Displacement;Displacement;10;0;Create;True;0;0;False;0;1;0.65;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;11;-345.8103,513.3961;Float;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;12;-178.3647,206.1318;Float;False;Property;_Smoothness;Smoothness;11;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-212.8103,294.3961;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-673,-201.5;Float;True;Property;_Albedo;Albedo;5;0;Create;True;0;0;False;0;None;8921fb670b68070458efd8c31abec79b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-671,25.5;Float;True;Property;_Normal;Normal;6;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;826f80ee0ad07444c8558af826a4df2e;1ea94253f901d9c488f08437b3dccdd4;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;4.63;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;212.3,5.800003;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Tile;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;50;10;25;True;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;27;0;20;1
WireConnection;27;1;20;3
WireConnection;22;0;27;0
WireConnection;22;1;20;2
WireConnection;4;0;3;1
WireConnection;4;1;3;3
WireConnection;23;0;22;0
WireConnection;23;1;6;0
WireConnection;5;0;4;0
WireConnection;5;1;6;0
WireConnection;24;0;23;0
WireConnection;24;1;6;1
WireConnection;7;0;5;0
WireConnection;7;1;6;1
WireConnection;26;0;25;2
WireConnection;26;1;29;0
WireConnection;19;0;7;0
WireConnection;19;1;24;0
WireConnection;19;2;26;0
WireConnection;17;0;19;0
WireConnection;8;1;18;0
WireConnection;15;0;8;0
WireConnection;15;1;16;0
WireConnection;9;0;15;0
WireConnection;9;1;10;0
WireConnection;9;2;11;0
WireConnection;1;1;18;0
WireConnection;2;1;18;0
WireConnection;2;5;14;0
WireConnection;0;0;1;0
WireConnection;0;1;2;0
WireConnection;0;4;12;0
WireConnection;0;11;9;0
ASEEND*/
//CHKSM=BF7BF048173408042970232C0F6BF58EE1C9B8A1