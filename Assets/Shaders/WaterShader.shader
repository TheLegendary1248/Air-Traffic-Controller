Shader "Unlit/WaterShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (0,0.5,1)
        _FoamColor("Color", Color) = (0.4,0.8,1)
        _Offset("Offset", Vector) = (0,0,0)
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
            float4 _MainTex_ST;
            half3 _Offset;
            float _Height;
            float _Cutoff;
            fixed _Slope;
            float4 _Color;
            float4 _FoamColor;
            float _Thresh;

            v2f vert (appdata v)
            {
                _Cutoff *= 0.01;
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float4 vert = v.vertex;
                float noise = abs(_Cutoff);
                float sloped = pow(noise, _Slope);

                vert.y += _Height * sloped;
                o.vertex = UnityObjectToClipPos(vert);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                _Thresh *= 0.001;
                _Cutoff *= 0.01;
                float3 spot = float3(i.uv.x , i.uv.y, 0) + _Offset;
                float noise = abs(ClassicNoise(spot));
                float movement = (sin(i.uv.x * 15 + _Time.y) * 0.01);
                bool val = noise < _Cutoff - _Thresh + movement;
                val = val && (0.2 * noise) < ((noise + _Cutoff + -_Thresh + movement + (_Time.x * 0.2)) % 0.04);
                fixed4 col = val ? _Color : lerp(_Color,_FoamColor,noise * 4);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
