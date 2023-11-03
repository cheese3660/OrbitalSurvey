﻿using BepInEx.Logging;
using HarmonyLib;
using KSP.Game.Load;
using KSP.IO;
using KSP.Sim;
using Newtonsoft.Json;
using System.Reflection;

namespace OrbitalSurvey
{
    public class SaveLoadPatches
    {
        private static readonly ManualLogSource _logger = BepInEx.Logging.Logger.CreateLogSource("OrbitalSurvey.SaveLoadPatches");
        /*
        [HarmonyPatch(typeof(SequentialFlow), "AddAction"), HarmonyPrefix]
        private static bool SaveOrbitalSurveyData(FlowAction action, SequentialFlow __instance)
        {
            _logger.LogDebug($"Action name: {action.Name}");

            if (action.Name == "Serializing Save File Contents")
            {
                _logger.LogDebug("Let's see what we have here...");
            }

            return true;
        }
        */

        /*
        [HarmonyPatch(typeof(SerializeGameDataFlowAction), "DoAction"), HarmonyPrefix]
        private static bool SaveOrbitalSurveyData2(Action resolve, Action<string> reject, SerializeGameDataFlowAction __instance)
        {
            _logger.LogDebug($"SerializeGameDataFlowAction.DoAction triggered.");

            return true;
        }
        */

        [HarmonyPatch(typeof(SerializeGameDataFlowAction), MethodType.Constructor), HarmonyPostfix]
        [HarmonyPatch(new Type[] { typeof(string), typeof(LoadGameData) })]
        private static void InjectCustomLoadGameData(string filename, LoadGameData data, SerializeGameDataFlowAction __instance)
        {
            _logger.LogDebug("SerializeGameDataFlowAction constructor postfix triggered");

            MySerializedSavedGame myData = MySerializedSavedGame.CreateDerivedInstanceFromBase(data.SavedGame);
            myData.MyValue = DEBUG_UI.Instance.DataToSave;

            data.SavedGame = myData;
        }

        //////////////////  LOADING //////////////////
        /*
        [HarmonyPatch(typeof(SaveLoadManager), "PrivateLoadCommon"), HarmonyPrefix]
        private static bool LoadTest_prefix(
            LoadOrSaveCampaignTicket loadOrSaveCampaignTicket,
            LoadGameData loadGameData,
            SequentialFlow loadingFlow,
            OnLoadOrSaveCampaignFinishedCallback onLoadOrSaveCampaignFinishedCallback,
            SaveLoadManager __instance)
        {
            _logger.LogDebug("SaveLoadManager.PrivateLoadCommon prefix triggered.");

            loadGameData.SavedGame = new MySerializedSavedGame();

            return true;
        }
        */

        /*
        [HarmonyPatch(typeof(SaveLoadManager), "PrivateLoadCommon"), HarmonyPostfix]
        private static void LoadTest_Postfix(
            LoadOrSaveCampaignTicket loadOrSaveCampaignTicket,
            LoadGameData loadGameData,
            SequentialFlow loadingFlow,
            OnLoadOrSaveCampaignFinishedCallback onLoadOrSaveCampaignFinishedCallback,
            SaveLoadManager __instance)
        {
            _logger.LogDebug("SaveLoadManager.PrivateLoadCommon postfix triggered.");
        }
        */

        [HarmonyPatch(typeof(DeserializeContentsFlowAction), "DoAction"), HarmonyPrefix]
        private static bool MyDeserialization(Action resolve, Action<string> reject, DeserializeContentsFlowAction __instance)
        {
            __instance._game.UI.SetLoadingBarText(__instance.Description);
            try
            {
                MySerializedSavedGame serializedSavedGame = null;
                IOProvider.FromJsonFile<MySerializedSavedGame>(__instance._filename, out serializedSavedGame);
                __instance._data.SavedGame = serializedSavedGame;
                __instance._data.DataLength = IOProvider.GetFileSize(__instance._filename);

                // Load all saved data mods registered (TODO)
                DEBUG_UI.Instance.LoadedData = serializedSavedGame.MyValue;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogException(ex);
                reject(ex.Message);
            }
            resolve();

            return false;
        }

    }

    public class PatchTest
    {
        private static readonly ManualLogSource _logger = BepInEx.Logging.Logger.CreateLogSource("OrbitalSurvey.PatchTest");

        // EMPTY ON PURPOSE
    }

    [Serializable]
    public class MySerializedSavedGame : SerializedSavedGame
    {
        [JsonProperty("MySaveGameField")]
        public string MyValue = "MyDefaultValue";

        public static MySerializedSavedGame CreateDerivedInstanceFromBase<T>(T baseInstance) where T : SerializedSavedGame
        {
            Type baseType = typeof(T);
            Type derivedType = typeof(MySerializedSavedGame);

            if (baseType.IsAssignableFrom(derivedType))
            {
                // Create an instance of the derived class using reflection
                object derivedObj = Activator.CreateInstance(derivedType);

                // Copy the properties from the base class instance to the derived class instance
                foreach (FieldInfo field in derivedType.GetFields())
                {
                    FieldInfo baseField = baseType.GetField(field.Name);
                    if (baseField != null)
                    {
                        object value = baseField.GetValue(baseInstance);
                        field.SetValue(derivedObj, value);
                    }
                }

                return (MySerializedSavedGame)derivedObj;
            }
            else
            {
                throw new InvalidOperationException("The provided type does not derive from the expected base class.");
            }
        }
    }
}
