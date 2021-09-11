using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingModifier : MonoBehaviour
{
    [SerializeField] PostProcessVolume LinkedVolume;
    [SerializeField] float BloomIntensity = 1f;
    [SerializeField] float VignetteIntensity = 0f;

    Bloom bloomSettings;
    Vignette vignetteSettings;

    // Start is called before the first frame update
    void Start()
    {
        LinkedVolume.profile.TryGetSettings<Bloom>(out bloomSettings);

        // try to retrieve the vignette
        if (!LinkedVolume.profile.TryGetSettings<Vignette>(out vignetteSettings))
        {
            // add the vignette as it was not found
            vignetteSettings = LinkedVolume.profile.AddSettings<Vignette>();

            // enable overriding of the intensity
            vignetteSettings.intensity.Override(VignetteIntensity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // update the settings if they were retrieved
        if (bloomSettings != null)
            bloomSettings.intensity.value = BloomIntensity;
        if (vignetteSettings != null)
            vignetteSettings.intensity.value = VignetteIntensity;
    }
}
