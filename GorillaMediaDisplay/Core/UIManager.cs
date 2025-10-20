using GorillaMediaDisplay.Tools;
using TMPro;
using UnityEngine;

namespace GorillaMediaDisplay.Core;

//this is for the vr view
public class UIManager : MonoBehaviour
{
    public static GameObject MediaDisplay;
    
    private TextMeshPro titleText, artistText, elapsedtimeText, endtimeText;
    private Renderer    albumArtRenderer;
    private Texture2D currentAlbumArtTexture;
    private Material  albumArtMaterialInstance;

    private GameObject playIcon;
    private GameObject pauseIcon;

    private void Start()
    {
        foreach (Collider col in MediaDisplay.GetComponentsInChildren<Collider>())
            col.enabled = false;
        
        
        titleText       = MediaDisplay.transform.Find("SongTitle")?.GetComponent<TextMeshPro>();
        artistText      = MediaDisplay.transform.Find("ArtistName")?.GetComponent<TextMeshPro>();
        elapsedtimeText = MediaDisplay.transform.Find("elapsedTime")?.GetComponent<TextMeshPro>();
        endtimeText     = MediaDisplay.transform.Find("endTime")?.GetComponent<TextMeshPro>();
        playIcon        = MediaDisplay.transform.Find("play")?.gameObject;
        pauseIcon       = MediaDisplay.transform.Find("pause")?.gameObject;
        
        Transform albumArtTransform = MediaDisplay.transform.Find("AlbumArt");
        if (albumArtTransform == null)
            return;

        albumArtRenderer = albumArtTransform.GetComponent<Renderer>();
        if (albumArtRenderer == null)
            return;
        
        albumArtMaterialInstance  = new Material(albumArtRenderer.material);
        albumArtRenderer.material = albumArtMaterialInstance;
    }

    private void LateUpdate()
    {
        // ReSharper disable once InvertIf
        if (MediaManager.Instance == null)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            Debug.LogWarning("MediaManager's instance is null");
            return;
        }
        
        if (titleText != null)
            titleText.text = MediaManager.Title;

        if (artistText != null)
            artistText.text = MediaManager.Artist;

        if (elapsedtimeText != null)
            elapsedtimeText.text = GMDUtilities.FormatTime(MediaManager.ElapsedTime);

        if (endtimeText != null)
            endtimeText.text = GMDUtilities.FormatTime(MediaManager.EndTime);


        if (playIcon != null && pauseIcon != null)
        {
            playIcon.SetActive(!MediaManager.Paused);
            pauseIcon.SetActive(MediaManager.Paused);
        }


        if (MediaManager.Icon != null && albumArtMaterialInstance != null && MediaManager.Icon != currentAlbumArtTexture)
        {
            albumArtMaterialInstance.mainTexture = MediaManager.Icon;
            currentAlbumArtTexture               = MediaManager.Icon;
            albumArtMaterialInstance.color       = Color.white * 1.1f;
            Debug.Log("Album art updated.");
        }
    }
}