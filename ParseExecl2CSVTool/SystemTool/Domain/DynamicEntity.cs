using System.Collections.Generic;

namespace Domain
{
    public class DynamicEntity
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public object this[string key]
        {
            get
            {
                object value;
                values.TryGetValue(key, out value);
                return value;
            }
            set
            {
                if (value == null) values.Remove(key);
                else values[key] = value;
            }
        }
    }
}