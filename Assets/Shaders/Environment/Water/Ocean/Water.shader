// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Water/Water1"
{
	Properties
	{
		[HideInInspector] _VTInfoBlock( "VT( auto )", Vector ) = ( 0, 0, 0, 0 )
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 3.8
		_TessPhongStrength( "Phong Tess Strength", Range( 0, 1 ) ) = 1
		_WaterA("Water A", Color) = (0,0,0,0)
		_WaterB("Water B", Color) = (0,0,0,0)
		_Normals("Normals", 2D) = "white" {}
		_Texture0("Texture 0", 2D) = "white" {}
		_NormalTile1("NormalTile1", Float) = 0.4
		_NormalTile2("NormalTile2", Float) = 69.13
		_NormalStrength("NormalStrength", Float) = 1
		_Smoothness("Smoothness", Float) = 0.8
		_NormalSpeed1("NormalSpeed1", Float) = 0.1
		_NormalSpeed2("NormalSpeed2", Float) = 1
		_WaveTile("WaveTile", Float) = 0
		_WaveScale("WaveScale", Float) = 1
		_WaveSpeed("WaveSpeed", Float) = 1
		_WaveStrength("WaveStrength", Float) = 0
		_EdgeDistance("EdgeDistance", Float) = 1
		_EdgePower("Edge Power", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "Amplify" = "True"  "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "Tessellation.cginc"
		#pragma target 5.0
		#pragma surface surf Standard keepalpha noshadow exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction tessphong:_TessPhongStrength 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 screenPos;
		};

		uniform float _WaveTile;
		uniform float _WaveSpeed;
		uniform float _WaveScale;
		uniform float _WaveStrength;
		uniform sampler2D _Normals;
		uniform float _NormalSpeed1;
		uniform float _NormalTile1;
		uniform float _NormalStrength;
		uniform sampler2D _Texture0;
		uniform float _NormalSpeed2;
		uniform float _NormalTile2;
		uniform float4 _WaterA;
		uniform float4 _WaterB;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _EdgeDistance;
		uniform float _EdgePower;
		uniform float _Smoothness;
		uniform float _EdgeLength;
		uniform float _TessPhongStrength;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float4 ase_vertex4Pos = v.vertex;
			float3 ase_objectlightDir = normalize( ObjSpaceLightDir( ase_vertex4Pos ) );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult16 = (float2(ase_worldPos.x , ase_worldPos.z));
			float simplePerlin2D57 = snoise( (( ( float2( 1,0.3 ) * appendResult16 ) * _WaveTile )*1.0 + ( ( ( _Time.y * float2( 1,0 ) ) / 20.0 ) * _WaveSpeed ))*_WaveScale );
			simplePerlin2D57 = simplePerlin2D57*0.5 + 0.5;
			float3 appendResult116 = (float3(ase_objectlightDir.x , ( simplePerlin2D57 * _WaveStrength ) , ase_objectlightDir.z));
			v.vertex.xyz += appendResult116;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float2 appendResult16 = (float2(ase_worldPos.x , ase_worldPos.z));
			o.Normal = BlendNormals( UnpackScaleNormal( tex2D( _Normals, ( (appendResult16*1.0 + ( ( ( _Time.y * float2( -1,0 ) ) / 20.0 ) * _NormalSpeed1 )) * _NormalTile1 ) ), _NormalStrength ) , UnpackScaleNormal( tex2D( _Texture0, ( (appendResult16*1.0 + ( ( ( _Time.y * float2( 1,0 ) ) / 20.0 ) * _NormalSpeed2 )) * ( _NormalTile2 * 2.0 ) ) ), _NormalStrength ) );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV3 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode3 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV3, 5.0 ) );
			float4 lerpResult4 = lerp( _WaterA , _WaterB , fresnelNode3);
			float simplePerlin2D57 = snoise( (( ( float2( 1,0.3 ) * appendResult16 ) * _WaveTile )*1.0 + ( ( ( _Time.y * float2( 1,0 ) ) / 20.0 ) * _WaveSpeed ))*_WaveScale );
			simplePerlin2D57 = simplePerlin2D57*0.5 + 0.5;
			float clampResult102 = clamp( simplePerlin2D57 , 0.3 , 1.0 );
			o.Albedo = ( lerpResult4 * clampResult102 ).rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth157 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth157 = abs( ( screenDepth157 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _EdgeDistance ) );
			float clampResult162 = clamp( ( ( 1.0 - distanceDepth157 ) * _EdgePower ) , 0.0 , 1.0 );
			float3 temp_cast_3 = (clampResult162).xxx;
			o.Emission = temp_cast_3;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
