using Misc.StateMachine;

namespace Heroes.Bot.States
{
    public class ChooseEnemy : IState
    
    {
        private readonly Bot _bot;
        private readonly BotSensor _botSensor;

        public ChooseEnemy(Bot bot, BotSensor botSensor)
        {
            _bot = bot;
            _botSensor = botSensor;
        }

        public void Tick()
        {
            _bot.Enemy = _botSensor.ClosestEnemyInDetectionRange;
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}