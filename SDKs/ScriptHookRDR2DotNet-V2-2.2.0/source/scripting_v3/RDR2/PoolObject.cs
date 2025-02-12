﻿//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;

namespace RDR2
{
	/// <summary>
	/// An object with position and rotation information.
	/// </summary>
	public interface ISpatial
	{
		Vector3 Position { get; set; }
		Vector3 Rotation { get; set; }
	}

	/// <summary>
	/// An object that can exist in the world.
	/// </summary>
	public interface IExistable
	{
		bool Exists();
	}

	/// <summary>
	/// An object that can be deleted from the world.
	/// </summary>
	public interface IDeletable : IExistable
	{
		void Delete();
	}

	/// <summary>
	/// An object that resides in one of the available object pools.
	/// </summary>
	public abstract class PoolObject : INativeValue, IDeletable
	{
		protected PoolObject(int handle)
		{
			Handle = handle;
		}

		/// <summary>
		/// The handle of the object.
		/// </summary>
		public int Handle
		{
			get; protected set;
		}

		/// <summary>
		/// The handle of the object translated to a native value.
		/// </summary>
		public ulong NativeValue
		{
			get => (ulong)Handle;
			set => Handle = unchecked((int)value);
		}

		public abstract bool Exists();
		public abstract void Delete();

		public static implicit operator int(PoolObject e)
		{
			// If the PoolObject is not null, then we can safely return it's handle.
			// Otherwise, return 0, which is NULL in C++
			if (e != null)
			{
				return e.Handle;
			}

			return 0;
		}
	}
}
