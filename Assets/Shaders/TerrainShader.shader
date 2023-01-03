Shader "Unlit/TerrainShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DiffTex("Texture", 2D) = "black" {}
        _Offset ("Offset", Vector) = (0,0,0)
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _MainTex_ST;
            half3 _Offset;
            float _Height;
            fixed _Slope;

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float4 vert = v.vertex;
                float noise = abs(ClassicNoise(_Offset + float3(o.uv.x, o.uv.y, 0)));
                float sloped = pow(noise, _Slope);

                vert.y += _Height * sloped;
                o.vertex = UnityObjectToClipPos(vert);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                
                float3 spot = float3(i.uv.x , i.uv.y, 0) + _Offset;
                float val = lerp(0.5,1,ClassicNoise(spot));
                
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
