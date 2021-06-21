using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eleon.Modding;

namespace LevelUp
{
    class API
    {
        public static void Alert(int Target, string Message, string Alert, float Time)
        {
            byte prio = 2;
            if (Alert == "red")
            {
                prio = 0;
            }
            else if (Alert == "yellow")
            {
                prio = 1;
            }
            else
            {
                prio = 2;
            }

            if (Target == 0)
            {
                Storage.GameAPI.Game_Request(CmdId.Request_InGameMessage_AllPlayers, (ushort)Storage.CurrentSeqNr, new IdMsgPrio(Target, Message, prio, Time));
            }
            else if (Target < 999)
            {
                Storage.GameAPI.Game_Request(CmdId.Request_InGameMessage_Faction, (ushort)Storage.CurrentSeqNr, new IdMsgPrio(Target, Message, prio, Time));
            }
            else if (Target > 999)
            {
                Storage.GameAPI.Game_Request(CmdId.Request_InGameMessage_SinglePlayer, (ushort)Storage.CurrentSeqNr, new IdMsgPrio(Target, Message, prio, Time));
            }
        }

        public static void Chat(string Type, int Target, string Message)
        {
            if (Type == "Global")
            {
                API.ConsoleCommand("say '" + Message + "'");
            }
            else if (Type == "Faction")
            {
                API.ConsoleCommand("say f:" + Target + " '" + Message + "'");
            }
            else if (Type == "Player")
            {
                API.ConsoleCommand("say p:" + Target + " '" + Message + "'");
            }
        }

        public static int PlayerInfo(int playerID)
        {
            Storage.CurrentSeqNr = CommonFunctions.SeqNrGenerator(Storage.CurrentSeqNr);
            Storage.GameAPI.Game_Request(CmdId.Request_Player_Info, (ushort)Storage.CurrentSeqNr, new Id(playerID));
            return Storage.CurrentSeqNr;
        }

        public static int TextWindowOpen(int TargetPlayer, string Message, String ConfirmText, String CancelText)
        {
            Storage.CurrentSeqNr = CommonFunctions.SeqNrGenerator(Storage.CurrentSeqNr);
            Storage.GameAPI.Game_Request(CmdId.Request_ShowDialog_SinglePlayer, (ushort)Storage.CurrentSeqNr, new DialogBoxData()
            {
                Id = Convert.ToInt32(TargetPlayer),
                MsgText = Message,
                NegButtonText = CancelText,
                PosButtonText = ConfirmText
            });
            return Storage.CurrentSeqNr;
        }

        public static int Gents(string playfield)
        {
            Storage.CurrentSeqNr = CommonFunctions.SeqNrGenerator(Storage.CurrentSeqNr);
            Storage.GameAPI.Game_Request(CmdId.Request_GlobalStructure_Update, (ushort)Storage.CurrentSeqNr, new PString(playfield));
            return Storage.CurrentSeqNr;
        }

        public static void ConsoleCommand(String Sendable)
        {
            Storage.GameAPI.Game_Request(CmdId.Request_ConsoleCommand, (ushort)Storage.CurrentSeqNr, new PString(Sendable));
        }

        public static int OpenItemExchange(int PlayerID, string Title, string Message, string ButtonText, ItemStack[] Inventory)
        {
            Storage.CurrentSeqNr = CommonFunctions.SeqNrGenerator(Storage.CurrentSeqNr);
            Storage.GameAPI.Game_Request(CmdId.Request_Player_ItemExchange, (ushort)Storage.CurrentSeqNr, new ItemExchangeInfo(PlayerID, Title, Message, ButtonText, Inventory));
            return Storage.CurrentSeqNr;
        }

        public static void Credits(int PlayerID, Double CreditChange)
        {
            Storage.GameAPI.Game_Request(CmdId.Request_Player_AddCredits, (ushort)Storage.CurrentSeqNr, new IdCredits(PlayerID, CreditChange));
        }

        public static int EntityData(int EntityID)
        {
            Storage.CurrentSeqNr = CommonFunctions.SeqNrGenerator(Storage.CurrentSeqNr);
            Storage.GameAPI.Game_Request(CmdId.Request_Structure_BlockStatistics, (ushort)Storage.CurrentSeqNr, new Id(EntityID));
            return Storage.CurrentSeqNr;
        }

        public static void Delete(int EntityID)
        {
            Storage.GameAPI.Game_Request(CmdId.Request_Entity_Destroy, (ushort)Storage.CurrentSeqNr, new Id(EntityID));
        }

        public static int SetLevel(int Player, int Level)
        {
            Storage.CurrentSeqNr = CommonFunctions.SeqNrGenerator(Storage.CurrentSeqNr);
            string Sendable = "remoteex cl=" + Player + "\'Level =" + Level + "\'";
            Storage.GameAPI.Game_Request(CmdId.Request_ConsoleCommand, (ushort)Storage.CurrentSeqNr, new PString(Sendable));
            if (Level == 25)
            {
                Sendable = "remoteex cl=" + Player + "\'Level x+ 1\'";
                Storage.GameAPI.Game_Request(CmdId.Request_ConsoleCommand, (ushort)Storage.CurrentSeqNr, new PString(Sendable));

            }
            return Storage.CurrentSeqNr;
        }

    }
}
