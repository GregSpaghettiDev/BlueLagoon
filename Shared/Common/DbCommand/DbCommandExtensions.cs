using System;
using System.Data.Common;

namespace Common.DbCommand
{
    public static class DbCommandExtensions
    {
        public static DbParameter CreateDbCommandParam<T>(this System.Data.Common.DbCommand dbCommand, string paramName, dynamic paramValue)
        {
            var command = dbCommand;

            Type type = typeof(T);

            var param = command.CreateParameter();
            param.ParameterName = paramName;
            param.Value = Convert.ChangeType(paramValue, type);

            return param;
        }
    }
}