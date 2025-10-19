using ExitGames.Client.Photon;
using GorillaMediaDisplay.Tools;
using Photon.Pun;
using UnityEngine;

namespace GorillaMediaDisplay.Core;

//This will later receive full networking im just too lazy to do it rn
public class NetworkingManager : MonoBehaviour
{
    private void Update()
    {
        string formattedMediaData =
                $"\nMEDIA DATA:\n" +
                $"Title: {MediaManager.Title}\n" +
                $"Artist: {MediaManager.Artist}\n" +
                $"AlbumArt Hash: {MediaManager.AlbumArt}\n" +
                $"Progression: {GMDUtilities.FormatTime(MediaManager.ElapsedTime)} / {GMDUtilities.FormatTime(MediaManager.EndTime)}\n" +
                $"Status: {(MediaManager.Paused ? "Paused" : "Playing")}";
        
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
        {
                {
                        Constants.NetworkKey, formattedMediaData
                },
        });
    }
}