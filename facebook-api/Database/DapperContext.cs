﻿using Microsoft.Data.SqlClient;
using System.Data;

namespace facebook_api.Database
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