292;73;958;655;2491.452;1490.174;4.023284;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;47;-2950.566,517.4254;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;46;-2950.748,618.1496;Inherit;False;Constant;_Vector1;Vector 1;7;0;Create;True;0;0;False;0;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;39;-2910.929,226.7036;Inherit;False;Constant;_Vector0;Vector 0;7;0;Create;True;0;0;False;0;-1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;34;-2910.747,125.9794;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-2724.509,159.2632;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-2738.502,270.5604;Inherit;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;False;0;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;13;-3659.082,166.0757;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;48;-2778.321,662.0063;Inherit;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;False;0;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;65;-3047.428,-236.0849;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;66;-3047.61,-135.3607;Inherit;False;Constant;_Vector3;Vector 3;7;0;Create;True;0;0;False;0;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-2764.328,550.7091;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;41;-2591.594,161.105;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-2875.183,-91.50391;Inherit;False;Constant;_Float2;Float 2;7;0;Create;True;0;0;False;0;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;16;-3382.742,194.7064;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-2861.19,-202.8011;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;73;-3244.066,-513.0283;Inherit;False;Constant;_WaveVector2;WaveVector2;14;0;Create;True;0;0;False;0;1,0.3;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;51;-2625.064,673.9077;Inherit;False;Property;_NormalSpeed2;NormalSpeed2;15;0;Create;True;0;0;False;0;1;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-2585.245,282.4617;Inherit;False;Property;_NormalSpeed1;NormalSpeed1;14;0;Create;True;0;0;False;0;0.1;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;50;-2631.413,552.5509;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;69;-2706.275,-202.9593;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-2457.795,551.984;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-2712.341,-69.1687;Inherit;False;Property;_WaveSpeed;WaveSpeed;18;0;Create;True;0;0;False;0;1;25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-2386.787,671.3022;Inherit;True;Property;_NormalTile2;NormalTile2;11;0;Create;True;0;0;False;0;69.13;0.009;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-2991.449,-557.7505;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-2798.06,-479.5171;Inherit;False;Property;_WaveTile;WaveTile;16;0;Create;True;0;0;False;0;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-2417.976,160.5381;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;156;-1041.433,714.0521;Inherit;False;Property;_EdgeDistance;EdgeDistance;20;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;32;-2239.161,-9.325803;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-2219.019,542.2085;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-2554.657,-201.5262;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;53;-2278.98,400.2034;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-2654.771,-555.2121;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-2155.87,144.7729;Inherit;True;Property;_NormalTile1;NormalTile1;10;0;Create;True;0;0;False;0;0.4;0.21;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1978.721,67.26742;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VirtualTextureObject;28;-1828.873,243.3599;Inherit;True;Property;_Texture0;Texture 0;9;0;Create;True;0;0;False;0;-1;None;7060a5f88c6a25346b9df09489ac6aec;True;white;Auto;Unity5;0;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-2018.541,458.7134;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;71;-2368.612,-303.1326;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VirtualTextureObject;11;-1830.734,-92.74127;Inherit;True;Property;_Normals;Normals;8;0;Create;True;0;0;False;0;-1;None;7060a5f88c6a25346b9df09489ac6aec;True;white;Auto;Unity5;0;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.DepthFade;157;-831.5369,702.2625;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-2414.99,-481.3048;Inherit;False;Property;_WaveScale;WaveScale;17;0;Create;True;0;0;False;0;1;0.06;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1471.197,206.303;Inherit;False;Property;_NormalStrength;NormalStrength;12;0;Create;True;0;0;False;0;1;0.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;29;-1559.281,348.3726;Inherit;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;158;-557.9463,713.7135;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;57;-2078.713,-546.3732;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;160;-575.8001,831.5205;Inherit;False;Property;_EdgePower;Edge Power;21;0;Create;True;0;0;False;0;1;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-595.4265,-883.7619;Inherit;False;Property;_WaterA;Water A;6;0;Create;True;0;0;False;0;0,0,0,0;0.01268245,0.1584248,0.1792451,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-1567.593,10.89783;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;115;-1655.824,-562.5732;Inherit;False;Property;_WaveStrength;WaveStrength;19;0;Create;True;0;0;False;0;0;2.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-598.3748,-708.221;Inherit;False;Property;_WaterB;Water B;7;0;Create;True;0;0;False;0;0,0,0,0;0,0.3887503,0.4528299,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;3;-613.4758,-532.224;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;-1318.597,-516.0939;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.UnpackScaleNormalNode;26;-1205.423,16.52876;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ClampOpNode;102;-1174.29,-422.1291;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.3;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;4;-280.1545,-835.3849;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ObjSpaceLightDirHlpNode;126;-1396.304,-802.8118;Inherit;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.UnpackScaleNormalNode;30;-1209.998,303.998;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;159;-348.9463,713.7135;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-366.4537,185.0204;Inherit;False;Property;_Smoothness;Smoothness;13;0;Create;True;0;0;False;0;0.8;1.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;162;-197.3925,440.6298;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-325.4834,-273.1719;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;116;-891.4926,-467.447;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;31;-942.0073,16.68819;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3,-1;Float;False;True;-1;7;ASEMaterialInspector;0;0;Standard;Water/Water1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;1;Custom;0.5;True;False;0;True;Transparent;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;3.8;10;25;True;1;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;1;0.6981132,0.3589356,0.3589356,0;VertexScale;True;False;Cylindrical;False;Relative;0;;5;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;40;0;34;0
WireConnection;40;1;39;0
WireConnection;49;0;47;0
WireConnection;49;1;46;0
WireConnection;41;0;40;0
WireConnection;41;1;42;0
WireConnection;16;0;13;1
WireConnection;16;1;13;3
WireConnection;67;0;65;0
WireConnection;67;1;66;0
WireConnection;50;0;49;0
WireConnection;50;1;48;0
WireConnection;69;0;67;0
WireConnection;69;1;68;0
WireConnection;52;0;50;0
WireConnection;52;1;51;0
WireConnection;59;0;73;0
WireConnection;59;1;16;0
WireConnection;43;0;41;0
WireConnection;43;1;44;0
WireConnection;32;0;16;0
WireConnection;32;2;43;0
WireConnection;56;0;55;0
WireConnection;70;0;69;0
WireConnection;70;1;72;0
WireConnection;53;0;16;0
WireConnection;53;2;52;0
WireConnection;62;0;59;0
WireConnection;62;1;63;0
WireConnection;17;0;32;0
WireConnection;17;1;18;0
WireConnection;54;0;53;0
WireConnection;54;1;56;0
WireConnection;71;0;62;0
WireConnection;71;2;70;0
WireConnection;157;0;156;0
WireConnection;29;0;28;0
WireConnection;29;1;54;0
WireConnection;158;0;157;0
WireConnection;57;0;71;0
WireConnection;57;1;64;0
WireConnection;5;0;11;0
WireConnection;5;1;17;0
WireConnection;114;0;57;0
WireConnection;114;1;115;0
WireConnection;26;0;5;0
WireConnection;26;1;23;0
WireConnection;102;0;57;0
WireConnection;4;0;1;0
WireConnection;4;1;2;0
WireConnection;4;2;3;0
WireConnection;30;0;29;0
WireConnection;30;1;23;0
WireConnection;159;0;158;0
WireConnection;159;1;160;0
WireConnection;162;0;159;0
WireConnection;82;0;4;0
WireConnection;82;1;102;0
WireConnection;116;0;126;1
WireConnection;116;1;114;0
WireConnection;116;2;126;3
WireConnection;31;0;26;0
WireConnection;31;1;30;0
WireConnection;0;0;82;0
WireConnection;0;1;31;0
WireConnection;0;2;162;0
WireConnection;0;4;27;0
WireConnection;0;11;116;0
ASEEND*/
//CHKSM=9999B126245941844D04B8F5F1CC2CFA0029764D