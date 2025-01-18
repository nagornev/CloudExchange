using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudExchange.OperationResults
{
    public class ResultCommandBuilder
    {
        private List<ResultCommandDelegate> _commands;

        private Func<Task> _successfulCallback;

        private Func<Task> _failureCallback;

        private ResultCommandBuilder()
        {
            _commands = new List<ResultCommandDelegate>();
        }

        public static ResultCommandBuilder Create()
        {
            return new ResultCommandBuilder();
        }

        public ResultCommandBuilder AddCommand(ResultCommandDelegate command)
        { 
            _commands.Add(command);

            return this;
        }

        public ResultCommandBuilder SetSuccessfulCallback(Func<Task> callback)
        {
            _successfulCallback = callback;

            return this;
        }

        public ResultCommandBuilder SetFailureCallback(Func<Task> callback)
        {
            _failureCallback = callback;

            return this;
        }

        public ResultCommand Build()
        {
            return new ResultCommand(_commands,
                                     _successfulCallback,
                                     _failureCallback);
        }
    }
}
