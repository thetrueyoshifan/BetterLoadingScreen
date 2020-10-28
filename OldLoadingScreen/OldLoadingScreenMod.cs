﻿// Pls be gentle this is my first mod, I know it's messy
// If there's better or more efficient ways of doing things please let me know :)


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using UnhollowerRuntimeLib;
using MelonLoader;
using OldLoadingScreen;
using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Networking;
using Object = UnityEngine.Object;
using VRC;
using VRC.Core;
// using VRCSDK2;

[assembly: MelonInfo(typeof(OldLoadingScreenMod), "OldLoadingScreen", "0.2.1", "Grummus")]
[assembly: MelonGame("VRChat", "VRChat")]


namespace OldLoadingScreen
{
    public class OldLoadingScreenMod : MelonMod
    {
        
        private AudioClip partiallyOfflineAudio;
        /*

        private GameObject partiallyOffline;
        */
        private GameObject cavernDry;
        private AudioSource cavernDrySource;
        private GameObject loadScreenPrefab;

        private AssetBundle assets;

        public override void OnApplicationStart()
        {
            MelonLogger.Log("ApplicationStart");

        }

        public override void VRChat_OnUiManagerInit()
        {
            // while (ReferenceEquals(VRCAudioManager.field_Private_Static_VRCAudioManager_0, null)) yield return null;
            MelonLogger.Log("Start init 2: electric boogaloo");

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("OldLoadingScreen.oldloadingscreen.assetbundle"))
            using (var tempStream = new MemoryStream((int)stream.Length))
            {
                stream.CopyTo(tempStream);

                assets = AssetBundle.LoadFromMemory_Internal(tempStream.ToArray(), 0);
                assets.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            }

            loadScreenPrefab = assets.LoadAsset_Internal("Assets/Bundle/OldLoadingScreen.prefab", Il2CppType.Of<GameObject>()).Cast<GameObject>();
            loadScreenPrefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            cavernDry = assets.LoadAsset_Internal("Assets/Bundle/CavernDry.prefab", Il2CppType.Of<GameObject>()).Cast<GameObject>();
            cavernDry.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            CreateGameObjects();
        }

        private void CreateGameObjects()
        {
            MelonLogger.Log("Creating Game Objects");
            var UIRoot = GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup");
            var InfoPanel = GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel");
            var SkyCube = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/SkyCube_Baked");
            var bubbles = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/_FX_ParticleBubbles");
            var StartScreen = GameObject.Find("/UserInterface/LoadingBackground_TealGradient_Music/");
            var originalStartScreenAudio = GameObject.Find("/UserInterface/LoadingBackground_TealGradient_Music/LoadingSound");
            var originalStartScreenSkyCube = GameObject.Find("/UserInterface/LoadingBackground_TealGradient_Music/SkyCube_Baked");
            var originalLoadingAudio = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/LoadingSound");

            MelonLogger.Log("Finished Finding GameObjects");

            loadScreenPrefab = CreateGameObject(loadScreenPrefab, new Vector3(400, 400, 400), "UserInterface/MenuContent/Popups/", "LoadingPopup");
            cavernDry = CreateGameObject(cavernDry, new Vector3(400, 400, 400), "UserInterface/", "LoadingBackground_TealGradient_Music");



            InfoPanel.active = false;
            SkyCube.active = false;
            bubbles.active = false;
            // AudioSource menuAudioComp = originalMenuAudio.GetComponent<AudioSource>();
            // menuAudioComp.clip = cavernDryAudio;
            originalLoadingAudio.active = false;
            originalStartScreenAudio.active = false;
            originalStartScreenSkyCube.active = false;

        }

        private AudioSource CreateAudioSource(AudioClip clip, GameObject parent)
        {
            var source = parent.AddComponent<AudioSource>();
            source.clip = clip;
            source.spatialize = false;
            source.volume = 100;
            source.loop = false;
            source.playOnAwake = false;
            source.outputAudioMixerGroup = VRCAudioManager.field_Private_Static_VRCAudioManager_0.uiGroup;
            return source;
        }

        private GameObject CreateGameObject(GameObject obj, Vector3 scale, String rootDest, String parent)
        {
            MelonLogger.Log("Creating " + obj.name);
            var UIRoot = GameObject.Find(rootDest);
            var requestedParent = UIRoot.transform.Find(parent);
            // var requestedParent = UIRoot.transform;
            var newObject = Object.Instantiate(obj, requestedParent, false).Cast<GameObject>();
            newObject.transform.parent = requestedParent;
            newObject.transform.localScale = scale;
            return newObject;
        }
    }
}