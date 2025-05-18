Shader "Shader /VertexColor"
{
    Properties
    {
        _Gloss ("Gloss", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf StandardSpecular fullforwardshadows
        #pragma target 3.0

        struct Input
        {
            float4 color : COLOR;
        };

        half _Gloss;

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
            fixed4 c = IN.color;
            
            o.Albedo = c.rgb;
            o.Specular = float3(0,0,0);
            o.Smoothness = _Gloss;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
