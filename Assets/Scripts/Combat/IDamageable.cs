﻿using Heroes;

namespace Combat
{
    public interface IDamageable
    {
        void TakeDamage(float damage, Hero dealer);
    }
}