Shader "Custom/DecalShader"
{
    Properties 
    {
	    _MainTexColor("Main Texture Color", Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white" {}
        _MainTexRotation("Main Texture Rotation", Float) = 0
        
        [Toggle] _ShowDecal("Show Decal?", Float) = 0
        _DecalTexColor("Decal Texure Color", Color) = (1,1,1,1)
        _DecalTex("Decal Texture", 2D) = "white" {}
		_DecalTexRotation("Decal Texture Rotation", Float) = 0
    }
    SubShader 
    {
        Tags { "RenderType"="Opaque" "" = ""}
		
        Pass 
        {
            CGPROGRAM
			
            #pragma vertex vert
            #pragma fragment frag
			
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTexColor;
			float _MainTexRotation;
            float4 _MainTex_ST;
			
            sampler2D _DecalTex;
			float _DecalTexColor;
			float _DecalTexRotation;
			float4 _DecalTex_ST;
			
            float _ShowDecal;
			
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target 
            {
			    float x, y;

				//Get the Main Texture UV
                float2 _MainUVs = i.uv;
				x = _MainUVs.x *= _MainTex_ST.x;
				y = _MainUVs.y *= _MainTex_ST.y;

				_MainUVs += _MainTex_ST.wz;

                //Rotate the Texture UV
				x = _MainUVs.x * cos(_MainTexRotation) - _MainUVs.y * sin(_MainTexRotation);
				y = _MainUVs.x * sin(_MainTexRotation) + _MainUVs.y * cos(_MainTexRotation);
				_MainUVs.x = x;
				_MainUVs.y = y;

				//Get the Decal Texture UV
                float2 _DecalUVs = i.uv;
				x = _DecalUVs.x *= _DecalTex_ST.x;
				y = _DecalUVs.y *= _DecalTex_ST.y;

				_DecalUVs += _DecalTex_ST.wz;

                //Rotate the UV
				x = _DecalUVs.x * cos(_DecalTexRotation) - _DecalUVs.y * sin(_DecalTexRotation);
				y = _DecalUVs.x * sin(_DecalTexRotation) + _DecalUVs.y * cos(_DecalTexRotation);
				_DecalUVs.x = x;
				_DecalUVs.y = y;

			    fixed4 a = tex2D(_MainTex, _MainUVs);
                fixed4 b = tex2D(_DecalTex, _DecalUVs) * _ShowDecal;
				
                return half4 (b.a > 0.5 ? b.rgb * _DecalTexColor: a.rgb * _MainTexColor, 1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}


