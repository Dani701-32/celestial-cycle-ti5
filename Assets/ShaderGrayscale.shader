Shader "Custom/ShaderGrayscale"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float4 _MainTex_ST; 

            fixed4 frag(v2f_img i): SV_Target{
                fixed4 col = tex2D(_MainTex, i.uv);
                col.xyz = dot(col.xyz, float3(0.233,0.715,0.072)); 
                return col;
            }
         
            ENDCG
        }
    }
}
