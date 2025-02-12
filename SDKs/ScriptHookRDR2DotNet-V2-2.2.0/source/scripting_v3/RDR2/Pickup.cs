//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;

namespace RDR2
{
	public sealed class Pickup : PoolObject
	{
		public Pickup(int handle) : base(handle)
		{
			Handle = handle;
		}

		public Vector3 Position => OBJECT.GET_PICKUP_COORDS(Handle);
		public Prop Object => new Prop(OBJECT.GET_PICKUP_OBJECT(Handle));
		public bool IsCollected => OBJECT.HAS_PICKUP_BEEN_COLLECTED(Handle);

		public bool ObjectExists()
		{
			return OBJECT.DOES_PICKUP_OBJECT_EXIST(Handle);
		}

		public override void Delete()
		{
			OBJECT.REMOVE_PICKUP(Handle);
		}

		public override bool Exists()
		{
			return OBJECT.DOES_PICKUP_EXIST(Handle);
		}

		public static bool Exists(Pickup pickup)
		{
			return pickup != null && pickup.Exists();
		}

		public bool Equals(Pickup obj)
		{
			return !(obj is null) && Handle == obj.Handle;
		}
		public sealed override bool Equals(object obj)
		{
			return !(obj is null) && obj.GetType() == GetType() && Equals((Pickup)obj);
		}

		public static bool operator ==(Pickup left, Pickup right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		public static bool operator !=(Pickup left, Pickup right)
		{
			return !(left == right);
		}

		public sealed override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
