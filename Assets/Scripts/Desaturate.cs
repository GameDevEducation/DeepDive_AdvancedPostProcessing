using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace DesaturateEffect
{
    [System.Serializable]
    [PostProcess(typeof(DesaturateRenderer), PostProcessEvent.AfterStack, "Desaturate")]
    public class Desaturate : PostProcessEffectSettings
    {
        [Range(0f, 1f)]
        public FloatParameter Intensity = new FloatParameter { value = 0f };
    }

    public sealed class DesaturateRenderer : PostProcessEffectRenderer<Desaturate>
    {
        const string ShaderPath = "Hidden/PostProcessing/Desaturate";

        public override void Render(PostProcessRenderContext context)
        {
            // attempt to retrieve the shader
            var shader = Shader.Find(ShaderPath);
            if (shader == null)
            {
                Debug.LogError("Failed to find shader: " + ShaderPath);
                return;
            }

            // attempt to retrieve the property sheet
            var propertySheet = context.propertySheets.Get(shader);
            if (propertySheet == null)
            {
                Debug.LogError("Failed to retrieve property sheet for shader: " + ShaderPath);
                return;
            }

            // configure the shader
            propertySheet.properties.SetFloat("_Intensity", settings.Intensity);

            context.command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 0);
        }
    }
}
