using Misc.StateMachine;

namespace Heroes.Bot.States
{
    public class ChooseBoost : IState
    
    {
        private readonly Bot _bot;
        private readonly BotSensor _botSensor;

        public ChooseBoost(Bot bot, BotSensor botSensor)
        {
            _bot = bot;
            _botSensor = botSensor;
        }

        public void Tick()
        {
            _bot.Boost = _botSensor.ClosestBoostInDetectionRange;
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}