Shader "Unlit/TerrainShader"
{
    Properties
    {
        _MainTex ("Color Palette", 2D) = "white" {}
        _DiffTex("Terrain Modif", 2D) = "" {}
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
                float noise = abs(ClassicNoise(float3(o.uv.xy, _ZOffset)));
                
                //Modif
                /*
                fixed4 modif = tex2Dlod(_DiffTex, float4(v.uv.xy,0,0));
                modif.x = (modif.x * 2) - 1;
                noise = lerp(noise + modif.x, modif.y, modif.z);
                */
                float sloped = pow(noise, _Slope);
                vert.y += _Height * sloped;


                o.vertex = UnityObjectToClipPos(vert);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                
                float3 spot = float3(i.uv.xy, _ZOffset);
                float val = lerp(0.5,1,ClassicNoise(spot));
                //FOR TERRAIN MODIF TEXTURE
                //Use red channel as terrain 'target' height
                //Use green channel as terrain lerp to 'target' height
                //Use blue channel as terrain add/minus, mapped to -1, 1
                //Use alpha channel as terrain add/minus effect multiplier
                //height = lerp(val, target)
                /*
                fixed4 modif = tex2D(_DiffTex, i.difuv);
                modif.x = (modif.x * 2) - 1;
                val = lerp(val, modif.r, modif.g) + (modif.b * modif.a);
                */
                fixed4 col = tex2D(_MainTex, val);
                col = lerp(0,col,i.uv.y % 0.2 > 0.1 ? 1 : 0.9);
                // apply fog
                // UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
