using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace GorillaMediaDisplay
{
    public class MediaManager : MonoBehaviour
    {
        public static string Title { get; private set; } = "Loading...";
        public static string Artist { get; private set; } = "Loading...";
        public static string AlbumArt { get; private set; } = "INVALIDJSONHASH";
        public static Texture2D Icon { get; private set; } = new Texture2D(2, 2);
        public static bool Paused { get; private set; } = true;
        public static bool ValidData { get; private set; }

        public static float StartTime { get; private set; }
        public static float EndTime { get; private set; }
        public static float ElapsedTime { get; private set; }

        public static string quickSongPath { get; private set; }
        public static MediaManager instance { get; private set; }

        public void Awake()
        {
            instance = this;

            string resourcePath = "GorillaMediaDisplay.Assets.QuickSong.exe";
            quickSongPath = Path.Combine(Path.GetTempPath(), "QuickSong.exe");

            if (File.Exists(quickSongPath))
                File.Delete(quickSongPath);

            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
            using FileStream fs = new FileStream(quickSongPath, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fs);
        }

        public static async Task UpdateDataAsync()
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = quickSongPath,
                Arguments = "-all",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using Process proc = new Process { StartInfo = psi };
            proc.Start();
            string output = await proc.StandardOutput.ReadToEndAsync();

            await Task.Run(() => proc.WaitForExit());

            ValidData = false;
            Paused = true;
            Title = "No Media";
            Artist = "Detected";
            AlbumArt = "INVALIDJSONHASH";

            StartTime = 0f;
            EndTime = 0f;
            ElapsedTime = 0f;

            try
            {
                Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(output);
                Title = (string)data["Title"];
                Artist = (string)data["Artist"];

                StartTime = Convert.ToSingle(data["StartTime"]);
                EndTime = Convert.ToSingle(data["EndTime"]);
                ElapsedTime = Convert.ToSingle(data["ElapsedTime"]);

                Paused = (string)data["Status"] != "Playing";
                Icon.LoadImage(Convert.FromBase64String((string)data["ThumbnailBase64"]));

                if (data.ContainsKey("ThumbnailBase64") && !string.IsNullOrEmpty((string)data["ThumbnailBase64"]))
                {
                    byte[] thumbBytes = Convert.FromBase64String((string)data["ThumbnailBase64"]);
                    using (var md5 = System.Security.Cryptography.MD5.Create())
                    {
                        byte[] hash = md5.ComputeHash(thumbBytes);
                        AlbumArt = BitConverter.ToString(hash).Replace("-", "").ToLower();
                    }
                }
                else
                {
                    AlbumArt = "NOTHUMBNAILHASH [Err -01]";
                }

                ValidData = true;
            }
            catch { }
        }

        IEnumerator UpdateDataCoroutine(float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            _ = UpdateDataAsync();
            yield return null;
        }

        private static float updateDataLatency;
        public void Update()
        {
            if (Time.time > updateDataLatency)
            {
                updateDataLatency = Time.time + 1f;
                StartCoroutine(UpdateDataCoroutine());
            }

            if (!Paused)
                ElapsedTime += Time.deltaTime;
        }
    }
}