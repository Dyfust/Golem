using FSM;

namespace OrbStates
{
    public class IdleState : State
    {
        private Orb _orb;

        public IdleState(Orb orb) : base("Idle State")
        {
            _orb = orb;
        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

        public override void UpdateLogic()
        {

        }

        public override void UpdatePhysics()
        {

        }
    }

    public class RollingState : State
    {
        private Orb _orb;
        private AudioEmitter _rollingSFX;

        public RollingState(Orb orb, AudioEmitter rollingSFX) : base("Rolling State")
        {
            _orb = orb;
            _rollingSFX = rollingSFX;
        }

        public override void OnEnter()
        {
            _rollingSFX.Play();
        }

        public override void OnExit()
        {
            _rollingSFX.Stop();
        }

        public override void UpdateLogic()
        {
            if (_orb.IsGrounded())
                _rollingSFX.SetValue(_orb.GetVelocity().magnitude / _orb.GetMaxSpeed());
            else
                _rollingSFX.SetValue(0f);
        }

        public override void UpdatePhysics()
        {
            _orb.Move();
            _orb.Roll();
        }
    }

    public class MountedState : State
    {
        private Orb _orb;

        public MountedState(Orb orb) : base("Mounted State")
        {
            _orb = orb;
        }

        public override void OnEnter()
        {
            _orb.ResetMovement();
            _orb.EnterGolem();
        }

        public override void OnExit()
        {
            _orb.ResetMovement();
            _orb.ExitGolem();
        }

        public override void UpdateLogic()
        {
        }

        public override void UpdatePhysics()
        {
            _orb.OrientateToGolem();
        }
    }
}
