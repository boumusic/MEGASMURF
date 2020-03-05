// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Tile"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset][Normal]_Normal("Normal", 2D) = "white" {}
		[NoScaleOffset]_Height("Height", 2D) = "white" {}
		_Displacement("Displacement", Float) = 1
		_Smoothness("Smoothness", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
		};

		uniform sampler2D _Height;
		uniform sampler2D _Sampler06;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float _Displacement;
		uniform sampler2D _Normal;
		uniform float _Smoothness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float4 appendResult4 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 temp_output_7_0 = ( ( appendResult4 * float4( _Albedo_ST.xy, 0.0 , 0.0 ) ) + float4( _Albedo_ST.zw, 0.0 , 0.0 ) );
			v.vertex.xyz += ( tex2Dlod( _Height, float4( temp_output_7_0.xy, 0, 0.0) ) * _Displacement * float4( float3(0,1,0) , 0.0 ) ).rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float4 appendResult4 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 temp_output_7_0 = ( ( appendResult4 * float4( _Albedo_ST.xy, 0.0 , 0.0 ) ) + float4( _Albedo_ST.zw, 0.0 , 0.0 ) );
			o.Normal = tex2D( _Normal, temp_output_7_0.xy ).rgb;
			o.Albedo = tex2D( _Albedo, temp_output_7_0.xy ).rgb;
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
-1871;16;1758;953;1696.071;150.5923;1;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;3;-1583,-152.5;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;4;-1291,-94.5;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureTransformNode;6;-1338,-360.5;Float;False;1;1;0;SAMPLER2D;_Sampler06;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1118,-132.5;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-859.4377,-63.31967;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector3Node;11;-381.8103,475.3961;Float;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;10;-590.8103,544.3961;Float;False;Property;_Displacement;Displacement;3;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-752.39,324.7737;Float;True;Property;_Height;Height;2;1;[NoScaleOffset];Create;True;0;0;False;0;cbe94f53fe50e83498d80e0fccd1d85e;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-673,-201.5;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;0000000000000000f000000000000000;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-678,33.5;Float;True;Property;_Normal;Normal;1;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;826f80ee0ad07444c8558af826a4df2e;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-212.8103,294.3961;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-228.3647,139.1318;Float;False;Property;_Smoothness;Smoothness;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;40.3,-5.2;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Tile;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;3;1
WireConnection;4;1;3;3
WireConnection;5;0;4;0
WireConnection;5;1;6;0
WireConnection;7;0;5;0
WireConnection;7;1;6;1
WireConnection;8;1;7;0
WireConnection;1;1;7;0
WireConnection;2;1;7;0
WireConnection;9;0;8;0
WireConnection;9;1;10;0
WireConnection;9;2;11;0
WireConnection;0;0;1;0
WireConnection;0;1;2;0
WireConnection;0;4;12;0
WireConnection;0;11;9;0
ASEEND*/
//CHKSM=0B873FD92B4A562C659237E416A393CDD298E178