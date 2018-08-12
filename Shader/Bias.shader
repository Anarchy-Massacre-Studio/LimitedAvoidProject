Shader "GameUI/Bias"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ImageColor ("ImageColor", COLOR) = (1,1,1,1)
		_Color ("Color", COLOR) = (1,1,1,1)
		_Gradient("Gradient,x:slope,y:speed,zw:gradual",vector) = (-.1,5,.3,.01)  
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent"  
               "Queue" = "Transparent"  
            }  
		LOD 100

		Pass
		{
			Zwrite Off  
            Blend SrcAlpha OneMinusSrcAlpha  
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
				float2 local_uv:TEXCOORD1;  
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _ImageColor;
			fixed4 _Color;  
            float4 _Gradient;  
            float _Speed; 

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.local_uv = v.uv; 
				o.color = _ImageColor;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				_Speed += _Time.x*_Gradient.y;  
                fixed4 col = tex2D(_MainTex, i.uv);
                _Speed += _Gradient.x*i.local_uv.y;  
                float gradient = i.local_uv.x - (_Speed%1.5);  
                gradient = gradient > 0 ?   
                    max(_Gradient.w - gradient, 0) / _Gradient.w:  
                    max(_Gradient.z + gradient, 0) / _Gradient.z;  
                col.rgb = lerp(col.rgb,_Color.rgb, gradient);  
                return col * i.color;
			}
			ENDCG
		}
	}
}
