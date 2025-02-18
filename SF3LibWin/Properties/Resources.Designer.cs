﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SF3.Win.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SF3.Win.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap CameraFromFrontBmp {
            get {
                object obj = ResourceManager.GetObject("CameraFromFrontBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap CameraFromTopBmp {
            get {
                object obj = ResourceManager.GetObject("CameraFromTopBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap CameraPointToCenterBmp {
            get {
                object obj = ResourceManager.GetObject("CameraPointToCenterBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap EventIDsBmp {
            get {
                object obj = ResourceManager.GetObject("EventIDsBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap IconHelpBmp {
            get {
                object obj = ResourceManager.GetObject("IconHelpBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap IconWireframeBmp {
            get {
                object obj = ResourceManager.GetObject("IconWireframeBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap LightingNewBmp {
            get {
                object obj = ResourceManager.GetObject("LightingNewBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap LightingOldBmp {
            get {
                object obj = ResourceManager.GetObject("LightingOldBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap NormalsBmp {
            get {
                object obj = ResourceManager.GetObject("NormalsBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///in vec3 normalFrag;
        ///
        ///out vec4 FragColor;
        ///
        ///void main() {
        ///    FragColor = vec4(normalFrag, 1.0);
        ///}
        ///.
        /// </summary>
        internal static string NormalsFrag {
            get {
                return ResourceManager.GetString("NormalsFrag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///layout (location = 0) in vec3 position;
        ///layout (location = 1) in vec3 normal;
        ///
        ///uniform mat4 model;
        ///uniform mat4 view;
        ///uniform mat4 projection;
        ///uniform mat3 normalMatrix;
        ///
        ///out vec3 normalFrag;
        ///
        ///void main() {
        ///    gl_Position = projection * view * model * vec4(position, 1.0);
        ///
        ///    float prevLength = length(normal);
        ///    vec3 modelNormal = normalize(normalMatrix * normal) * prevLength;
        ///
        ///    normalFrag = modelNormal * vec3(1.0, 0.0, 1.0) + 0.5;
        ///}
        ///.
        /// </summary>
        internal static string NormalsVert {
            get {
                return ResourceManager.GetString("NormalsVert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///uniform sampler2D textureAtlas;
        ///uniform sampler2D textureTerrainTypes;
        ///uniform sampler2D textureEventIDs;
        ///
        ///in vec4 colorFrag;
        ///in vec3 glowFrag;
        ///in vec4 lightColorFrag;
        ///
        ///in vec2 texCoordAtlasFrag;
        ///in vec2 texCoordTerrainTypesFrag;
        ///in vec2 texCoordEventIDsFrag;
        ///
        ///out vec4 FragColor;
        ///
        ///void main() {
        ///    vec4 surfaceTex = (texture(textureAtlas, texCoordAtlasFrag) + lightColorFrag);
        ///    surfaceTex = surfaceTex * colorFrag + vec4(glowFrag, 0.0);
        ///
        ///    vec4 overlayTex =
        ///        [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ObjectFrag {
            get {
                return ResourceManager.GetString("ObjectFrag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///uniform mat4 model;
        ///uniform mat4 view;
        ///uniform mat4 projection;
        ///uniform mat3 normalMatrix;
        ///uniform vec3 lightPosition;
        ///uniform sampler2D textureLighting;
        ///uniform bool useNewLighting;
        ///
        ///layout (location = 0) in vec3 position;
        ///layout (location = 1) in vec4 color;
        ///layout (location = 2) in vec3 glow;
        ///layout (location = 3) in vec3 normal;
        ///
        ///layout (location = 4) in vec2 texCoordAtlas;
        ///layout (location = 5) in vec2 texCoordTerrainTypes;
        ///layout (location = 6) in vec2 texCoordEvent [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ObjectVert {
            get {
                return ResourceManager.GetString("ObjectVert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ShowCameraBoundaries {
            get {
                object obj = ResourceManager.GetObject("ShowCameraBoundaries", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ShowEventIDs {
            get {
                object obj = ResourceManager.GetObject("ShowEventIDs", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ShowTerrainTypes {
            get {
                object obj = ResourceManager.GetObject("ShowTerrainTypes", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///in vec4 colorFrag;
        ///out vec4 FragColor;
        ///
        ///void main() {
        ///    FragColor = colorFrag;
        ///}
        ///.
        /// </summary>
        internal static string SolidFrag {
            get {
                return ResourceManager.GetString("SolidFrag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///layout (location = 0) in vec3 position;
        ///layout (location = 1) in vec4 color;
        ///
        ///uniform mat4 model;
        ///uniform mat4 view;
        ///uniform mat4 projection;
        ///
        ///out vec4 colorFrag;
        ///
        ///void main() {
        ///    gl_Position = projection * view * model * vec4(position, 1.0);
        ///    colorFrag = color;
        ///}
        ///.
        /// </summary>
        internal static string SolidVert {
            get {
                return ResourceManager.GetString("SolidVert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap SpritesPointingUpBmp {
            get {
                object obj = ResourceManager.GetObject("SpritesPointingUpBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap TerrainTypesBmp {
            get {
                object obj = ResourceManager.GetObject("TerrainTypesBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///uniform sampler2D texture0;
        ///
        ///in vec4 colorFrag;
        ///in vec2 texCoord0Frag;
        ///in vec3 glowFrag;
        ///
        ///out vec4 FragColor;
        ///
        ///void main() {
        ///    vec4 texColor = texture(texture0, texCoord0Frag) * colorFrag + vec4(glowFrag, 0.0);
        ///    if (texColor.a &lt; 0.001)
        ///        discard;
        ///
        ///    FragColor = texColor;
        ///}
        ///.
        /// </summary>
        internal static string TextureFrag {
            get {
                return ResourceManager.GetString("TextureFrag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///layout (location = 0) in vec3 position;
        ///layout (location = 1) in vec4 color;
        ///layout (location = 2) in vec2 texCoord0;
        ///layout (location = 3) in vec3 glow;
        ///
        ///uniform mat4 model;
        ///uniform mat4 view;
        ///uniform mat4 projection;
        ///
        ///out vec4 colorFrag;
        ///out vec2 texCoord0Frag;
        ///out vec3 glowFrag;
        ///
        ///void main() {
        ///    gl_Position = projection * view * model * vec4(position, 1.0);
        ///    colorFrag = color;
        ///    texCoord0Frag = texCoord0;
        ///    glowFrag = glow;
        ///}
        ///.
        /// </summary>
        internal static string TextureVert {
            get {
                return ResourceManager.GetString("TextureVert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap TileHoverBmp {
            get {
                object obj = ResourceManager.GetObject("TileHoverBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap TileSelectedBmp {
            get {
                object obj = ResourceManager.GetObject("TileSelectedBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap TileWireframeBmp {
            get {
                object obj = ResourceManager.GetObject("TileWireframeBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap TransparentBlackBmp {
            get {
                object obj = ResourceManager.GetObject("TransparentBlackBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap TransparentWhiteBmp {
            get {
                object obj = ResourceManager.GetObject("TransparentWhiteBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///uniform sampler2D texture0;
        ///uniform sampler2D texture1;
        ///
        ///in vec4 colorFrag;
        ///in vec2 texCoord0Frag;
        ///in vec2 texCoord1Frag;
        ///
        ///out vec4 FragColor;
        ///
        ///void main() {
        ///    vec4 tex0Color = texture(texture0, texCoord0Frag);
        ///    vec4 tex1Color = texture(texture1, texCoord1Frag);
        ///
        ///    vec4 compositeColor = tex0Color * colorFrag * (1.0 - tex1Color.a) + vec4(tex1Color.rgb * tex1Color.a, tex1Color.a);
        ///    if (compositeColor.a &lt; 0.001)
        ///        discard;
        ///
        ///    FragColor = compositeColor;
        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string TwoTextureFrag {
            get {
                return ResourceManager.GetString("TwoTextureFrag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///layout (location = 0) in vec3 position;
        ///layout (location = 1) in vec4 color;
        ///layout (location = 2) in vec2 texCoord0;
        ///layout (location = 3) in vec2 texCoord1;
        ///
        ///uniform mat4 model;
        ///uniform mat4 view;
        ///uniform mat4 projection;
        ///
        ///out vec4 colorFrag;
        ///out vec2 texCoord0Frag;
        ///out vec2 texCoord1Frag;
        ///
        ///void main() {
        ///    gl_Position = projection * view * model * vec4(position, 1.0);
        ///    colorFrag = color;
        ///    texCoord0Frag = texCoord0;
        ///    texCoord1Frag = texCoord1;
        ///}
        ///.
        /// </summary>
        internal static string TwoTextureVert {
            get {
                return ResourceManager.GetString("TwoTextureVert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ViewerHelpBmp {
            get {
                object obj = ResourceManager.GetObject("ViewerHelpBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap WhiteBmp {
            get {
                object obj = ResourceManager.GetObject("WhiteBmp", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///uniform sampler2D texture1;
        ///
        ///in vec2 texCoord1Frag;
        ///
        ///out vec4 FragColor;
        ///
        ///void main() {
        ///    vec4 texColor = texture(texture1, texCoord1Frag);
        ///    if (texColor.a &lt; 0.001)
        ///        discard;
        ///
        ///    FragColor = texColor;
        ///}
        ///.
        /// </summary>
        internal static string WireframeFrag {
            get {
                return ResourceManager.GetString("WireframeFrag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///layout (location = 0) in vec3 position;
        ///layout (location = 1) in vec2 texCoord1;
        ///
        ///uniform mat4 model;
        ///uniform mat4 view;
        ///uniform mat4 projection;
        ///
        ///out vec2 texCoord1Frag;
        ///
        ///void main() {
        ///    gl_Position = projection * view * model * vec4(position, 1.0);
        ///    texCoord1Frag = texCoord1;
        ///}
        ///.
        /// </summary>
        internal static string WireframeVert {
            get {
                return ResourceManager.GetString("WireframeVert", resourceCulture);
            }
        }
    }
}
