using GorillaMediaDisplay.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GorillaMediaDisplay.Core;

//this is for the pc view
public class PCUIManager : MonoBehaviour
{ 
    public static GameObject MediaDisplayPC;
    
    private TextMeshProUGUI titleText, artistText, elapsedtimeText, endtimeText, statusText;
    private Texture2D       currentAlbumArtTexture;
    private Image           albumArtImage;


    private void Start()
    {
        titleText       = MediaDisplayPC.transform.Find("SongTitle")?.GetComponent<TextMeshProUGUI>();
        artistText      = MediaDisplayPC.transform.Find("ArtistName")?.GetComponent<TextMeshProUGUI>();
        elapsedtimeText = MediaDisplayPC.transform.Find("CurrentTime")?.GetComponent<TextMeshProUGUI>();
        endtimeText     = MediaDisplayPC.transform.Find("EndTime")?.GetComponent<TextMeshProUGUI>();
        statusText      = MediaDisplayPC.transform.Find("StatusText")?.GetComponent<TextMeshProUGUI>();
        
        Transform albumArtTransform = MediaDisplayPC.transform.Find("AlbumArt");
        if (albumArtTransform == null)
            return;

        albumArtImage = albumArtTransform.GetComponent<Image>();
        
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
        
        if (statusText != null)
            statusText.text = MediaManager.Paused ? "Paused" : "Playing";
        

        if (MediaManager.Icon != null && albumArtImage != null && MediaManager.Icon != currentAlbumArtTexture)
        {
            Rect   rect   = new Rect(0, 0, MediaManager.Icon.width, MediaManager.Icon.height);
            Sprite sprite = Sprite.Create(MediaManager.Icon, rect, new Vector2(0.5f, 0.5f));

            albumArtImage.sprite   = sprite;
            albumArtImage.color    = Color.white;
            currentAlbumArtTexture = MediaManager.Icon;
        }

    }
}