Shader"TestShaders/TestShader_1"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,0.5,0.5,1)
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM

            // инструкции для компилятора
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // структура необходимая для обработки вершин
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            // структура, которая помогает преобразовать данные вершины в данные фрагмента
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            //здесь происходит обработка вершин
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            //здесь происходит обработка пикселей, цвет пикселей умножается на цвет материала
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col = col * _Color;
                return col;
            }

            ENDCG
        }

    }
}
