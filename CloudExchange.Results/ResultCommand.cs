using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudExchange.OperationResults
{
    public class ResultCommand
    {
        private Result _result;

        private IReadOnlyCollection<ResultCommandDelegate> _commands;

        private Func<Task> _successfulCallback;

        private Func<Task> _failureCallback;

        public ResultCommand(IReadOnlyCollection<ResultCommandDelegate> commands,
                             Func<Task> successfulCallback = default,
                             Func<Task> failureCallback = default)
        {
            _commands = commands;
            _successfulCallback = successfulCallback;
            _failureCallback = failureCallback;
        }

        public async Task<Result> Invoke()
        {
            if (_result != null)
                return _result;

            _result = await InternalInvoke();

            return _result;
        }

        private async Task<Result> InternalInvoke()
        {
            Result result = await ExecuteCommands();

            switch (result.Success)
            {
                case true:
                    await _successfulCallback?.Invoke();
                    break;

                case false:
                    await _failureCallback?.Invoke();
                    break;
            }

            return result;
        }

        private async Task<Result> ExecuteCommands()
        {
            foreach (ResultCommandDelegate command in _commands)
            {
                Result result = await command.Invoke();

                if (!result.Success)
                    return result;
            }

            return Result.Successful();
        }
    }
}
