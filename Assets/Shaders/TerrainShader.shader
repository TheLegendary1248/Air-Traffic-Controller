Shader "Unlit/TerrainShader"
{
    Properties
    {
        _MainTex ("Color Palette", 2D) = "white" {}
        _DiffTex("Terrain Modif", 2D) = "black" {}
        _ZOffset ("Z Offset", float) = 0
        _Height ("Height", float) = 10
        _Slope ("Slope",float) = 4
        _BaseColor ("Color", Color) = (0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Packages/jp.keijiro.noiseshader/Shader/ClassicNoise3D.hlsl"
            #include "Common.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 difuv : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float height : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _MainTex_ST;
            sampler2D _DiffTex;
            float _ZOffset;
            float _Height;
            fixed _Slope;

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.difuv = v.uv;
                
                //Change vert height to match color
                float4 vert = v.vertex;
                float noise = ClassicNoise(float3(o.uv.xy, _ZOffset));
                float sign = (noise > 0) * 2 - 1;
                noise = abs(noise);
                noise = pow(noise, _Slope);

                //Modify by texture
                fixed4 modif = tex2Dlod(_DiffTex, float4(v.uv.xy,0,0));
                noise = ModifyByTex(modif, noise);
                o.height = noise * sign;
                //Adjustable height
                vert.y += _Height * noise;

                o.vertex = UnityObjectToClipPos(vert);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.height + 0.5);
            //col = i.height * float4(1,-1,0,1);
                col = lerp(0,col,i.uv.y % 0.2 > 0.1 ? 1 : 0.9);
                // apply fog
                // UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
