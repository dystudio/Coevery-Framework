﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Coevery.WebApi.Filters {
    public class CoeveryApiActionFilterDispatcher : IActionFilter {
        public bool AllowMultiple { get; private set; }
        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation) {
            var workContext = actionContext.ControllerContext.GetWorkContext();

            foreach (var actionFilter in workContext.Resolve<IEnumerable<IApiFilterProvider>>().OfType<IActionFilter>()) {
                continuation = () => actionFilter.ExecuteActionFilterAsync(actionContext, cancellationToken, continuation);
            }

            return await continuation();
        }
    }
}