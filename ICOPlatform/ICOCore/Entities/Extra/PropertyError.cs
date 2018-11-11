using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Entities.Extra
{
    class PropertyError
    {
        public PropertyError() { }
        public PropertyError(string propertyName, string errorMessage)
        {
            this.PropertyName = propertyName;
            this.ErrorMessage = errorMessage;
        }

        public string PropertyName { set; get; }
        public string ErrorMessage { set; get; }
    }
}
