Shader "Examples/2 Alpha Blended Textures" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _SpecColor ("Spec Color", Color) = (1,1,1,1)
        _Emission ("Emmisive Color", Color) = (0,0,0,0)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.7
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BlendTex ("Alpha Blended (RGBA) ", 2D) = "white" {}
        _SpecMap ("Specular map", 2D) = "black" {}
    }
    SubShader 
    {
    	Tags {"RenderType"="Opaque"}
        Pass 
        {
        
        	Material {
                Diffuse [_Color]
                Ambient [_Color]
                Shininess [_Shininess]
                Specular [_SpecColor]                
            }
            Lighting On
            SeparateSpecular On
        
            // Apply base texture
            SetTexture [_MainTex] {
                combine Primary * texture
            }
            // Blend in the alpha texture using the lerp operator
            SetTexture [_BlendTex] {
                combine texture lerp (texture) previous
            }          
	            
		}
        
    }
} 
