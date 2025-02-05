using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAnim : MonoBehaviour
{
    private Animator player;
    public AudioSource audioSource;

    public AudioClip RoadFTStep;  
    public AudioClip WastlandFT;   
   

    private bool isPlayingFootstep = false; 

    private void Start()
    {
        player = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (VRMovement.instance != null)
        {
            bool isMoving = VRMovement.instance.IsMoving;

           
            player.SetBool("Move", isMoving);

            if (isMoving)
            {
                PlayDynamicFootstep(); 
            }
            else
            {
                StopFootstepSound(); 
            }
        }
    }

    public void PlayDynamicFootstep()
    {
        if (VRMovement.instance == null || audioSource == null) return;

     
       if (isPlayingFootstep) return;

        string surfaceType = VRMovement.instance.CurrentSurface; 
        AudioClip footstepClip = null;

        if (surfaceType == "WastelandPlane")
        {
            footstepClip = WastlandFT;
        }
        else if (surfaceType == "RoadPlane")
        {
            footstepClip = RoadFTStep;
        }


        if (footstepClip != null)
        {
            StartCoroutine(PlayFootstepWithInterval(footstepClip, 0.5f)); 
        }
    }

     IEnumerator PlayFootstepWithInterval(AudioClip clip, float interval)
    {
        isPlayingFootstep = true;
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(interval);
        isPlayingFootstep = false;
    }

    public void StopFootstepSound()
    {
        isPlayingFootstep = false;
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

 
}
