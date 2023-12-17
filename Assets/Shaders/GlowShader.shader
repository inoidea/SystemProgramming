Shader"TestShaders/GlowShader" 
{
    Properties 
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.01
    }
 
    SubShader 
    {
        Tags {"Queue"="Transparent" "RenderType"="Opaque"}
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
            float4 _OutlineColor;
            float _OutlineWidth;
 
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
 
                // Calculate the outline color
                fixed4 outlineColor = 0;
                float width = _OutlineWidth;
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        if (j != 0 || k != 0)
                        {
                            outlineColor += tex2D(_MainTex, i.uv + float2(j, k) * width) - col;
                        }
                    }
                }
                outlineColor *= _OutlineColor;
 
                // Add the outline color to the main color
                col += outlineColor;
 
                return col;
            }
            ENDCG
        }
    }
}