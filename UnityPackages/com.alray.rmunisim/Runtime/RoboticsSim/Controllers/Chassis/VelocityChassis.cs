using System;
using System.Collections;
using System.Numerics;
using com.alray.rmunisim.Contracts.Interfaces;


namespace com.alray.rmunisim.RoboticsSim.Infrastructure.Controllers.Chassis
{

    struct VelocityChassis2D : IChassis2D, IPollingUpdateBinder<UnityEngine.Transform>
    {
        Vector2 linearVelocity;
        float angularVelocity;

        readonly float IChassis2D.AngularVelocity => angularVelocity;
        readonly Vector2 IChassis2D.LinearVelocity => linearVelocity;

        public IPollingUpdateBinder<UnityEngine.Transform>.BinderContext? Context { get; set; }

        /// <summary>
        /// Set the chassis velocities.
        /// </summary>
        /// <param name="linearVelocity">m/s</param>
        /// <param name="angularVelocity">rad/s</param>
        public void SetVelocity(Vector2 linearVelocity, float angularVelocity)
        {
            this.linearVelocity = linearVelocity;
            this.angularVelocity = angularVelocity;
        }


        readonly void IPollingUpdateBinder<UnityEngine.Transform>.UpdateBinder()
        {
            Context.Value.BindTarget.position +=
                    new UnityEngine.Vector3(linearVelocity.X, linearVelocity.Y, 0) * Context.Value.UpdateInterval;
            Context.Value.BindTarget.Rotate(UnityEngine.Vector3.up, angularVelocity * Context.Value.UpdateInterval);
        }
    }
}