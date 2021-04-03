using CloudNative.CloudEvents;
using System;
using System.Collections.Generic;

namespace Shared
{
    public class RetryAttemptCloudEventExtension : ICloudEventExtension
    {
        public const string AttemptsAttributeName = "attempts";

        private readonly IDictionary<string, object> attributes = new Dictionary<string, object>();

        public uint Attempts
        {
            get => (uint) attributes[AttemptsAttributeName];
            set => attributes[AttemptsAttributeName] = value;
        }

        public RetryAttemptCloudEventExtension(): this(0)
        { }

        public RetryAttemptCloudEventExtension(uint attempts)
        {
            Attempts = attempts;
        }

        public void Attach(CloudEvent cloudEvent)
        {
            var eventAttributes = cloudEvent.GetAttributes();
            if (attributes == eventAttributes)
            {
                return;
            }

            foreach (var attr in attributes)
            {
                if (attr.Value != null)
                {
                    eventAttributes[attr.Key] = attr.Value;
                }
            }
        }

        public bool ValidateAndNormalize(string key, ref dynamic value)
        {
            switch (key)
            {
                case AttemptsAttributeName:
                    if (value is uint)
                    {
                        return true;
                    }

                    throw new InvalidOperationException("Attempts value is not a uint");
                default:
                    return false;
            }
        }

        public Type GetAttributeType(string name)
        {
            switch (name)
            {
                case AttemptsAttributeName:
                    return typeof(uint);
            }

            return null;
        }
    }
}
