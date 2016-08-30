﻿Shader "Custom/UIMesh"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite OFF
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			sampler2D _NoiseTex;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// Calculate a "rough" (i.e. low-res) pixel position:
				float2 noisePixelsRate = _MainTex_TexelSize.zw*0.03;
				int2 noise_pixel = floor( i.uv * noisePixelsRate + 0.5 );
				float2 noise_continuous_pos = noise_pixel / noisePixelsRate;

				// Sample the texture at the calculated rough position:
				fixed n = tex2D( _MainTex, noise_continuous_pos ).r;
				n *= 0.5 + 0.5*sin(_Time[1] + i.uv.x*20);
				fixed4 noise = float4( n, n, 1.5*n, n*0.5 );

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				//col.rgb += noise.rgb*0.3;
				//col *= (0.7 + noise*0.3);
				col.rgb *= (1 + noise.rgb*5);

				float m = 1/0.02;
				float border = m*(1-i.uv.y) + 1 - m;		// y = m*x + b
				border = max( border, 0 );
				col += fixed4( 0.5*border, 0.7*border, border, 0.5*border*border );
				return col;
			}
			ENDCG
		}
	}
}
