namespace HandcraftedGames.AgentController.Abilities
{
    public interface IChangeSpeedAbility: IAbility
    {
        bool SetSpeedMultiplier(float multiplier);
    }
}