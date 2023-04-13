﻿using Exiled.API.Features;
using System;
using HarmonyLib;
using Map = Exiled.Events.Handlers.Map;
using Player = Exiled.Events.Handlers.Player;

namespace BetterCoinflips
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance;
        public override Version RequiredExiledVersion => new Version(6,0,0);
        public override Version Version => new Version(2, 0,0);
        public override string Author => "Miki_hero";

        private EventHandlers _eventHandler;
        private Harmony _harmony;
        
        public override void OnEnabled()
        {
            Instance = this;
            RegisterEvents();
            Patch();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnPatch();
            UnRegisterEvents();
            Instance = null;
            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            _eventHandler = new EventHandlers();
            Player.FlippingCoin += _eventHandler.OnCoinFlip;
            Map.SpawningItem += _eventHandler.OnSpawningItem;
            Player.InteractingDoor += _eventHandler.OnInteractingDoorEventArgs;
        }

        private void UnRegisterEvents()
        {
            Player.FlippingCoin -= _eventHandler.OnCoinFlip;
            Map.SpawningItem -= _eventHandler.OnSpawningItem;
            Player.InteractingDoor -= _eventHandler.OnInteractingDoorEventArgs;
            _eventHandler = null;
        }

        private void Patch()
        {
            try
            {
                _harmony = new Harmony("bettercoinflips.patch");
                _harmony.PatchAll();
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to patch: {ex}");
                _harmony.UnpatchAll();
            }
        }

        private void UnPatch()
        {
            _harmony.UnpatchAll();
            _harmony = null;
        }
    }
}