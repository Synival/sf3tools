using System;
using System.ComponentModel;
using System.Linq;
using OpenTK.Mathematics;
using SF3.Win.Extensions;
using SF3.Win.OpenGL.MPD_File;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerGLControl {
        private static Vector3? s_lastPosition = null;
        private static float? s_lastPitch      = null;
        private static float? s_lastYaw        = null;

        public void LookAtTarget(Vector3 target) {
            // Calculate the pitch/yaw to point the camera to that target.
            var posMinusTarget = Position - target;
            Pitch = -MathHelper.RadiansToDegrees((float) Math.Atan2(posMinusTarget.Y, double.Hypot(posMinusTarget.X, posMinusTarget.Z)));
            Yaw   =  MathHelper.RadiansToDegrees((float) Math.Atan2(posMinusTarget.X, posMinusTarget.Z));
        }

        public void LookAtCurrentTileTarget() {
            var targetDist = GetCurrentTileTargetAndDistance();
            if (!targetDist.HasValue)
                return;

            LookAtTarget(targetDist.Value.Target);
            Invalidate();
        }

        public void PanToCurrentTileTarget() {
            var targetDist = GetCurrentTileTargetAndDistance();
            if (!targetDist.HasValue)
                return;

            var distVec = new Vector3(0, 0, targetDist.Value.Distance);
            var distForward = -distVec
                * Matrix3.CreateRotationX(MathHelper.DegreesToRadians(Pitch))
                * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(Yaw));

            Position = targetDist.Value.Target - distForward;
            Invalidate();
        }

        public void MoveCameraForward(float amount) {
            Position += new Vector3(0, 0, amount)
                * Matrix3.CreateRotationX(MathHelper.DegreesToRadians(Pitch))
                * Matrix3.CreateRotationY(MathHelper.DegreesToRadians(Yaw));
            Invalidate();
        }

        public struct TargetAndDistance {
            public Vector3 Target;
            public float Distance;
        }

        public TargetAndDistance? GetCurrentTileTargetAndDistance() {
            if (!_tileHoverPos.HasValue)
                return null;

            var tile = MPD_File.Tiles[_tileHoverPos.Value.X, _tileHoverPos.Value.Y];
            var tileVertices = tile.GetSurfaceModelVertices();
            var target = new Vector3(
                _tileHoverPos.Value.X + WorldResources.ModelOffsetX + 0.5f,
                tileVertices.Select(x => x.Y).Average(),
                (63 - _tileHoverPos.Value.Y) + WorldResources.ModelOffsetZ + 0.5f);

            var dist = (float) Math.Sqrt(
                Math.Pow(target.X - Position.X, 2) +
                Math.Pow(target.Y - Position.Y, 2) +
                Math.Pow(target.Z - Position.Z, 2)
            );

            return new TargetAndDistance { Target = target, Distance = dist };
        }

        public void ResetCamera()
            => ResetCamera((0, 5, 0), 140.0f);

        public void ResetCamera(Vector3 offset, float distance) {
            Position = new Vector3(0.0f, 0.416f, 0.909f) * distance + offset;
            LookAtTarget(offset);
        }

        private void SetInitialCameraPosition() {
            if (!s_lastPosition.HasValue || !s_lastPitch.HasValue || !s_lastYaw.HasValue)
                ResetCamera();
            else {
                Position = s_lastPosition.Value;
                Pitch    = s_lastPitch.Value;
                Yaw      = s_lastYaw.Value;
            }
        }

        private Vector3 _position = new Vector3(0, 0, 0);

        /// <summary>
        /// Position of the camera
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Vector3 Position {
            get => _position;
            set {
                if (value != _position) {
                    _position = value;
                    s_lastPosition = value;
                }
            }
        }

        private float _pitch = 0;

        /// <summary>
        /// Pitch, in degrees
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float Pitch {
            get => _pitch;
            set {
                _pitch = Math.Clamp(value, -90, 90);
                s_lastPitch = _pitch;
                _renderer.InvalidateSpriteMatrices(_models);
            }
        }

        private float _yaw = 0;

        /// <summary>
        /// Yaw, in degrees
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float Yaw {
            get => _yaw;
            set {
                _yaw = value - 360.0f * ((int) value / 360);
                s_lastYaw = _yaw;
                _renderer.InvalidateSpriteMatrices(_models);
            }
        }
    }
}
