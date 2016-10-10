/**********************************************************************************************
 * Copyright 2010 YouSendIt, Inc. All Rights Reserved.
 *
 * Licensed under the API agreement. You may not use this file except in compliance with the 
 * License. 
 *
 * This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
 * either express or implied. See the API License for the specific language governing 
 * permissions and limitations under the License. 
 *
 * ********************************************************************************************
 */
using System;

namespace YouSendIt
{
    // C# uses ":" instead of "extends" to express inheritance.

    public class APIException : Exception
    {
        int httpStatusCode;
        String httpStatusMessage;

        // C# uses base() instead of super() to call superclass methods. Also, note this idiom for calling the superclass constructor.

        public APIException(int httpStatusCode, String httpStatusMessage, String errorMessage) : base(errorMessage)
            {
            this.httpStatusCode = httpStatusCode;
            this.httpStatusMessage = httpStatusMessage;
            }

        public override String ToString()
            {
            return "HTTP status code [" + httpStatusCode + "], HTTP status message [" + httpStatusMessage + "], " + Message;
            }
    }
}