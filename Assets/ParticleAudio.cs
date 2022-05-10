using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAudio : MonoBehaviour
{
    private AudioSource pop;
    private ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        pop = this.GetComponent<AudioSource>();
        ps = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(ps.particleCount);
        pop.pitch = Random.Range(0.2f, 0.8f);
        pop.Play();
        if(ps.particleCount == 0)
        {
            pop.Stop();
        }
    }
}
