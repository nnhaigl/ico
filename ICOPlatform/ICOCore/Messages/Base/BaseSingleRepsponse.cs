using ICOCore.Entities.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Messages.Base
{
    public class BaseSingleResponse<T> : BaseResponse
    {
        #region Constructors

        public BaseSingleResponse()
        {
            IsSuccess = true;
            PropertyErrors = new List<PropertyError>();
        }

        public BaseSingleResponse(T value)
        {
            this.Value = value;
            IsSuccess = true;
            PropertyErrors = new List<PropertyError>();
        }

        public BaseSingleResponse(T value, List<PropertyError> propertyErrors)
        {
            this.Value = value;
            PropertyErrors = propertyErrors;
            IsSuccess = true;
        }

        #endregion

        #region Properties
        public T Value { get; set; }

        #endregion

    }
}
