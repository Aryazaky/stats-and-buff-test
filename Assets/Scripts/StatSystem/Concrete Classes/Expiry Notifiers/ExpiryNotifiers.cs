using System;
using System.Collections.Generic;
using UnityEngine;
using StatSystem;
using StatSystem.Modifiers;

namespace StatSystem.Concrete_Classes.Expiry_Notifiers
{
    public class InvokeLimitExpiryNotifier : Modifier.IExpiryNotifier
    {
        private readonly int _invokeLimit;
        private readonly List<Modifier> _trackedModifiers = new();

        public InvokeLimitExpiryNotifier(int invokeLimit)
        {
            if (invokeLimit <= 0)
                throw new ArgumentException("Invoke limit must be greater than zero.", nameof(invokeLimit));

            _invokeLimit = invokeLimit;
        }

        public void TrackModifier(Modifier modifier)
        {
            if (_trackedModifiers.Contains(modifier))
                throw new InvalidOperationException("This modifier is already being tracked.");

            _trackedModifiers.Add(modifier);

            modifier.OnInvoke += CheckLimit;
            modifier.OnDispose += RemoveModifier;
        }

        public void CheckLimit()
        {
            foreach (var modifier in _trackedModifiers)
            {
                CheckLimit(modifier);
            }
        }

        private void CheckLimit(Modifier modifier)
        {
            if (modifier.InvokedCount >= _invokeLimit)
            {
                modifier.Dispose();
            }
        }

        private void RemoveModifier(Modifier modifier)
        {
            _trackedModifiers.Remove(modifier);
            modifier.OnInvoke -= CheckLimit;
            modifier.OnDispose -= RemoveModifier;
        }
    }

    public class TimeLimitExpiryNotifier : Modifier.IExpiryNotifier
    {
        private readonly float _timeLimit;
        private readonly List<Modifier> _trackedModifiers = new();

        public TimeLimitExpiryNotifier(float timeLimit)
        {
            if (timeLimit <= 0)
                throw new ArgumentException("Time limit must be greater than zero.", nameof(timeLimit));

            _timeLimit = timeLimit;
        }

        public void TrackModifier(Modifier modifier)
        {
            if (_trackedModifiers.Contains(modifier))
                throw new InvalidOperationException("This modifier is already being tracked.");

            _trackedModifiers.Add(modifier);

            modifier.OnTryInvoke += CheckLimit;
            modifier.OnDispose += RemoveModifier;
        }

        public void CheckLimit()
        {
            foreach (var modifier in _trackedModifiers)
            {
                CheckLimit(modifier);
            }
        }

        private void CheckLimit(Modifier modifier)
        {
            if (Time.time - modifier.CreatedTime >= _timeLimit)
            {
                modifier.Dispose();
            }
        }

        private void RemoveModifier(Modifier modifier)
        {
            _trackedModifiers.Remove(modifier);
            modifier.OnTryInvoke -= CheckLimit;
            modifier.OnDispose -= RemoveModifier;
        }
    }
}