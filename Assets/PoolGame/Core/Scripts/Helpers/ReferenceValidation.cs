using System.Collections.Generic;
using UnityEngine;

namespace PoolGame.Core.Helpers
{
    public static class ReferenceValidation
    {
        public static void LogMissing(Object context, string ownerName, params (string name, Object value)[] references)
        {
            List<string> missing = new();

            foreach ((string name, Object value) in references)
            {
                if (value == null)
                    missing.Add($"<color=yellow>{name}</color>");
            }

            if (missing.Count == 0)
                return;

            Debug.LogError(
                $"[{ownerName}] Missing references: {string.Join(", ", missing)}",
                context
            );
        }
    }
}