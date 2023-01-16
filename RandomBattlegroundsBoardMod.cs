using MelonLoader;
using HarmonyLib;
using System.Linq;
using System;

namespace RandomBattlegroundsBoard
{
    public class RandomBattlegroundsBoardMod : MelonMod
    {
        public static MelonLogger.Instance SharedLogger;

        public static int currentBoardSkinId = 0;

        public override void OnInitializeMelon()
        {
            RandomBattlegroundsBoardMod.SharedLogger = LoggerInstance;
            var harmony = this.HarmonyInstance;
            harmony.PatchAll(typeof(BaconBoardPatcher));
            harmony.PatchAll(typeof(GameMgrPatcher));
        }
    }


    public static class GameMgrPatcher
    {
        [HarmonyPatch(typeof(GameMgr), "OnGameSetup")]
        [HarmonyPostfix]
        public static void OnGameSetupPostfix()
        {
            if (GameMgr.Get().IsBattlegrounds())
            {
                RandomBattlegroundsBoardMod.currentBoardSkinId = Utils.GetRandomBoardSkinId();
            }
        }
    }

    // Implementation insired by https://github.com/Pik-4/HsMod/blob/master/HsMod/Patcher.cs#L1955
    public static class BaconBoardPatcher
    {
        [HarmonyPatch(typeof(BaconBoard), "LoadInitialTavernBoard")]
        [HarmonyPrefix]
        public static bool PrefixLoadInitialTavernBoard(Entity __instance, ref int chosenBoardSkinId)
        {
            //RandomBattlegroundsBoardMod.SharedLogger.Msg($"LoadInitialTavernBoard {chosenBoardSkinId}");
            chosenBoardSkinId = RandomBattlegroundsBoardMod.currentBoardSkinId;
            return true;
        }

        [HarmonyPatch(typeof(BaconBoard), "OnBoardSkinChosen")]
        [HarmonyPrefix]
        public static bool PrefixOnBoardSkinChosen(Entity __instance, ref int chosenBoardSkinId)
        {
            //RandomBattlegroundsBoardMod.SharedLogger.Msg($"OnBoardSkinChosen {chosenBoardSkinId}");
            chosenBoardSkinId = RandomBattlegroundsBoardMod.currentBoardSkinId;
            return true;
        }
    }
}
