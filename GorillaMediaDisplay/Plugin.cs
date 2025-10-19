using System.IO;
using System.Reflection;
using BepInEx;
using ExitGames.Client.Photon;
using GorillaLocomotion;
using GorillaMediaDisplay.Core;
using Photon.Pun;
using UnityEngine;

namespace GorillaMediaDisplay;

[BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
public class Plugin : BaseUnityPlugin
{
    private GameObject mediaDisplay;
    private GameObject mediaDisplayPC;

    private void Awake() => Logger.Log(Constants.Description);

    private void Start() => GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);

    private void OnPlayerSpawned()
    {
#region VRView
        
        Stream bundleStream = Assembly.GetExecutingAssembly()
                                      .GetManifestResourceStream("GorillaMediaDisplay.Assets.gorillamediadisplay");

        AssetBundle bundle = AssetBundle.LoadFromStream(bundleStream);
        // ReSharper disable once PossibleNullReferenceException
        bundleStream.Close();

        mediaDisplay = Instantiate(bundle.LoadAsset<GameObject>("MP3"));
        
        // ReSharper disable once PossibleNullReferenceException
        // ReSharper disable once Unity.InstantiateWithoutParent
        mediaDisplay.transform.SetParent(Camera.main.transform, false);
        mediaDisplay.transform.localPosition = new Vector3(-0.3446f, 0.1754f, 0.5489f);
        mediaDisplay.transform.localRotation = Quaternion.Euler(0f, 70f, -5f);
        mediaDisplay.transform.localScale    = Vector3.one * 0.015f;
        mediaDisplay.SetLayer(UnityLayer.FirstPersonOnly);  //no work, idk why but yeah?

        mediaDisplay.AddComponent<UIManager>();
        UIManager.MediaDisplay = mediaDisplay;

        mediaDisplay.AddComponent<NetworkingManager>();
        mediaDisplay.AddComponent<MediaManager>();
        
#endregion

#region PCView

        Stream bundleStreampc = Assembly.GetExecutingAssembly()
                                      .GetManifestResourceStream("GorillaMediaDisplay.Assets.pcmediadisplay");

        AssetBundle bundlepc = AssetBundle.LoadFromStream(bundleStreampc);
        // ReSharper disable once PossibleNullReferenceException
        bundleStream.Close();

        mediaDisplayPC = Instantiate(bundlepc.LoadAsset<GameObject>("GMDCanvas"));
        
        mediaDisplay.AddComponent<PCUIManager>();
        PCUIManager.MediaDisplayPC = mediaDisplayPC;

#endregion
    }
}