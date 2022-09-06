using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Std20EasyArchitect.ApiHostBase;

namespace Lab2WebCore21App1.Controllers
{
    [Route("api/{fileName}/{nameSpace}/{className}/{methodName}/{*pathInfo}")]
    public class ValuesController : ApiHostBase
    {
    }
}
