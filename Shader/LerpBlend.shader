Shader "GameUI/LerpBlend"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MainColor ("MainColor", COLOR) = (1,1,1,1)
		_SecondTex ("SecondTex", 2D) = "white" {}
		_SecondColor ("SecondColor", COLOR) = (1,1,1,1)
		_Lerp ("Lerp", Range(0.0,1.0)) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
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
				float2 uv2 : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _MainColor;

			sampler2D _SecondTex;
			float4 _SecondTex_ST;
			fixed4 _SecondColor;
			
			float _Lerp;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv2, _SecondTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 mainTex = tex2D(_MainTex, i.uv) * _MainColor;
				fixed4 secondTex = tex2D(_SecondTex, i.uv2) * _SecondColor;

				fixed4 col = lerp(mainTex, secondTex, _Lerp);

				return col;
			}
			ENDCG
		}
	}
}
