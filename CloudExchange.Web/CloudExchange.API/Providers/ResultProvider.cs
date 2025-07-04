﻿using CloudExchange.API.Abstractions.Providers;
using CloudExchange.OperationResults;

namespace CloudExchange.API.Providers
{
    public class ResultProvider : IResultProvider
    {
        private readonly ResultSuccessFactory _successFactory;

        private readonly ResultFailedFactory _failedFactory;

        public ResultProvider(ResultSuccessFactory successFactory, ResultFailedFactory failedFactory)
        {
            _successFactory = successFactory;
            _failedFactory = failedFactory;
        }

        public IResult GetResult(Result result)
        {
            return result.IsSuccess ?
                    _successFactory.GetResult(result) :
                    _failedFactory.GetResult(result);
        }

        public IResult GetResult<T>(Result<T> result)
        {
            return result.IsSuccess ?
                    _successFactory.GetResult(result) :
                    _failedFactory.GetResult(result);
        }
    }
}
