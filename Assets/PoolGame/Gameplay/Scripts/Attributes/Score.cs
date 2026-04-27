using System;
using UnityEngine;
using Attribute = PoolGame.Game.Attribute.Attribute;

namespace PoolGame.Gameplay.Attributes
{
    public class Score : Attribute
    {
        private static readonly int NoScore = 0;
        
        private void Update()
        {
            Logwin.Log("Score", AttributeValue, "Score");
        }

        public override void DecreaseAttribute(int amount)
        {
            AttributeValue = Mathf.Max(AttributeValue - amount, NoScore);
        }

        public override void IncreaseAttribute(int amount)
        {
            AttributeValue += amount;
        }
    }
}