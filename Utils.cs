using Assets;
using System.Collections.Generic;
using System.Linq;

namespace RandomBattlegroundsBoard
{
    public class Utils
    {
        public static List<int> CacheBgsBoard = new List<int>();

        public static class CacheInfo
        {
            public static void UpdateBgsBoard()
            {
                CacheBgsBoard.Clear();
                // Standard board
                CacheBgsBoard.Add(1);
                HashSet<Hearthstone.BattlegroundsBoardSkinId> ownedBgSkins = NetCache.Get().GetNetObject<NetCache.NetCacheBattlegroundsBoardSkins>().OwnedBattlegroundsBoardSkins;
                foreach (var skinId in ownedBgSkins)
                {
                    CacheBgsBoard.Add(skinId.ToValue());
                }
            }
        }

        public static int GetRandomBoardSkinId()
        {
            if (CacheBgsBoard.Count == 0)
            {
                CacheInfo.UpdateBgsBoard();
            }
            var skin = CacheBgsBoard[UnityEngine.Random.Range(0, CacheBgsBoard.Count)];
            var allSkinsInfo = CacheBgsBoard.Select(id => $"(id={id})");
            RandomBattlegroundsBoardMod.SharedLogger.Msg($"Loaded new board skin {skin}. All options were {string.Join(", ", allSkinsInfo)}");
            return skin;
        }
    }
}
