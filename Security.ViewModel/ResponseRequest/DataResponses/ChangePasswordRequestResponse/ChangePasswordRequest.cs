﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Service.ViewModel.ResponseRequest.DataResponses.ChangePasswordRequestResponse
{
    public class ChangePasswordRequest: IRequestData<ChangePasswordResponse>
    {
        public ChangePassword ChangePassword { get; set; }
    }
}
