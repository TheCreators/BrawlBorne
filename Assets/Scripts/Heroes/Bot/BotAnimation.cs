namespace Heroes.Bot
{
    public class BotAnimation : HeroAnimation
    {
        public float ForwardSpeed
        {
            set => Animator.SetFloat(ForwardSpeedHash, value);
        }

        public float RightSpeed
        {
            set => Animator.SetFloat(RightSpeedHash, value);
        }

        public bool IsMoving
        {
            set => Animator.SetBool(IsMovingHash, value);
        }
    }
}