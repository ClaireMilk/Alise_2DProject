Shader "Unlit/PlayTextGlow"
{
    Properties
    {
    [Header(Light)]
        [HDR] _Color("NormalColor", Color) = (1, 1, 1, 1)
        [HDR]_LightCol("GlowColor",Color) = (1,1,1,1)
        _LightRange("Intensity",Range(0,3)) = 1
        [HideInInspector]_LightIntensity("LightIntensity",Float) = 1
        _LightSpeed("LightVelocity",Range(0,10)) = 1
        _LightMask("LightMask",2D) = "white"{}
        [PerRendererData] _MainTex("Texture", 2D) = "white" {}
        [HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
        [HideInInspector]_Stencil("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255
    }
    SubShader
    {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }
            Stencil
            {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }
            Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask[_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ UNITY_UI_ALPHACLIP

            #include "UnityUI.cginc"
            #include "UnityCG.cginc"

            fixed4 _TextureSampleAdd; // Unity????????Alpha8 
            float4 _ClipRect;// Unity???2D????
            sampler2D _MainTex;
            sampler2D _LightMask;


            struct a2v 
            {
                float4 vertex       : POSITION;
                float4 color        : COLOR;
                float2 texcoord     : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex       : SV_POSITION;
                float4 color        : COLOR;
                float2 texcoord     : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            v2f vert(a2v IN) {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);// ?????
                OUT.vertex = UnityObjectToClipPos(IN.vertex);// ?????????
                OUT.color = IN.color;
                OUT.texcoord = IN.texcoord;
                return OUT;
            }
            fixed4 frag(v2f IN) :SV_Target
            {
                // ???????? ???? ImageColor
            half4 color = tex2D(_MainTex,IN.texcoord) * IN.color;
            //fixed var_LightMask = tex2D(_LightMask, IN.uv0);

            return color;
            }


            ENDCG
        }
    }
}
