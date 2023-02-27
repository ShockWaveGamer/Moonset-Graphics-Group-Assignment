Shader "Hidden/Fog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Fog Color", COLOR) = (1, 1, 1, 1)
		_Inensity ("Fog Inensity", Range(0, 10000)) = 1
	}
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Inensity;
            float4 _Color;
            uniform sampler2D _CameraDepthTexture; //Depth Texture
			const int Density;

            fixed4 frag (v2f i) : SV_Target
            {
			    fixed4 cameraColor = tex2D(_MainTex, i.uv);
                fixed4 fogColor = _Color;
				
				float depth = tex2D(_CameraDepthTexture, i.uv).r;
                
                return fogColor * (1 - (1 - exp(-_Inensity * depth))) + cameraColor * (1 - exp(-_Inensity * depth));
            }
            ENDCG  
        }
    }
}
