﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Behavior;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.NeoProfiles;
using ff14bot.Pathing.Service_Navigation;
using LlamaLibrary.Helpers;
using LlamaLibrary.Logging;
using LlamaLibrary.Structs;
using Newtonsoft.Json;
using TreeSharp;

//using UI_Checker;

namespace LlamaBotBases.ResplendentTools
{
    public class ResplendentToolsCrafter : BotBase
    {
        private static readonly LLogger Log = new LLogger(_name, Colors.Gold);

        private Composite _root;
        public override string Name => _name;

        private static readonly string _name = "ResplendentTools";
        public override PulseFlags PulseFlags => PulseFlags.All;

        public override bool IsAutonomous => true;
        public override bool RequiresProfile => false;

        public override Composite Root => _root;

        public override bool WantButton { get; } = false;

        public static bool Carpenter => true;
        public static bool Blacksmith => true;
        public static bool Armorer => true;
        public static bool Goldsmith => true;
        public static bool Leatherworker => true;
        public static bool Weaver => true;
        public static bool Alchemist => true;
        public static bool Culinarian => true;

        private async Task<bool> Run()
        {
            while (true)
            {
                await TurninResplendentCrafting();
                await Coroutine.Sleep(2000);
                await DoResplendentCrafting();
                await Coroutine.Sleep(2000);
            }

            TreeRoot.Stop("Stop Requested");
            return true;
        }

        public override void Start()
        {
            Navigator.PlayerMover = new SlideMover();
            Navigator.NavigationProvider = new ServiceNavigationProvider();
            _root = new ActionRunCoroutine(r => Run());
        }

        public override void Stop()
        {
            (Navigator.NavigationProvider as IDisposable)?.Dispose();
            Navigator.NavigationProvider = null;
            _root = null;
        }

        public static Task<string> CalculateLisbethResplendentOrder(string job, int finalItemId, int cMaterialId, int bMaterialId, int cComponentId, int bComponentId, int aComponentId)
        {
            var outList = new List<LisbethOrder>();

            var item = InventoryManager.FilledSlots.FirstOrDefault(i => i.RawItemId == finalItemId);
            var finalItemCount = (int) (item == null ? 0 : item.Count);

            LisbethOrder order;
            if (ConditionParser.HasAtLeast((uint) cMaterialId, 2))
            {
                order = new LisbethOrder(1, 1, cComponentId, NumberToCraft(finalItemCount, cMaterialId), job);
            }
            else if (ConditionParser.HasAtLeast((uint) bMaterialId, 2))
            {
                order = new LisbethOrder(1, 1, bComponentId, NumberToCraft(finalItemCount, bMaterialId), job);
            }
            else
            {
                order = new LisbethOrder(1, 1, aComponentId, 30 - (finalItemCount / 2), job);
            }

            outList.Add(order);
            return Task.FromResult(JsonConvert.SerializeObject(outList, Formatting.None));
        }

        public static async Task<string> GetLisbethResplendentOrder()
        {
            if (Carpenter && ConditionParser.HasAtLeast(33210, 60) == false && ConditionParser.HasAtLeast(33154, 1) == false)
            {
                return await CalculateLisbethResplendentOrder("Carpenter", 33210, 33202, 33194, 36327, 36319, 36311);
            }

            if (Blacksmith && ConditionParser.HasAtLeast(33211, 60) == false && ConditionParser.HasAtLeast(33155, 1) == false)
            {
                return await CalculateLisbethResplendentOrder("Blacksmith", 33211, 33203, 33195, 36328, 36320, 36312);
            }

            if (Armorer && ConditionParser.HasAtLeast(33212, 60) == false && ConditionParser.HasAtLeast(33156, 1) == false)
            {
                return await CalculateLisbethResplendentOrder("Armorer", 33212, 33204, 33196, 36329, 36321, 36313);
            }

            if (Goldsmith && ConditionParser.HasAtLeast(33213, 60) == false && ConditionParser.HasAtLeast(33157, 1) == false)
            {
                return await CalculateLisbethResplendentOrder("Goldsmith", 33213, 33205, 33197, 36330, 36322, 36314);
            }

            if (Leatherworker && ConditionParser.HasAtLeast(33214, 60) == false && ConditionParser.HasAtLeast(33158, 1) == false)
            {
                return await CalculateLisbethResplendentOrder("Leatherworker", 33214, 33206, 33198, 36331, 36323, 36315);
            }

            if (Weaver && ConditionParser.HasAtLeast(33215, 60) == false && ConditionParser.HasAtLeast(33159, 1) == false)
            {
                return await CalculateLisbethResplendentOrder("Weaver", 33215, 33207, 33199, 36332, 36324, 36316);
            }

            if (Alchemist && ConditionParser.HasAtLeast(33216, 60) == false && ConditionParser.HasAtLeast(33160, 1) == false)
            {
                return await CalculateLisbethResplendentOrder("Alchemist", 33216, 33208, 33200, 36333, 36325, 36317);
            }

            if (Culinarian && ConditionParser.HasAtLeast(33217, 60) == false && ConditionParser.HasAtLeast(33161, 1) == false)
            {
                return await CalculateLisbethResplendentOrder("Culinarian", 33217, 33209, 33201, 36334, 36326, 36318);
            }

            return "";
        }

        private static int NumberToCraft(int finalItemCount, int currentIngredientId)
        {
            var currentIngredient = InventoryManager.FilledSlots.FirstOrDefault(i => i.RawItemId == currentIngredientId);
            var currentIngredientCount = (int) (currentIngredient == null ? 0 : currentIngredient.Count);

            return Math.Min(30 - (finalItemCount / 2), currentIngredientCount / 2);
        }

        public async Task DoResplendentCrafting()
        {
            var lisbethOrder = await GetLisbethResplendentOrder();

            if (lisbethOrder == "")
            {
                Log.Warning("Not Calling lisbeth.");
            }
            else
            {
                Log.Information("Calling lisbeth");
                if (!await Lisbeth.ExecuteOrders(lisbethOrder))
                {
                    Log.Error("Lisbeth order failed, Dumping order to GCSupply.json");
                    using (var outputFile = new StreamWriter("GCSupply.json", false))
                    {
                        await outputFile.WriteAsync(lisbethOrder);
                    }
                }
                else
                {
                    Log.Information("Lisbeth order should be done");
                }
            }
        }

        public static async Task TurninResplendentCrafting()
        {
            await GeneralFunctions.TurninResplendentCrafting();
        }
    }
}
