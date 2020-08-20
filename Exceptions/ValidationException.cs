using System;
using System.Collections.Generic;

namespace ECS.Framework.Exceptions
{
    public class ValidationException : Exception
    {
        private readonly Dictionary<string, string> _validationErrors;

        public Dictionary<string, string> Errors { get { return _validationErrors; } }

        public string ReturnMessages
        {
            get
            {
                var msg = "[";
                foreach (var erro in Errors)
                {
                    msg += "{" + String.Format("\"key\":\"{0}\",\"value\":\"{1}\"", erro.Key, erro.Value) + "},";
                }
                return msg.Substring(0, msg.Length - 1) + "]";
            }
        }

        public ValidationException(string message)
            : base(message)
        {
            _validationErrors = new Dictionary<string, string>();
        }

        public void AddValidationError(string key, string message)
        {
            if (!_validationErrors.ContainsKey(key))
                _validationErrors.Add(key, message);
        }

        public bool HasErrors { get { return _validationErrors.Count > 0; } }
    }
}
