using Fishy.Models.Packets;
using Fishy.Models;
using System.Numerics;

namespace Fishy.Utils
{
    public static class Spawner
    {
        static float _rainChance = 0.0f;
        static int _alienCooldown = 16;
        static readonly ActorType[] _baseTypes = [ActorType.FISH_SPAWN, ActorType.NONE];
        static readonly Random _random = new();

        // Vanilla spawn routine, ported from world.gd
        public static void SpawnIngameEvent()
        {
            foreach (Actor instance in Fishy.Actors.ToList())
            {
                float instanceAge = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - instance.SpawnTime.ToUnixTimeSeconds();
                if ((instance.Type == ActorType.FISH_SPAWN_ALIEN && instanceAge > 120)
                    || (instance.Type == ActorType.FISH_SPAWN && instanceAge > 80)
                    || (instance.Type == ActorType.RAINCLOUD && instanceAge > 550)
                    || (instance.Type == ActorType.VOID_PORTAL && instanceAge > 600))
                {
                    RemoveActor(instance);
                }
            }

            int count = Fishy.Actors.Where(a => a.Type == ActorType.METAL_SPAWN).Count();
            if (count < 7)
                SpawnMetalSpot();

            ActorType type = _baseTypes[_random.Next(0, 2)];

            _alienCooldown -= 1;

            if (_random.NextSingle() < 0.01 && _random.NextSingle() < 0.4 && _alienCooldown <= 0)
            {
                type = ActorType.FISH_SPAWN_ALIEN;
                _alienCooldown = 16;
            }

            if (_random.NextSingle() < _rainChance && _random.NextSingle() < .12f)
            {
                type = ActorType.RAINCLOUD;
                _rainChance = 0;
            }
            else
                if (_random.NextSingle() < .75f) _rainChance += .001f;

            if (_random.NextSingle() < 0.01f && _random.NextSingle() < 0.25)
                type = ActorType.VOID_PORTAL;

            switch (type)
            {
                case ActorType.NONE:
                    return;
                case ActorType.FISH_SPAWN:
                    if (Fishy.Actors.Count > 15)
                        return;
                    SpawnFish();
                    break;
                case ActorType.FISH_SPAWN_ALIEN:
                    SpawnFish(type);
                    break;
                case ActorType.RAINCLOUD:
                    SpawnRainCloud();
                    break;
                case ActorType.VOID_PORTAL:
                    SpawnVoidPortal();
                    break;
            }
        }

        public static void SpawnFish(ActorType type = ActorType.FISH_SPAWN)
        {
            int id = _random.Next();
            Vector3 pos = Fishy.World.FishSpawns[_random.Next(Fishy.World.FishSpawns.Count)];
            SpawnActor(new Actor(id, type, pos));
            if (type != ActorType.FISH_SPAWN)
                new ActorRemovePacket(id) { Action = "_ready" }.SendPacket("all", (int)CHANNELS.GAME_STATE);
        }

        public static void SpawnRainCloud()
        {
            Vector3 pos = new(_random.Next(-100, 150), 42f, _random.Next(-150, 100));
            SpawnActor(new RainCloud(GetFreeId(), pos));
        }

        public static void SpawnMetalSpot()
        {
            Vector3 pos = Fishy.World.TrashPoints[_random.Next(Fishy.World.TrashPoints.Count)];

            if (_random.NextSingle() < .15f)
                pos = Fishy.World.ShorelinePoints[_random.Next(Fishy.World.ShorelinePoints.Count)];

            SpawnActor(new Actor(GetFreeId(), ActorType.METAL_SPAWN, pos));
        }

        public static void SpawnVoidPortal()
        {
            Vector3 pos = Fishy.World.HiddenSpots[_random.Next(Fishy.World.HiddenSpots.Count)];
            SpawnActor(new Actor(GetFreeId(), ActorType.VOID_PORTAL, pos));
        }
        public static int GetFreeId()
            => _random.Next();
        
        public static void SpawnActor(Actor actor)
        {
            new ActorSpawnPacket(actor.Type, actor.Position, actor.InstanceID).SendPacket("all", (int)CHANNELS.GAME_STATE);
            if (!Fishy.Actors.Contains(actor))
                Fishy.Actors.Add(actor);
            if (actor.Rotation == default)
                return;
            new ActorUpdatePacket(actor.InstanceID, actor.Position, actor.Rotation).SendPacket("all", (int)CHANNELS.GAME_STATE);

        }
        public static Actor SpawnActor(ActorType type, Vector3 position, Vector3 entRot = default)
        {
            var actor = new Actor(GetFreeId(), type, position, entRot);
            SpawnActor(actor);
            return actor;
        }
        public static void RemoveActor(Actor actor)
        {
            new ActorRemovePacket(actor.InstanceID).SendPacket("all", (int)CHANNELS.GAME_STATE);
            Fishy.Actors.Remove(actor);
        }

        public static void RemoveActor(int ID)
        {
            new ActorRemovePacket(ID).SendPacket("all", (int)CHANNELS.GAME_STATE);
            for (int i=0; i<Fishy.Actors.Count; i++)
            {
                if (i >= Fishy.Actors.Count)
                    return;
                var actor = Fishy.Actors[i];
                if (actor.InstanceID != ID)
                    continue;
                Fishy.Actors.Remove(actor);
                return;
            }
        }
    }
 }
