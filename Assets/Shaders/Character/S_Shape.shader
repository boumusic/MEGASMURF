// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shape"
{
	Properties
	{
		_Distortion("Distortion", Float) = 0.2
		_ColorA("ColorA", Color) = (1,1,1,0)
		_ColorB("ColorB", Color) = (1,1,1,0)
		_Perlin("Perlin", 2D) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
			INTERNAL_DATA
		};

		uniform float4 _ColorA;
		uniform float4 _ColorB;
		uniform sampler2D _Perlin;
		uniform sampler2D _Sampler045;
		uniform float4 _Perlin_ST;
		uniform sampler2D _GrabTexture;
		uniform float _Distortion;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 panner46 = ( 1.0 * _Time.y * _Perlin_ST.zw + ( ase_screenPosNorm * float4( _Perlin_ST.xy, 0.0 , 0.0 ) ).xy);
			float4 tex2DNode40 = tex2D( _Perlin, panner46 );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 normalizeResult28 = normalize( ( ase_vertex3Pos + float3( 0,0.5,0 ) ) );
			float fresnelNdotV26 = dot( normalize( normalizeResult28 ), ase_worldViewDir );
			float fresnelNode26 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV26, 2.0 ) );
			float4 lerpResult33 = lerp( _ColorA , _ColorB , ( tex2DNode40 * saturate( fresnelNode26 ) ));
			float4 temp_cast_2 = (0.5).xxxx;
			float4 screenColor37 = tex2D( _GrabTexture, ( ase_screenPosNorm + ( _Distortion * ( tex2DNode40 - temp_cast_2 ) ) ).xy );
			o.Emission = ( ( lerpResult33 + screenColor37 ) * 1.0 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15900
-1920;75;1758;947;929.3386;332.7993;1.544912;True;True
Node;AmplifyShaderEditor.TextureTransformNode;45;-192.4617,819.1761;Float;False;40;1;0;SAMPLER2D;_Sampler045;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.ScreenPosInputsNode;38;-305.1475,435.4393;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;27;-624.4248,-96.68362;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-20.93059,530.8065;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-340.7636,-84.49514;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0.5,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;46;264.7471,525.3611;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;40;472.065,493.5859;Float;True;Property;_Perlin;Perlin;3;0;Create;True;0;0;False;0;None;8031679cb224dd346ab13f403adac6dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;28;-101.0168,66.71361;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;44;781.1007,699.8458;Float;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;26;137.0879,79.26428;Float;False;Standard;WorldNormal;ViewDir;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;41;885.6745,537.7435;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.5;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;43;1093.148,564.4291;Float;False;Property;_Distortion;Distortion;0;0;Create;True;0;0;False;0;0.2;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;35;392.5812,24.04315;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;1031.328,309.787;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;25;551.3674,-451.9629;Float;False;Property;_ColorA;ColorA;1;0;Create;True;0;0;False;0;1,1,1,0;0.2431369,0.5896502,0.972549,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;34;517.6943,-245.3181;Float;False;Property;_ColorB;ColorB;2;0;Create;True;0;0;False;0;1,1,1,0;0.2941174,0.4676295,0.9607843,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;39;1013.732,115.3023;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;773.9818,-126.2385;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;37;1168.283,147.6843;Float;False;Global;_GrabScreen0;Grab Screen 0;2;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;33;897.4498,-249.7339;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;1296.341,-204.1047;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;49;1318.25,-341.775;Float;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;2188.149,-253.3896;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2448.226,22.99526;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Shape;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.02;1,1,1,0;VertexScale;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;51;0;38;0
WireConnection;51;1;45;0
WireConnection;29;0;27;0
WireConnection;46;0;51;0
WireConnection;46;2;45;1
WireConnection;40;1;46;0
WireConnection;28;0;29;0
WireConnection;26;0;28;0
WireConnection;41;0;40;0
WireConnection;41;1;44;0
WireConnection;35;0;26;0
WireConnection;42;0;43;0
WireConnection;42;1;41;0
WireConnection;39;0;38;0
WireConnection;39;1;42;0
WireConnection;50;0;40;0
WireConnection;50;1;35;0
WireConnection;37;0;39;0
WireConnection;33;0;25;0
WireConnection;33;1;34;0
WireConnection;33;2;50;0
WireConnection;36;0;33;0
WireConnection;36;1;37;0
WireConnection;48;0;36;0
WireConnection;48;1;49;0
WireConnection;0;2;48;0
ASEEND*/
//CHKSM=9D0BE868647D57AA7C82DF64A4AFB4702FDA9083