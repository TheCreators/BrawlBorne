namespace Models
{
    public record HealthAmount(float CurrentHealth, float MaxHealth)
    {
        public float CurrentHealth { get; set; } = CurrentHealth;

        public float MaxHealth { get; set; } = MaxHealth;
    }
}