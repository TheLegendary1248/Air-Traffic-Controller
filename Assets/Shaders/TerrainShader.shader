Shader "Unlit/TerrainShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Offset ("Offset", Vector) = (0,0,0)
        _Height ("Height", float) = 10
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise3D.hlsl"
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

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float4 vert = v.vertex;
                vert.y += 
                    _Height * 
                    ClassicNoise(
                        _Offset + float3(o.uv.x * _MainTex_TexelSize.z, o.uv.y * _MainTex_TexelSize.w ,0)
                    );
                o.vertex = UnityObjectToClipPos(vert);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                
                float3 spot = float3(i.uv.x * _MainTex_TexelSize.z, i.uv.y * _MainTex_TexelSize.w, 0) + _Offset;
                float val = lerp(0.5,1,ClassicNoise(spot));
                fixed4 col = tex2D(_MainTex, val);
                // apply fog
                // UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
