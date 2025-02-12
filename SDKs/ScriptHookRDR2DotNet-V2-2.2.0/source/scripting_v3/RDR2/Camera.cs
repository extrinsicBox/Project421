//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;

namespace RDR2
{
	public sealed class Camera : PoolObject, ISpatial
	{
		public Camera(int handle) : base(handle)
		{
			Handle = handle;
		}

		public bool IsInterpolating => CAM.IS_CAM_INTERPOLATING(Handle);
		public bool IsShaking => CAM.IS_CAM_SHAKING(Handle);
		public bool IsRendering => CAM.IS_CAM_RENDERING(Handle);

		public bool Active
		{
			get => CAM.IS_CAM_ACTIVE(Handle);
			set => CAM.SET_CAM_ACTIVE(Handle, value);
		}

		public float FarClip
		{
			set => CAM.SET_CAM_FAR_CLIP(Handle, value);
		}

		public float NearClip
		{
			set => CAM.SET_CAM_NEAR_CLIP(Handle, value);
		}

		public float FieldOfView
		{
			get => CAM.GET_CAM_FOV(Handle);
			set => CAM.SET_CAM_FOV(Handle, value);
		}

		public float MotionBlurStrength
		{
			set => CAM.SET_CAM_MOTION_BLUR_STRENGTH(Handle, value);
		}

		public float FocusDistance
		{
			set => CAM._SET_CAM_FOCUS_DISTANCE(Handle, value);
		}



		public Vector3 Position
		{
			get => CAM.GET_CAM_COORD(Handle);
			set => CAM.SET_CAM_COORD(Handle, value.X, value.Y, value.Z);
		}

		public Vector3 Rotation
		{
			get => CAM.GET_CAM_ROT(Handle, 2);
			set => CAM.SET_CAM_ROT(Handle, value.X, value.Y, value.Z, 2);
		}

		public Vector3 Direction
		{
			get
			{
				Vector3 rot = Rotation;
				double rotX = rot.X / 57.295779513082320876798154814105;
				double rotZ = rot.Z / 57.295779513082320876798154814105;
				double multXY = System.Math.Abs(System.Math.Cos(rotX));

				return new Vector3((float)(-System.Math.Sin(rotZ) * multXY), (float)(System.Math.Cos(rotZ) * multXY), (float)System.Math.Sin(rotX));
			}
			set
			{
				value.Normalize();
				Vector3 vector1 = new Vector3(value.X, value.Y, 0.0f);
				Vector3 vector2 = new Vector3(value.Z, vector1.Length(), 0.0f);
				Vector3 vector3 = Vector3.Normalize(vector2);
				Rotation = new Vector3((float)(System.Math.Atan2(vector3.X, vector3.Y) * 57.295779513082320876798154814105), 0.0f, (float)(-System.Math.Atan2(value.X, value.Y) * 57.295779513082320876798154814105));
			}
		}

		public Vector3 ForwardVector => Direction;

		public Vector3 GetOffsetInWorldCoords(Vector3 offset)
		{
			Vector3 Forward = Direction;
			const double D2R = 0.01745329251994329576923690768489;
			double num1 = System.Math.Cos(Rotation.Y * D2R);
			double x = num1 * System.Math.Cos(-Rotation.Z * D2R);
			double y = num1 * System.Math.Sin(Rotation.Z * D2R);
			double z = System.Math.Sin(-Rotation.Y * D2R);
			Vector3 Right = new Vector3((float)x, (float)y, (float)z);
			Vector3 Up = Vector3.Cross(Right, Forward);
			return Position + (Right * offset.X) + (Forward * offset.Y) + (Up * offset.Z);
		}

		public Vector3 GetOffsetFromWorldCoords(Vector3 worldCoords)
		{
			Vector3 Forward = Direction;
			const double D2R = 0.01745329251994329576923690768489;
			double num1 = System.Math.Cos(Rotation.Y * D2R);
			double x = num1 * System.Math.Cos(-Rotation.Z * D2R);
			double y = num1 * System.Math.Sin(Rotation.Z * D2R);
			double z = System.Math.Sin(-Rotation.Y * D2R);
			Vector3 Right = new Vector3((float)x, (float)y, (float)z);
			Vector3 Up = Vector3.Cross(Right, Forward);
			Vector3 Delta = worldCoords - Position;
			return new Vector3(Vector3.Dot(Right, Delta), Vector3.Dot(Forward, Delta), Vector3.Dot(Up, Delta));
		}



		public void Render(bool bShouldRender, bool bEaseCam = false, int iEaseTime = 3000)
		{
			CAM.RENDER_SCRIPT_CAMS(bShouldRender, bEaseCam, iEaseTime, true, false, 0);
		}


		public void Shake(CameraShake shakeType, float amplitude)
		{
			CAM.SHAKE_CAM(Handle, shakeType.ToString(), amplitude);
		}

		public void StopShaking()
		{
			CAM.STOP_CAM_SHAKING(Handle, true);
		}



		public void PointAt(Vector3 target)
		{
			CAM.POINT_CAM_AT_COORD(Handle, target.X, target.Y, target.Z);
		}
		public void PointAt(Entity target)
		{
			CAM.POINT_CAM_AT_ENTITY(Handle, target.Handle, 0.0f, 0.0f, 0.0f, true);
		}
		public void PointAt(Entity target, Vector3 offset)
		{
			CAM.POINT_CAM_AT_ENTITY(Handle, target.Handle, offset.X, offset.Y, offset.Z, true);
		}

		public void StopPointing()
		{
			CAM.STOP_CAM_POINTING(Handle);
		}



		public void AttachTo(Entity entity, Vector3 offset)
		{
			CAM.ATTACH_CAM_TO_ENTITY(Handle, entity.Handle, offset.X, offset.Y, offset.Z, true);
		}
		public void AttachTo(Ped ped, int boneIndex, Vector3 offset)
		{
			CAM.ATTACH_CAM_TO_PED_BONE(Handle, ped.Handle, boneIndex, offset.X, offset.Y, offset.Z, true);
		}

		public void Detach()
		{
			CAM.DETACH_CAM(Handle);
		}



		public override void Delete()
		{
			CAM.DESTROY_CAM(Handle, false);
		}

		public override bool Exists()
		{
			return CAM.DOES_CAM_EXIST(Handle);
		}

		public static bool Exists(Camera camera)
		{
			return camera != null && camera.Exists();
		}

		public bool Equals(Camera obj)
		{
			return !(obj is null) && Handle == obj.Handle;
		}
		public override bool Equals(object obj)
		{
			return !(obj is null) && obj.GetType() == GetType() && Equals((Camera)obj);
		}

		public static bool operator ==(Camera left, Camera right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		public static bool operator !=(Camera left, Camera right)
		{
			return !(left == right);
		}

		public sealed override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}

	public enum CameraShake
	{
		NONE = -1,
		SMALL_EXPLOSION_SHAKE,
		MEDIUM_EXPLOSION_SHAKE,
		LARGE_EXPLOSION_SHAKE,
		HAND_SHAKE,
		JOLT_SHAKE,
		VIBRATE_SHAKE,
		WOBBLY_SHAKE,
		DRUNK_SHAKE,
	}
}
