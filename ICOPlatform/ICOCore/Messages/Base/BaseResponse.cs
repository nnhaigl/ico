using ICOCore.Entities.Extra;
using ICOCore.Infrastructures.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.Messages.Base
{
    public class BaseResponse
    {
        #region Constructors
        public BaseResponse()
        {
            PropertyErrors = new List<PropertyError>();
            IsSuccess = true;
            ResultCode = ResultCode.OK;
        }

        public BaseResponse(List<PropertyError> propertyErrors)
        {
            PropertyErrors = propertyErrors;
            IsSuccess = true;
        }

        #endregion

        #region Properties & Methods
        public bool IsSuccess { get; set; }

        public string Message { get; set; }
        public List<PropertyError> PropertyErrors { get; set; }
        public void AddPropertyError(string propertyName, string errorMessage)
        {
            PropertyErrors.Add(new PropertyError { PropertyName = propertyName, ErrorMessage = errorMessage });
        }

        public ResultCode ResultCode { set; get; }

        public bool IsValid
        {
            get
            {
                return !PropertyErrors.Any();
            }
        }
        #endregion

    }
}
