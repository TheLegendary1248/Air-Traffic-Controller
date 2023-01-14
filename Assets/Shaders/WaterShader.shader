Shader "Unlit/WaterShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _DiffTex("Modif Texture", 2D) = "black" {}
        _Color("Color", Color) = (0,0.5,1)
        _DeepColor("Deep Color", Color) = (0,0.3,0.8)
        _FoamColor("Color", Color) = (0.4,0.8,1)
        _ZOffset("Z Offset", float) = 0
        _Height("Height", float) = 10
        _Slope("Slope",float) = 4
        _Cutoff("Cutoff", float) = 0.1
        _Thresh("Threshold", float) = 0.01
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
            };

            sampler2D _MainTex;
            sampler2D _DiffTex;
            float4 _MainTex_ST;
            float _ZOffset;
            float _Height;
            float _Cutoff;
            fixed _Slope;
            float4 _Color;
            float4 _DeepColor;
            float4 _FoamColor;
            float _Thresh;

            v2f vert (appdata v)
            {
                _Cutoff *= 0.01;
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.difuv = v.uv;

                float4 vert = v.vertex;
                //Height of the water
                float height = abs(_Cutoff);

                //Match overall height
                vert.y += _Height * height;
                o.vertex = UnityObjectToClipPos(vert);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                _Thresh *= 0.001;
                _Cutoff *= 0.01;
                float noise = abs(ClassicNoise(float3(i.uv.xy, _ZOffset)));
                noise = pow(noise, _Slope);
                fixed4 modif = tex2D(_DiffTex, i.difuv);
                noise = ModifyByTex(modif, noise);
                
                float movement = (sin(i.uv.x * 30 + _Time.y) * 0.00013);
                //Discard any pixels below terrain
                clip((noise < (_Cutoff + 0)) - 1);
                /*

                bool val = noise < _Cutoff - _Thresh + movement;
                val = val && (0.2 * noise) < ((noise + _Cutoff + -_Thresh + movement + (_Time.x * 0.2)) % 0.06);
                fixed4 col = val ? _Color : lerp(_Color,_FoamColor,noise * 4);*/
                bool val = noise + movement < _Cutoff  / 2.5;
                bool wave = ((noise + movement) + (_Time.x / 100)) % 0.001 > 0.0002;
                val = val && wave;
                _Color = lerp(_DeepColor,_Color,noise / _Cutoff);
                fixed4 col = val ? _Color : lerp(_Color,_FoamColor, noise / _Cutoff);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
