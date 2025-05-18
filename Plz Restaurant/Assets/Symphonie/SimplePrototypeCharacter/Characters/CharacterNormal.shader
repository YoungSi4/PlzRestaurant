Shader "Shader /CharacterNormal"
{
    Properties
    {
        _Color1 ("Color1", Color) = (0.5514706,0.3778077,0.004054935,1)
        _Gloss1 ("Gloss1", Float) = 0.3
        _Color2 ("Color2", Color) = (0.8455882,0.8455882,0.8455882,1)
        _Gloss2 ("Gloss2", Float) = 0.1
        _ColorSkin ("ColorSkin", Color) = (0.9191176,0.7891734,0.6893382,1)
        _GlossSkin ("GlossSkin", Float) = 0.1
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

        float _Gloss1;
        float _Gloss2;
        float _GlossSkin;
        float4 _Color1;
        float4 _Color2;
        float4 _ColorSkin;

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
            float3 normalizedColor = IN.color.rgb / dot(IN.color.rgb, float3(1,1,1));
            
            float3 colorMix = normalizedColor.r * _Color1.rgb + 
                             normalizedColor.g * _Color2.rgb + 
                             normalizedColor.b * _ColorSkin.rgb;
            
            o.Albedo = lerp(IN.color.rgb, colorMix, IN.color.a);
            
            float glossVal = lerp(_Gloss1, 
                                 dot(float3(float2(_Gloss1, _Gloss2), _GlossSkin), normalizedColor), 
                                 IN.color.a);
            
            o.Specular = float3(0,0,0);
            o.Smoothness = glossVal;
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
} 