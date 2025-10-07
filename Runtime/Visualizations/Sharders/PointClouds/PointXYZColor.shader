Shader "RmUniSim/PointCloud/XYZColor"
{
    Properties
    {
        [Toggle(_True)] _EnableX ("Enable X Gradient", Float) = 1
        [Toggle(_True)] _EnableY ("Enable Y Gradient", Float) = 1
        [Toggle(_True)] _EnableZ ("Enable Z Gradient", Float) = 1

        _XStartColor ("X Start Color", Color) = (0,0,0,1)
        _XEndColor   ("X End Color", Color) = (1,0,0,1)

        _YStartColor ("Y Start Color", Color) = (0,0,0,1)
        _YEndColor   ("Y End Color", Color) = (0,1,0,1)

        _ZStartColor ("Z Start Color", Color) = (0,0,0,1)
        _ZEndColor   ("Z End Color", Color) = (0,0,1,1)

        
        _PointSize ("Point Size", Float) = 0.1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            struct PointData {
                float3 position;
            };

            StructuredBuffer<PointData> _PointBuffer;

            float _EnableX, _EnableY, _EnableZ;

            float4 _XStartColor, _XEndColor;
            float4 _YStartColor, _YEndColor;
            float4 _ZStartColor, _ZEndColor;

            float _PointSize;

            struct v2g {
                float4 pos      : SV_POSITION;
                float3 world    : TEXCOORD0;
            };
            struct g2f {
                float4 pos          : SV_POSITION;
                float3 worldPos     : TEXCOORD0;
                float2 uv           : TEXCOORD1;
            };

            v2g vert(uint id : SV_VertexID)
            {
                v2g o;
                PointData p = _PointBuffer[id];
                o.pos       = mul(UNITY_MATRIX_VP,  float4(p.position,1)); 
                o.world     = float3(p.position);
                return o;
            }

            [maxvertexcount(36)] // 6 faces × 2 tris × 3 verts = 36
            void geom(point v2g input[1], inout TriangleStream<g2f> triStream)
            {
                float3 center = input[0].world;

                // 半边长
                float3 right = float3(_PointSize, 0, 0);
                float3 up    = float3(0, _PointSize, 0);
                float3 back  = float3(0, 0, _PointSize);

                // 8 个角点
                float3 p000 = center - right - up - back;
                float3 p001 = center - right - up + back;
                float3 p010 = center - right + up - back;
                float3 p011 = center - right + up + back;
                float3 p100 = center + right - up - back;
                float3 p101 = center + right - up + back;
                float3 p110 = center + right + up - back;
                float3 p111 = center + right + up + back;

                // 每个面用 2 个三角形
                int face[6][4] = {
                    {0,1,2,3}, // -X
                    {4,5,6,7}, // +X
                    {0,4,1,5}, // -Y
                    {2,3,6,7}, // +Y
                    {0,2,4,6}, // -Z
                    {1,5,3,7}  // +Z
                };

                float3 corners[8] = {p000,p001,p010,p011,p100,p101,p110,p111};
                float2 uv[4] = { float2(0,0), float2(1,0), float2(0,1), float2(1,1) };

                // 输出 6 个面
                for(int f=0; f<6; f++)
                {
                    g2f o;

                    // 三角形 1
                    o.pos = mul(UNITY_MATRIX_VP, float4(corners[face[f][0]],1)); o.uv = uv[0]; 
                    o.worldPos = center; triStream.Append(o);
                    o.pos = mul(UNITY_MATRIX_VP, float4(corners[face[f][1]],1)); o.uv = uv[1];
                    o.worldPos = center; triStream.Append(o);
                    o.pos = mul(UNITY_MATRIX_VP, float4(corners[face[f][2]],1)); o.uv = uv[2];
                    o.worldPos = center; triStream.Append(o);
                    triStream.RestartStrip();

                    // 三角形 2
                    o.pos = mul(UNITY_MATRIX_VP, float4(corners[face[f][2]],1)); o.uv = uv[2];
                    o.worldPos = center; triStream.Append(o);
                    o.pos = mul(UNITY_MATRIX_VP, float4(corners[face[f][1]],1)); o.uv = uv[1];
                    o.worldPos = center; triStream.Append(o);
                    o.pos = mul(UNITY_MATRIX_VP, float4(corners[face[f][3]],1)); o.uv = uv[3];
                    o.worldPos = center; triStream.Append(o);

                    
                    triStream.RestartStrip();
                }
            }
            
            fixed4 frag(g2f i) : SV_Target
            {
                float3 pos = i.worldPos;

                fixed4 color = fixed4(0,0,0,1);

                if (_EnableX > 0.5)
                {
                    float tx = saturate((pos.x + 1) * 0.5); // normalize from [-1,1] to [0,1]
                    color.rgb += lerp(_XStartColor.rgb, _XEndColor.rgb, tx);
                }

                if (_EnableY > 0.5)
                {
                    float ty = saturate((pos.y + 1) * 0.5);
                    color.rgb += lerp(_YStartColor.rgb, _YEndColor.rgb, ty);
                }

                if (_EnableZ > 0.5)
                {
                    float tz = saturate((pos.z + 1) * 0.5);
                    color.rgb += lerp(_ZStartColor.rgb, _ZEndColor.rgb, tz);
                }

                color.a = 1; // alpha 来自颜色本身
                
                float2 uv = i.uv * 2.0 - 1.0; 

                // 判断条件：任意一个维度的绝对值小于 0.02
                if (abs(uv.x) > 0.9 || abs(uv.y) > 0.9)
                {
                    return float4(1,1,1,1); // 白色
                }
                else
                {
                    return color;
                }
            }
            ENDCG
        }
    }
}
