using Fishy.Helper;
using System.Numerics;

namespace Fishy.Models
{
	public enum ActorType
	{
		NONE, PLAYER, FISH_SPAWN, FISH_SPAWN_ALIEN, RAINCLOUD, METAL_SPAWN, VOID_PORTAL,
	}

	public class Actor
    {
        public int InstanceID { get; set; }
        public ActorType Type { get; set; }
        public DateTimeOffset SpawnTime { get; set; } = DateTimeOffset.UtcNow;

        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        public int DespawnTime { get; set; }  = -1;
        public bool Despawn { get; set; }  = true;

        public Actor(int ID, ActorType type, Vector3 position, Vector3 entRot = default)
        {
            InstanceID = ID;
            Type = type;
            Position = position;
            if (entRot != default)
                Rotation = entRot;
            else
                Rotation = Vector3.Zero;
        }

        public virtual void OnUpdate() { }

    }

    public class RainCloud : Actor
    {
        public Vector3 toCenter;
        public float wanderDirection;
        public bool Static = false;

        public RainCloud(int ID, Vector3 entPos) : base(ID, ActorType.RAINCLOUD, Vector3.Zero)
        {
            Position = entPos;

            toCenter = (Position - new Vector3(30, 40, -50)).Normalized();
            wanderDirection = new Vector2(toCenter.X, toCenter.Z).Angle();

            Despawn = true;
            DespawnTime = 550;
        }

        public override void OnUpdate()
        {
            if (Static) return;

            Vector2 dir = new Vector2(-1, 0).Rotate(wanderDirection) * (0.17f / 6f);
            Position += new Vector3(dir.X, 0, dir.Y);
        }
    }
}
